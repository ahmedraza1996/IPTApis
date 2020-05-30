using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IptApis.Models.JobPortal;
using IptApis.Shared;
using Newtonsoft.Json.Linq;
using SqlKata;
using SqlKata.Execution;
namespace IptApis.Controllers.JobPortal
{
    public class JobController : ApiController
    {

        [HttpGet]
        [Route("api/job/detail/{id}")]
        public HttpResponseMessage GetDetail(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<JobDetails> response = db.Query("JobDetails").Where("jobid", "=", id).Get < JobDetails>();//;.Cast<ProjectModel>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        [Route("api/jobs")]
        public HttpResponseMessage GetAllJobs()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<Job> response = db.Query("Job").Get<Job>();//;.Cast<ProjectModel>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

<<<<<<< HEAD
     }
=======
        [HttpPost]
        [Route("api/addJob")]
        public int AddJob([FromBody]JobDetails newJob)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            int jobid = db.Query("Job").InsertGetId<int>(new
            {
                newJob.Title,
                newJob.Organization,
                newJob.LastApplyDate,
                newJob.Designation,
                newJob.MinExperience,
                newJob.AttachmentPath,
                newJob.ApplicationLink,
                newJob.Contactperson
            });
            foreach(string description in newJob.DescriptionList)
            {
                db.Query("JobDescription").InsertGetId<int>(new { jobid, description });
            }
            return jobid;
        }

        [HttpPost]
        [Route("api/apply")]
        public void ApplyFor([FromBody] AppliedFor studentJobID)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            int x = db.Query("AppliedFor").InsertGetId<int>(new
            {
                studentJobID.StudentId,
                studentJobID.JobId
            });
        }
}
