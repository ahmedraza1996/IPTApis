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

        public HttpResponseMessage GetOpeningsById(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<JobOpening> response = db.Query("JobOpening").Where("JobID", id).Get<JobOpening>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        public HttpResponseMessage AddJobOpening(Object job)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(job));

            object MinExperience;
            test.TryGetValue("MinExperience", out MinExperience);
            int _MinExperience = Convert.ToInt32(MinExperience);

            object JobDescription;
            test.TryGetValue("JobDescription", out JobDescription);
            string _JobDescription = Convert.ToString(JobDescription);

            object Date;
            test.TryGetValue("DatePosted", out Date);
            string _DatePosted = Convert.ToString(Date);

            object Date1;
            test.TryGetValue("ExpectedStartDate", out Date1);
            string _ExpectedStartDate = Convert.ToString(Date1);

            object DesignationID;
            test.TryGetValue("DesignationID", out DesignationID);
            int _DesignationID = Convert.ToInt32(DesignationID);

            object DepartmentID;
            test.TryGetValue("DepartmentID", out DepartmentID);
            int _DepartmentID = Convert.ToInt32(DepartmentID);

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var id = db.Query("JobOpening").InsertGetId<int>(new
                    {
                        MinExperience = _MinExperience,
                        JobDescription = _JobDescription,
                        DatePosted = _DatePosted,
                        ExpectedStartDate = _ExpectedStartDate,
                        DesignationID = _DesignationID,
                        DepartmentID = _DepartmentID
                    });
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, id);//, new Dictionary<string, object>() { { "LastInsertedId", res } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();   //if there are any error, rollback the transaction
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }


        [HttpDelete]
        [HttpGet]
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

        [HttpPost]
        public HttpResponseMessage UpdateJobOpening(Object job)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(job));

            object __JobID;
            test.TryGetValue("JobID", out __JobID);
            int _JobID = Convert.ToInt32(__JobID);


            object __MinExperience;
            test.TryGetValue("MinExperience", out __MinExperience);
            int _MinExperience = Convert.ToInt32(__MinExperience);

            object __JobDescription;
            test.TryGetValue("JobDescription", out __JobDescription);
            string _JobDescription = Convert.ToString(__JobDescription);

            object Date1;
            test.TryGetValue("ExpectedStartDate", out Date1);
            string _ExpectedStartDate = Convert.ToString(Date1);

            object __DesignationID;
            test.TryGetValue("DesignationID", out __DesignationID);
            int _DesignationID = Convert.ToInt32(__DesignationID);

            object __DepartmentID;
            test.TryGetValue("DepartmentID", out __DepartmentID);
            int _DepartmentID = Convert.ToInt32(__DepartmentID);

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {

                    var affected = db.Query("JobOpening").Where("JobID", _JobID).Update(new
                    {
                        MinExperience = _MinExperience,
                        JobDescription = _JobDescription,
                        ExpectedStartDate = _ExpectedStartDate,
                        DesignationID = _DesignationID,
                        DepartmentID = _DepartmentID
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

        public HttpResponseMessage GetAllDesignations()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<Designation> response = db.Query("Designation").Get<Designation>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetAllDepartments()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<Department> response = db.Query("Department").Get<Department>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        public HttpResponseMessage GetDesignationById(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<Designation> response = db.Query("Designation").Where("DesignationID", id).Get<Designation>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetDepartmentById(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<Department> response = db.Query("Department").Where("DepartmentID", id).Get<Department>();
            return Request.CreateResponse(HttpStatusCode.OK, response); ;
        }
    }
}
