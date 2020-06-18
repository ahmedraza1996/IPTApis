using IptApis.Shared;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Http;

namespace IptApis.Controllers.Cafeteria
{
    public class TestCafeteriaController : ApiController
    {
        public string Get()
        {
            return "Hello this is cafeteria";
        }
        [HttpGet]
        public string testconnection()
        {
            var db = DbUtils.GetDBConnection();
            
            if (db.Connection.State == ConnectionState.Open)
            {
                return ("connection success");
            }
            else
            {
                db.Connection.Open();
            }
            return ("test connection");

        }
        public async Task<HttpResponseMessage> PostUserImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            Dictionary<string, object> product = new Dictionary<string, object>();
            if (!Request.Content.IsMimeMultipartContent())
            {
                return   Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }
            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();
           
            var contentMP = filesReadToProvider.Contents;
           foreach (var item in contentMP)
            {
                if(item.Headers.ContentType.MediaType== "application/json")
                {
                    product[item.Headers.ContentDisposition.Name.ToString().Replace("\"","")]= item.ReadAsStringAsync().Result;
                 

                }
                else
                {
                    var fileBytes = await item.ReadAsByteArrayAsync();
                    int MaxContentLength = 1024 * 1024 * 5; //Size = 1 MB
                    IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                    var filename = item.Headers.ContentDisposition.FileName;
                    filename = filename.Replace("\"", "");

                    var ext = filename.Substring(filename.LastIndexOf('.'));
                    var extension = ext.ToLower();
                   // extension= extension.Remove(extension.Length - 1, 1);
                    if (!AllowedFileExtensions.Contains(extension))
                    {
                        var message = string.Format("Please Upload image of type .jpg,.gif,.png.");
                        dict.Add("error", message);

                        return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                    }
                    else if (item.Headers.ContentLength > MaxContentLength)
                    {

                        var message = string.Format("Please Upload a file upto 1 mb.");

                        dict.Add("error", message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                    }
                    else
                    {
                        var httpPostedFile = HttpContext.Current.Request.Files["image"];

                        string relativepath = "~/productimage/" + product["ItemName"]+extension;
                        product["filepath"] = relativepath;
                        var filePath = HttpContext.Current.Server.MapPath(relativepath);
                        httpPostedFile.SaveAs(filePath);
                      
                    }
                }
            }
            //db add


            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            // using (var scope = db.Connection.BeginTransaction())
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {

                    var res = db.Query("fooditem").Insert(product);   
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, object>() { { "LastInsertedId", res } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();   //if there are any error, rollback the transaction
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }

           
        }


        public HttpResponseMessage GetProductWithImage()
        {
            string relativepath = "~/productimage/" + "Bojack.jpg";
            var filePath = HttpContext.Current.Server.MapPath(relativepath);

            var Response = Request.CreateResponse(HttpStatusCode.OK);
            var path = "~/";
            byte[] contents = System.IO.File.ReadAllBytes(filePath);
            
                var base64string = Convert.ToBase64String(contents);
                //System.IO.MemoryStream ms = new System.IO.MemoryStream(contents);
                //StreamContent image = new StreamContent(base64string);
                //Response.Content = image;
                //Response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["base64string"] = base64string;

                return Request.CreateResponse(HttpStatusCode.OK, dict);

            
           
        }


    }
}
