using Newtonsoft.Json;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Transactions;
using IptApis.Shared;
using System.Threading.Tasks;
using System.Web;

namespace IptApis.Controllers.Cafeteria
{
    public class CafeteriaStaffController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage AddProduct(Object Product)
        {


            // string username = Thread.CurrentPrincipal.Identity.Name;

            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(Product));
            //below are optional,just to check if we get anynull value it will chage to their defaults
            object ItemName;
            test.TryGetValue("ItemName", out ItemName);   //hover onto trygetvalue, to see the method signature
            string _ItemName = Convert.ToString(ItemName);
            object ItemStatus;
            test.TryGetValue("ItemStatus", out ItemStatus);
            string _ItemStatus = Convert.ToString(ItemStatus);
            object IDescription;
            test.TryGetValue("IDescription", out IDescription);
            string _IDescription = Convert.ToString(IDescription);

            object Price;
            test.TryGetValue("Price", out Price);
            int _Price = Convert.ToInt32(Price);


            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            // using (var scope = db.Connection.BeginTransaction())
            using (TransactionScope scope = new TransactionScope())   
            {
                try
                {
                  
                var res = db.Query("fooditem").InsertGetId<int>(new
                {
                    ItemName = _ItemName,
                    ItemStatus = _ItemStatus,
                    IDescription = _IDescription,
                    Price = _Price
                });   //specify each field in form of dictionary or object

                    #region 

                    //var query = db.Query("propertydetail").AsInsert(test);

                    //

                    ////Inject the Identity in the Compiled Query SQL object
                    //

                    ////Name Binding house the values that the insert query needs 
                    //
                    //var res = db.Query("fooditem").AsInsert(fooditemobj);
                    //SqlKata.SqlResult compiledQuery = compiler.Compile(res);
                    //var sql = compiledQuery.Sql + "; SELECT @@IDENTITY as ID;";
                    //var IdentityKey = db.Select<string>(sql, compiledQuery.NamedBindings).FirstOrDefault();
                    //var insertedIdString = db.Select("SELECT @@IDENTITY");
                    //var res = db.Query("fooditem").InsertGetId<int>(fooditemobj);
                    //var res = db.Query()
                    //scope.Commit();
                    #endregion 
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
        public async Task<HttpResponseMessage> AddProductWithImage()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            Dictionary<string, object> product = new Dictionary<string, object>();
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }
            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

            var contentMP = filesReadToProvider.Contents;
            foreach (var item in contentMP)
            {
                if (item.Headers.ContentType.MediaType == "application/json")
                {
                    product[item.Headers.ContentDisposition.Name.ToString().Replace("\"", "")] = item.ReadAsStringAsync().Result;


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

                        string relativepath = "~/productimage/" + product["ItemName"] + extension;
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

    }
}
