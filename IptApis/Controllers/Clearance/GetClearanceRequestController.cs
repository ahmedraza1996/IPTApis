using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Transactions;
using IptApis.Shared;

namespace IptApis.Controllers.Clearance
{
    public class GetClearanceRequestController : ApiController
    {
        //Method to get Clearance Requests where Status column of clearance request is Null.
        public HttpResponseMessage GetAllClearanceRequests()
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("ClearanceRequest").Where("Status", "NULL").Get().Cast<IDictionary<string, object>>();  //get product by id=1
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        //Method to get Clearance Request by Id
        public HttpResponseMessage GetClearanceRequestbyID(string id)
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("ClearanceRequest").Where("RequestID", id).Get().Cast<IDictionary<string, object>>();  //get product by id=1
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

    }
}
