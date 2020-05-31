using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IptApis.Models.JobPortal;
using IptApis.Shared;
using SqlKata.Execution;
namespace IptApis.Controllers.JobPortal
{
    public class JobController : ApiController
    {
        public HttpResponseMessage GetAllJobs()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<Job> response = db.Query("Job").Get<Job>();//;.Cast<ProjectModel>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
     }
}
