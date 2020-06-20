using IptApis.Models.FacultyRecruitment;
using IptApis.Shared;
using Newtonsoft.Json;
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
    public class EcandidateApplicationController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage GetAllEcandidateApplication()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<EcandidateApplication> response = db.Query("ECandidateApplication").Get<EcandidateApplication>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetEcandidateApplicationById(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<EcandidateApplication> response = db.Query("ECandidateApplication").Where("RefID", id).Get<EcandidateApplication>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        public HttpResponseMessage AddEcandidateApplication(Object candidate)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(candidate));

            object QualifiedStatus;
            test.TryGetValue("QualifiedStatus", out QualifiedStatus);
            string _QualifiedStatus = Convert.ToString(QualifiedStatus);

            object ApplyDate;
            test.TryGetValue("ApplyDate", out ApplyDate);
            string _ApplyDate = Convert.ToString(ApplyDate);

            object TestDate;
            test.TryGetValue("TestDate", out TestDate);
            string _TestDate = Convert.ToString(TestDate);

            object ECandidateID;
            test.TryGetValue("ECandidateID", out ECandidateID);
            int _ECandidateID = Convert.ToInt32(ECandidateID);

            object JobID;
            test.TryGetValue("JobID", out JobID);
            int _JobID = Convert.ToInt32(JobID);

           

           


            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var candidate_id = db.Query("ECandidateApplication").InsertGetId<int>(new
                    {

                        QualifiedStatus = _QualifiedStatus,
                        ApplyDate = _ApplyDate,
                        TestDate = _TestDate,
                        ECandidateID = _ECandidateID,
                        JobID = _JobID,
                       
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

        [HttpDelete]
        public HttpResponseMessage DeleteEcandidateApplication(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            try
            {
                _ = db.Query("ECandidateApplication").Where("RefID", "=", id).Delete();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateEcandidateApplication(Object job)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(job));

            object __RefID;
            test.TryGetValue("RefID", out __RefID);
            int _RefID = Convert.ToInt32(__RefID);


            object __QualifiedStatus;
            test.TryGetValue("QualifiedStatus", out __QualifiedStatus);
            string _QualifiedStatus = Convert.ToString(__QualifiedStatus);

            object __ApplyDate;
            test.TryGetValue("ApplyDate", out __ApplyDate);
            string _ApplyDate = Convert.ToString(__ApplyDate);

            

            object __TestDate;
            test.TryGetValue("TestDate", out __TestDate);
            string _TestDate = Convert.ToString(__TestDate);

            object __ECandidateID;
            test.TryGetValue("ECandidateID", out __ECandidateID);
            int _ECandidateID = Convert.ToInt32(__ECandidateID);

            object __JobID;
            test.TryGetValue("JobID", out __JobID);
            int _JobID = Convert.ToInt32(__JobID);

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {

                    var affected = db.Query("ECandidateApplication").Where("RefID", _RefID).Update(new
                    {
                        QualifiedStatus = _JobID,
                        ApplyDate = _ApplyDate,
                        TestDate = _TestDate,
                        ECandidateID = _ECandidateID,
                        JobID = _JobID
                    });
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception e)
                {
                    scope.Dispose();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                }
            }
        }






    }
}
