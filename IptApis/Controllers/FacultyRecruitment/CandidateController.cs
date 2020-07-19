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
using System.Web.Http.Cors;

namespace IptApis.Controllers.FacultyRecruitment
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CandidateController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage AddCandidate(Object candidate)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(candidate));

            object EName;
            test.TryGetValue("EName", out EName);
            string _EName = Convert.ToString(EName);

            object Email;
            test.TryGetValue("Email", out Email);
            string _Email = Convert.ToString(Email);

            object Epassword;
            test.TryGetValue("EPassword", out Epassword);
            string _Epassword = Convert.ToString(Epassword);

            object EAddress;
            test.TryGetValue("EAddress", out EAddress);
            string _EAddress = Convert.ToString(EAddress);

            object MobileNumber;
            test.TryGetValue("MobileNumber", out MobileNumber);
            string _MobileNumber = Convert.ToString(MobileNumber);

            object EducationalLevel;
            test.TryGetValue("EducationalLevel", out EducationalLevel);
            string _EducationalLevel = Convert.ToString(EducationalLevel);

            object ExperienceYears;
            test.TryGetValue("ExperienceYears", out ExperienceYears);
            int _ExperienceYears = Convert.ToInt32(ExperienceYears);

            object CVPath;
            test.TryGetValue("CVpath", out CVPath);
            string _CVPath = Convert.ToString(CVPath);

            object CoverLetterPath;
            test.TryGetValue("Coverletterpath", out CoverLetterPath);
            string _CoverLetterPath = Convert.ToString(CoverLetterPath);


            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var candidate_id = db.Query("CandidateEmployee").InsertGetId<int>(new
                    {
                        EName = _EName,
                        Email = _Email,
                        EPassword = _Epassword,
                        EAddress = _EAddress,
                        MobileNumber = _MobileNumber,
                        EducationLevel = _EducationalLevel,
                        ExperienceYears = _ExperienceYears,
                        CVpath = _CVPath,
                        Coverletterpath = _CoverLetterPath
                    });

                    var Alluser_id = db.Query("AllUsers").InsertGetId<int>(new
                    {
                        UserName = _Email
                    });

                    var UserRole_id = db.Query("UserRole").InsertGetId<int>(new
                    {
                        UserId = Alluser_id,
                        Role = "candidate"
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
            IEnumerable<CandidateEmployee> response = db.Query("CandidateEmployee").Where("ECandidateID", id).Get<CandidateEmployee>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }



        [HttpPut]
        public HttpResponseMessage UpdateCandidate(object candidate)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(candidate));
            object ECandidateID;
            test.TryGetValue("ECandidateID", out ECandidateID);
            int _ECandidateID = Convert.ToInt32(ECandidateID);

            object EName;
            test.TryGetValue("EName", out EName);
            string _EName = Convert.ToString(EName);

            object Email;
            test.TryGetValue("Email", out Email);
            string _Email = Convert.ToString(Email);

            object Epassword;
            test.TryGetValue("EPassword", out Epassword);
            string _Epassword = Convert.ToString(Epassword);

            object EAddress;
            test.TryGetValue("EAddress", out EAddress);
            string _EAddress = Convert.ToString(EAddress);

            object MobileNumber;
            test.TryGetValue("MobileNumber", out MobileNumber);
            string _MobileNumber = Convert.ToString(MobileNumber);

            object EducationalLevel;
            test.TryGetValue("EducationalLevel", out EducationalLevel);
            string _EducationalLevel = Convert.ToString(EducationalLevel);

            object ExperienceYears;
            test.TryGetValue("ExperienceYears", out ExperienceYears);
            int _ExperienceYears = Convert.ToInt32(ExperienceYears);

            object CVPath;
            test.TryGetValue("CVpath", out CVPath);
            string _CVPath = Convert.ToString(CVPath);

            object CoverLetterPath;
            test.TryGetValue("Coverletterpath", out CoverLetterPath);
            string _CoverLetterPath = Convert.ToString(CoverLetterPath);

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            try
            {

                int affected = db.Query("CandidateEmployee").Where("ECandidateID", _ECandidateID).Update(new
                {
                    EName = _EName,
                    Email = _Email,
                    EPassword = _Epassword,
                    EAddress = _EAddress,
                    MobileNumber = _MobileNumber,
                    EducationLevel = _EducationalLevel,
                    ExperienceYears = _ExperienceYears,
                    CVpath = _CVPath,
                    Coverletterpath = _CoverLetterPath
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
