using IptApis.Shared;
using Newtonsoft.Json;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace IptApis.Controllers.Search_Module.QuerySearch
{
    public class SearchController : ApiController
    {
        public string Get()
        {
            return "Hello World";
        }

        public HttpResponseMessage GetSearchResult(object data)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response = null;

            var jsonData = JsonConvert.SerializeObject(data);

            var dictJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);
            object actionObject;
            dictJson.TryGetValue("actionName", out actionObject);
            string Action = actionObject.ToString();

            if (Action.Equals("GetInstructorByName"))
            {
                object empName;
                dictJson.TryGetValue("EmpName", out empName);
                response = db.Query("Employee").WhereRaw("lower(EmpName) = ?", empName).Get().Cast<IDictionary<string, object>>();
                //response = db.Query("Employee").WhereRaw("lower(EmpName) = ?", "dr. abdul aziz").Get().Cast<IDictionary<string, object>>();

            }
            else if (Action.Equals("GetInstructorByEmail"))
            {
                object email;
                dictJson.TryGetValue("Email", out email);
                response = db.Query("Employee").WhereRaw("lower(Email) = ?", email).Get().Cast<IDictionary<string, object>>();
            }
            else if (Action.Equals("GetInstructorByRank"))
            {
                object designationTitle;
                dictJson.TryGetValue("DesignationTitle", out designationTitle);
                response = db.Query("Employee").Join("Designation", "Employee.DesignationID", "Designation.DesignationID").Where("DesignationTitle", designationTitle).Get().Cast<IDictionary<string, object>>();  //get product by id=1
            }
            else if (Action.Equals("GetInstructorByDepartment"))
            {
                object departmentID;
                dictJson.TryGetValue("DepartmentID", out departmentID);
                response = db.Query("Employee").Join("Department", "Employee.DepartmentID", "Department.DepartmentID").Where("Department.DepartmentID", departmentID).Get().Cast<IDictionary<string, object>>();  //get product by id=1

            }
            else if (Action.Equals("GetInstructorByNameStartsWith"))
            {
                object startNamePrefix;
                dictJson.TryGetValue("EmpName", out startNamePrefix);
                string prefixString = startNamePrefix.ToString() + "%";
                response = db.Query("Employee").WhereLike("EmpName", prefixString).Get().Cast<IDictionary<string, object>>();  //get product by id=1

            }
            else if (Action.Equals("GetInstructorByNameContains"))
            {
                object startNamePrefix;
                dictJson.TryGetValue("EmpName", out startNamePrefix);
                string prefixString = "%" + startNamePrefix.ToString() + "%";
                response = db.Query("Employee").WhereLike("EmpName", prefixString).Get().Cast<IDictionary<string, object>>();  //get product by id=1

            }
            else if (Action.Equals("GetInstructorByCourseName"))
            {
                object courseName;
                dictJson.TryGetValue("CourseName", out courseName);
                response = db.Query("Employee").Join("CourseFaculty", "Employee.EmpID", "CourseFaculty.EmpID").Join("CourseOffered", "CourseOffered.CourseOfferedID", "CourseFaculty.CourseOfferedID").Join("Course", "CourseOffered.CourseID", "Course.CourseID").Where("CourseName", courseName).Get().Cast<IDictionary<string, object>>();  //get product by id=1

            }
            else if (Action.Equals("GetInstructorByCourseCode"))
            {
                object courseCode;
                dictJson.TryGetValue("CourseCode", out courseCode);
                response = db.Query("Employee").Join("CourseFaculty", "Employee.EmpID", "CourseFaculty.EmpID").Join("CourseOffered", "CourseOffered.CourseOfferedID", "CourseFaculty.CourseOfferedID").Join("Course", "CourseOffered.CourseID", "Course.CourseID").Where("CourseCode", courseCode).Get().Cast<IDictionary<string, object>>();  //get product by id=1

            }
            else if (Action.Equals("GetCoursesTaughtByASpecificInstructor"))
            {
                object EmpName;
                dictJson.TryGetValue("EmpName", out EmpName);
                response = db.Query("Semester").Select("CourseName").Join("CourseOffered", "Semester.SemesterID", "CourseOffered.SemesterID").Join("Course", "Course.CourseID", "CourseOffered.CourseID")
                .Join("CourseFaculty", "CourseFaculty.CourseOfferedID", "CourseOffered.CourseOfferedID")
                .Join("Employee", "Employee.EmpID", "CourseFaculty.EmpID")
                .Where("EmpName", EmpName).Get().Cast<IDictionary<string, object>>();  //get product by id=1

            }
            else if (Action.Equals("GetCoursesTaughtByAInstructorInAParticularSemester"))
            {
                
                object semesterName;
                object empName;
                dictJson.TryGetValue("EmpName", out empName);
                dictJson.TryGetValue("SemesterName", out semesterName);
                response = db.Query("Semester").Join("CourseOffered", "Semester.SemesterID", "CourseOffered.SemesterID").Join("Course", "Course.CourseID", "CourseOffered.CourseID")
                 .Join("CourseFaculty", "CourseFaculty.CourseOfferedID", "CourseOffered.CourseOfferedID")
                .Join("Employee", "Employee.EmpID", "CourseFaculty.EmpID")
                .Where("SemesterName", semesterName)
                .Where("EmpName", empName).Get().Cast<IDictionary<string, object>>();  //get product by id=1
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        public HttpResponseMessage GetInstructorByName()
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Employee").WhereRaw("lower(EmpName) = ?", "dr. abdul aziz").Get().Cast<IDictionary<string, object>>();  //get product by id=1
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetInstructorByEmail()
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Employee").WhereRaw("lower(Email) = ?", "xyz").Get().Cast<IDictionary<string, object>>();  //get product by id=1
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetInstructorByRank()
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Employee").Join("Designation", "Employee.DesignationID", "Designation.DesignationID").Where("DesignationTitle", "Lab Instructor").Get().Cast<IDictionary<string, object>>();  //get product by id=1
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetInstructorByDepartment()
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Employee").Join("Department", "Employee.DepartmentID", "Department.DepartmentID").Where("Department.DepartmentID", "1").Get().Cast<IDictionary<string, object>>();  //get product by id=1
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetInstructorByNameStartsWith()
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Employee").WhereLike("EmpName", "d%").Get().Cast<IDictionary<string, object>>();  //get product by id=1

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetInstructorByNameContains()
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Employee").WhereLike("EmpName", "%khan%").Get().Cast<IDictionary<string, object>>();  //get product by id=1

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetInstructorByCourseName()
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Employee").Join("CourseFaculty", "Employee.EmpID", "CourseFaculty.EmpID").Join("CourseOffered", "CourseOffered.CourseOfferedID", "CourseFaculty.CourseOfferedID").Join("Course", "CourseOffered.CourseID", "Course.CourseID").Where("CourseName", "Object Oriented Programming").Get().Cast<IDictionary<string, object>>();  //get product by id=1

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetInstructorByCourseCode()
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Employee").Join("CourseFaculty", "Employee.EmpID", "CourseFaculty.EmpID").Join("CourseOffered", "CourseOffered.CourseOfferedID", "CourseFaculty.CourseOfferedID").Join("Course", "CourseOffered.CourseID", "Course.CourseID").Where("CourseCode", "CS217").Get().Cast<IDictionary<string, object>>();  //get product by id=1

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetCoursesTaughtByAInstructorInAParticularSemester()
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Semester").Join("CourseOffered", "Semester.SemesterID", "CourseOffered.SemesterID").Join("Course", "Course.CourseID", "CourseOffered.CourseID")
                 .Join("CourseFaculty", "CourseFaculty.CourseOfferedID", "CourseOffered.CourseOfferedID")
                .Join("Employee", "Employee.EmpID", "CourseFaculty.EmpID")
                .Where("SemesterName", "Spring2020")
                .Where("EmpName", "Dr. Abdul Aziz").Get().Cast<IDictionary<string, object>>();  //get product by id=1

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetCoursesTaughtByASpecificInstructor()
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Semester").Select("CourseName").Join("CourseOffered", "Semester.SemesterID", "CourseOffered.SemesterID").Join("Course", "Course.CourseID", "CourseOffered.CourseID")
                 .Join("CourseFaculty", "CourseFaculty.CourseOfferedID", "CourseOffered.CourseOfferedID")
                 .Join("Employee", "Employee.EmpID", "CourseFaculty.EmpID")
                 .Where("EmpName", "Dr. Abdul Aziz").Get().Cast<IDictionary<string, object>>();  //get product by id=1

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

















        [System.Web.Http.HttpGet]
        public IHttpActionResult getInstructorDetailByName()
        {
            String Name = "dr. abdul aziz";
            string queryString = "SELECT EmpID, EmpName, Email, MobileNumber  FROM dbo.Employee WHERE LOWER(EmpName) LIKE @Name";

            string connectionString  = ConfigurationManager.AppSettings["SqlDBConn"].ToString();
            String result = "";
            List<String> Data1 = new List<String>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@Name", Name.ToLower());
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Data1.Add(reader.GetString(reader.GetOrdinal("EmpName")) + "," + reader.GetString(reader.GetOrdinal("Email")) + "," + reader.GetString(reader.GetOrdinal("MobileNumber")) + ",");
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            //return new System.Web.Mvc.JsonResult { Data = jss.Serialize(Data1) };

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent(jss.Serialize(Data1));
            return ResponseMessage(response);
            //return Json(Data1);
        }
    }
}