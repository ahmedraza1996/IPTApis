using IptApis.Models.FacultyRecruitment;
using IptApis.Shared;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;

namespace IptApis.Controllers.FacultyRecruitment
{
    public class NucesJobController : ApiController
    {
        //Get All JobOpenings

        [HttpGet]
        public HttpResponseMessage GetAllOpenings()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<JobOpening> response = db.Query("JobOpening").Get<JobOpening>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        public HttpResponseMessage AddJobOpening(JobOpening job)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var id = db.Query("JobOpening").InsertGetId<int>(new
                    {
                        MinExperience = job.MinExperience,
                        JobDescription = job.JobDescription,
                        DatePosted = job.DatePosted,
                        ExpectedStartDate = job.ExpectedStartDate,
                        DesignationID = job.DesignationID
                    });
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, id);//, new Dictionary<string, object>() { { "LastInsertedId", res } });
                }
                catch(Exception ex)
                {
                    scope.Dispose();   //if there are any error, rollback the transaction
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }


        [HttpDelete]
        public HttpResponseMessage DeleteJobOpening(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            try
            {
                _ = db.Query("JobOpening").Where("JobID", "=", id).Delete();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateJobOpening(JobOpening job)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            try
            {

                int affected = db.Query("JobOpening").Where("Id", job.JobID).Update(new
                {
                    MinExperience = job.MinExperience,
                    JobDescription = job.JobDescription,
                    DatePosted = job.DatePosted,
                    ExpectedStartDate = job.ExpectedStartDate,
                    DesignationID = job.DesignationID
                });
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }






    }
}
