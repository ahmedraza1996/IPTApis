using System;
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
    public class GetRequestApprovalController : ApiController
    {
        //Method to get request approval by ID
        public HttpResponseMessage GetRequestApprovalbyID(string id)
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("RequestApproval").Where("RequestID", id).Get().Cast<IDictionary<string, object>>();  //get product by id=1
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

    }
}
