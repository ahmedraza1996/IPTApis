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
            var connection = DbUtils.GetDBConnection();

            var compiler = new SqlServerCompiler();
            var db = new QueryFactory(connection, compiler);
            db.Connection.Open();
            //var res = db.Query("fooditem").Get();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("fooditem").Get().Cast<IDictionary<string, object>>(); ;
            //foreach (var row in response)
            //{

            //}
            //var strResponse = response.ElementAt(0).ToString().Replace("DapperRow,", "").Replace("=", ":");
            
            //Dictionary<string, object> temp = JsonConvert.DeserializeObject<Dictionary<string, object>>(strResponse);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
