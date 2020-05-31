using IptApis.Shared;
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
using System.Transactions;
using System.Web;
using System.Web.Http;

namespace IptApis.Controllers.Cafeteria
{
    
    public class CafeteriaController : ApiController
    {

        public HttpResponseMessage GetItems()
        {
            var db = DbUtils.GetDBConnection();   //GetDBConnection will return a DBFactory type object, which have establish sql connection
            db.Connection.Open();
            //var res = db.Query("fooditem").Get();


            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("fooditem").Get().Cast<IDictionary<string, object>>();   // gets all food items from table and cast it to list of dictionary
            //you can use model instead of dictionary
           
            foreach( var item in response)
            { 
                
                item["base64image"] =fileToBase64(item["filepath"]) ; 
            }

            //var strResponse = response.ElementAt(0).ToString().Replace("DapperRow,", "").Replace("=", ":");
            //comment
            //Dictionary<string, object> temp = JsonConvert.DeserializeObject<Dictionary<string, object>>(strResponse);
            return this.Request.CreateResponse(HttpStatusCode.OK, response);   //send list of items and Status code =200
        }
        public string fileToBase64(object relativePath)
        {
           
            string base64string = "";
            if (!(relativePath is null)) {
                var filePath = HttpContext.Current.Server.MapPath(relativePath.ToString());

                byte[] contents = System.IO.File.ReadAllBytes(filePath);

                base64string = Convert.ToBase64String(contents);
            }
         
            return base64string;
        }

        public HttpResponseMessage GetAvailableItems()
        {
            var db = DbUtils.GetDBConnection(); 
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("fooditem").Where("ItemStatus", "Available").Get().Cast<IDictionary<string, object>>();
            foreach (var item in response)
            {
                item["base64image"] = fileToBase64(item["filepath"]);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, response);
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
        public HttpResponseMessage ViewOrders()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("FoodOrder").Where("OrderStatus", "Pending").OrderByDesc("OrderDate").OrderByDesc("OrderTime").Get().Cast<IDictionary<string, object>>();
            foreach (var item in response)
            {
                object OrderId;
                item.TryGetValue("OrderID", out OrderId);
                IEnumerable<IDictionary<string, object>> OrderDetails;
                /* SELECT
    [itemname],
   [qunatity]
         FROM
   [OrderDetails]
   INNER JOIN[fooditem] ON[orderdetails].[itemid] = [fooditem].[itemid]
         WHERE
 [orderid] = 1 */
                OrderDetails = db.Query("OrderDetails")
                    .Select("ODID", "itemname", "qunatity")
                    .Where("orderid", OrderId)
                    .Join("fooditem", "orderdetails.itemid", "fooditem.itemid")
                    .Get()
                    .Cast<IDictionary<string, object>>();
                item["OrderDetails"] = OrderDetails;

            }

            return this.Request.CreateResponse(HttpStatusCode.OK, response);

        }

    }
}
