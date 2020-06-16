using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IptApis.Shared;
using Newtonsoft.Json;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Configuration;
using System.Data.SqlClient;

namespace IptApis.Controllers.Clearance
{
    public class ProgDepartController : ApiController
    {
        //Method to get Programme by ID
        public HttpResponseMessage GetProgrammebyID(string id)
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Programme").Where("ProgrammeID", id).Get().Cast<IDictionary<string, object>>();  //get product by id=1
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        //Method to get Department by ID
        public HttpResponseMessage GetDepartmentbyID(string id)
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Department").Where("DepartmentID", id).Get().Cast<IDictionary<string, object>>();  //get product by id=1
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
