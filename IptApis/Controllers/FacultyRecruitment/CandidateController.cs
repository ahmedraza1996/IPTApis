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
    public class CandidateController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage AddCandidate(CandidateEmployee candidate)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var candidate_id = db.Query("CandidateEmployee").InsertGetId<int>(new
                    {
                        EName = candidate.EName,
                        Email = candidate.Email,
                        Epassword = candidate.Epassword,
                        EAddress = candidate.EAddress,
                        MobileNumber = candidate.MobileNumber,
                        EducationalLevel = candidate.EducationalLevel,
                        ExperienceYears = candidate.ExperienceYears,
                        CVPath = candidate.CVPath,
                        CoverLetterPath = candidate.CoverLetterPath
                    });
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, candidate_id);//, new Dictionary<string, object>() { { "LastInsertedId", res } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();   //if there are any error, rollback the transaction
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }


        [HttpGet]
        public HttpResponseMessage GetAllCandidates()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<CandidateEmployee> response = db.Query("CandidateEmployee").Get<CandidateEmployee>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetCandidateById(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<CandidateEmployee> response = db.Query("CandidateEmployee").Where("ECandidateID",id).Get<CandidateEmployee>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }



        [HttpPut]
        public HttpResponseMessage UpdateCandidate(CandidateEmployee candidate)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            try
            {

                int affected = db.Query("CandidateEmployee").Where("Id", candidate.ECandidateID).Update(new
                {
                    EName = candidate.EName,
                    Email = candidate.Email,
                    Epassword = candidate.Epassword,
                    EAddress = candidate.EAddress,
                    MobileNumber = candidate.MobileNumber,
                    EducationalLevel = candidate.EducationalLevel,
                    ExperienceYears = candidate.ExperienceYears,
                    CVPath = candidate.CVPath,
                    CoverLetterPath = candidate.CoverLetterPath
                });
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }


        [HttpDelete]
        public HttpResponseMessage DeleteCandidate(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            try
            {
                _ = db.Query("CandidateEmployee").Where("ECandidateID", "=", id).Delete();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
