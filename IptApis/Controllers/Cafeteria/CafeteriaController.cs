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
using System.Web.Http;

namespace IptApis.Controllers.Cafeteria
{
    public class CafeteriaController : ApiController
    {

        public HttpResponseMessage GetProduct()
        {
            var db = DbUtils.GetDBConnection();   //GetDBConnection will return a DBFactory type object, which have establish sql connection
            db.Connection.Open();
            //var res = db.Query("fooditem").Get();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("fooditem").Get().Cast<IDictionary<string, object>>();   // gets all food items from table and cast it to list of dictionary
            //you can use model instead of dictionary
           

            //var strResponse = response.ElementAt(0).ToString().Replace("DapperRow,", "").Replace("=", ":");
            
            //Dictionary<string, object> temp = JsonConvert.DeserializeObject<Dictionary<string, object>>(strResponse);
            return Request.CreateResponse(HttpStatusCode.OK, response);   //send list of items and Status code =200
        }
        public HttpResponseMessage GetProductbyID()
        {
         
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("fooditem").Where("ItemId", 1).Get().Cast<IDictionary<string, object>>();  //get product by id=1
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
