using IptApis.Models.AdminPanel;
using IptApis.Shared;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;
using System.Transactions;

namespace IptApis.Controllers.AdminPanel
{


    [System.Web.Http.RoutePrefix("api")]
    public class admincontroller : ApiController
    {
        private static string CONNSTRING = ConfigurationManager.AppSettings["SqlDBConn"];

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpGet]
        //api/admin/loginadmin/1
        public HttpResponseMessage LoginAdmin(int id)
        {
            var db = DbUtils.GetDBConnection();   //GetDBConnection will return a DBFactory type object, which have establish sql connection
            db.Connection.Open();

            IEnumerable<admin> response;
            response = db.Query("admin").Where("adminId", id).Get<admin>();

            return Request.CreateResponse(HttpStatusCode.OK,response);





            //if (model == null || !ModelState.IsValid) return Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid credentials");
            //Tuple<HttpStatusCode, string> error;
            //var log = dbo.admins.Where(a => a.adminEmail.Equals(adm.adminEmail) && a.adminPassword.Equals(adm.adminPassword)).FirstOrDefault();
            //var response = Request.CreateResponse(HttpStatusCode.OK, admin);
            //response.Headers.Add("Authorization", token);

        }


        [System.Web.Http.HttpPost]
        //[ValidateAntiForgeryToken]
        [System.Web.Http.Route("admin/adminLogin")]
        public HttpResponseMessage adminLogin([FromBody] dynamic adm)
        {
            //System.Diagnostics.Debug.WriteLine("Called admin login route");
            //var db = DbUtils.GetDBConnection();   //GetDBConnection will return a DBFactory type object, which have establish sql connection
            //db.Connection.Open();

            ////var log = db.Query("admin").Where(a => a.adminEmail.Equals(adm.adminEmail) && a.adminPassword.Equals(adm.adminPassword)).FirstOrDefault();
            // //Object log = (db.Query("admin").Where("adminEmail",adm.adminEmail).Where("adminPassword", adm.adminPassword));
            ////response = db.Query("TeacherCourseSectionDetail").Where("EmpName", empId).Where("CourseID", cid)/*.Get<Section>();*/

            //IEnumerable < admin > log;
            //log = db.Query("admin").Where("adminEmail", adm.adminEmail).Where("adminPassword", adm.adminPassword).POST<admin>();
            //System.Diagnostics.Debug.WriteLine(log);
            //return Request.CreateResponse(HttpStatusCode.OK, log);


            string query = $"SELECT * FROM ADMIN where adminEmail='{adm.adminEmail}' and adminPassword='{adm.adminPassword}'";
            using (SqlConnection sqlConnection = new SqlConnection(CONNSTRING))
            {
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                try
                {
                    String email = adm.adminEmail;
                    String password = adm.adminPassword;
                    sqlConnection.Open();
                    SqlDataReader adminReader = sqlCommand.ExecuteReader();
                    admin admn = null;
                    while (adminReader.Read())
                    {
                        admn = new admin(
                            adminReader.GetString(adminReader.GetOrdinal("adminName")),
                            adminReader.GetString(adminReader.GetOrdinal("adminEmail")),
                            adminReader.GetString(adminReader.GetOrdinal("adminPassword")),
                            adminReader.GetString(adminReader.GetOrdinal("adminPhone"))
                            );
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, admn);
                    
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e);

                }
            }


        }



















        // Faculty CRUD Operations work will be done here

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("admin/getFaculties")]
        public HttpResponseMessage GetFaculties()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("employee").Select("employee.EmpID", "employee.EmpName", "employee.Email", "employee.MobileNumber", "Designation.DesignationID", "Designation.DesignationTitle", "Department.DepartmentID", "Department.DepartmentName").Join("Designation", "employee.DesignationID", "Designation.DesignationID").Join("Department", "employee.DepartmentID", "Department.DepartmentID").Get().Cast<IDictionary<string, object>>();   // gets all food items from table and cast it to list of dictionary

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("admin/getFaculties/")]
        public HttpResponseMessage GetFaculties(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("employee").Where("employee.EmpID", id).Select("employee.EmpID", "employee.EmpName", "employee.Email", "employee.MobileNumber", "Designation.DesignationTitle", "Department.DepartmentName").Join("Designation", "employee.DesignationID", "Designation.DesignationID").Join("Department", "employee.DepartmentID", "Department.DepartmentID").Get().Cast<IDictionary<string, object>>();   // gets all food items from table and cast it to list of dictionary

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [HttpGet]
        public HttpResponseMessage GetDesignation()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<ADesignations> response;

            response = db.Query("Designation").Get<ADesignations>();
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }
        [HttpGet]
        public HttpResponseMessage GetDepartment()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<ADepartments> response;

            response = db.Query("Department").Get<ADepartments>();
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        [HttpPost]
        public HttpResponseMessage AddFaculty(DataFaculties data)
        {
            string EmpName = data.EmpName;
            string Email = data.Email;
            string MobileNumber = data.MobileNumber;
            string EPassword = data.EPassword;
            int DesignationID = data.DesignationID;
            int DepartmentID = data.DepartmentID;


            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            using (TransactionScope scope = new TransactionScope())
            {

                var query = db.Query("Employee").InsertGetId<int>(new
                {
                    EmpName = EmpName,
                    Email = Email,
                    MobileNumber = MobileNumber,
                    EPassword = EPassword,
                    DesignationID = DesignationID,
                    DepartmentID = DepartmentID,
                    RefID = 1
                });


                scope.Complete();  // if record is entered successfully , transaction will be committed


                db.Connection.Close();
                return Request.CreateResponse(HttpStatusCode.OK, query);
            }
        }



        [HttpPost]
        [System.Web.Http.Route("admin/DeleteFaculty")]
        public HttpResponseMessage DeleteFaculty(DataFaculties data)
        {
            int id = data.EmpID;

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    db.Query("Employee").Where("EmpID", id).Delete();
                    scope.Complete();
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }








        // Students CRUD Operations work will be done here


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("admin/getStudents")]
        public HttpResponseMessage GetStudents()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Student").Select("Student.StudentID", "Student.SName", "Student.Email", "Student.MobileNumber", "Student.RollNumber", "Student.SPassword", "Batch.BatchID", "Batch.BatchYear", "Programme.ProgrammeID", "Programme.ProgrammeName").Join("Batch", "Student.BatchID", "Batch.BatchID").Join("Programme", "Student.ProgrammeID", "Programme.ProgrammeID").Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [HttpGet]
        public HttpResponseMessage GetBatch()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<ABatch> response;

            response = db.Query("Batch").Get<ABatch>();
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        [HttpPost]
        public HttpResponseMessage AddStudent(DataStudents data)
        {
            string SName = data.SName;
            string Email = data.Email;
            string MobileNumber = data.MobileNumber;
            string RollNumber = data.RollNumber;
            string SPassword = data.SPassword;
            int BatchID = data.BatchID;
            int ProgrammeID = data.ProgrammeID;


            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            using (TransactionScope scope = new TransactionScope())
            {

                var query = db.Query("Student").InsertGetId<int>(new
                {
                    SName = SName,
                    Email = Email,
                    MobileNumber = MobileNumber,
                    RollNumber = RollNumber,
                    SPassword = SPassword,
                    BatchID = BatchID,
                    ProgrammeID = ProgrammeID,
                    CandidateID = 2
                });


                scope.Complete();  // if record is entered successfully , transaction will be committed


                db.Connection.Close();
                return Request.CreateResponse(HttpStatusCode.OK, query);
            }
        }



        [HttpPost]
        [System.Web.Http.Route("admin/DeleteStudent")]
        public HttpResponseMessage DeleteStudent(DataStudents data)
        {
            int id = data.StudentID;

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    db.Query("Student").Where("StudentID", id).Delete();
                    scope.Complete();
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }















        // Courses CRUD Operations work will be done here


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("admin/getCourses")]
        public HttpResponseMessage GetCourses()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Course").Select("Course.CourseID", "Course.CourseName", "Course.CourseCode").Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }




        [HttpPost]
        public HttpResponseMessage AddCourse(DataCourse data)
        {
            string CourseName = data.CourseName;
            string CourseCode = data.CourseCode;

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            using (TransactionScope scope = new TransactionScope())
            {

                var query = db.Query("Course").InsertGetId<int>(new
                {
                    CourseName = CourseName,
                    CourseCode = CourseCode
                });


                scope.Complete();  // if record is entered successfully , transaction will be committed


                db.Connection.Close();
                return Request.CreateResponse(HttpStatusCode.OK, query);
            }
        }



        [HttpPost]
        [System.Web.Http.Route("admin/DeleteCourse")]
        public HttpResponseMessage DeleteCourse(DataCourse data)
        {
            int id = data.CourseID;

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    db.Query("Course").Where("CourseID", id).Delete();
                    scope.Complete();
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }






















        /*
         internalError = null;
			string query = $"INSERT INTO RESTAURANTS(RName, RAddress, RPhoneNumber, REmail, RPassword) VALUES ('{model.Name}', '{model.Address}','{model.PhoneNumber}','{model.Email}','{model.Password}')";
			SqlConnection sqlConnection = new SqlConnection(CONNSTRING);
			try
			{
				sqlConnection.Open();
				SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.ExecuteNonQuery();
				sqlCommand.Dispose();
				sqlConnection.Close();

				return new Restaurant(model.Id, model.Name, model.Email, model.Address, model.PhoneNumber);
			}
			catch (Exception e)
			{
				internalError = e.Message;
				sqlConnection.Close();
				return null;
			}
             */














        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}