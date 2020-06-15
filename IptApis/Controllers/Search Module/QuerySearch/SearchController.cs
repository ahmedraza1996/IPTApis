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
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
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
            if (Action.Equals("GetTableSchema"))
            {
                object tableName;
                dictJson.TryGetValue("TableName", out tableName);
                response = db.Query("INFORMATION_SCHEMA.COLUMNS").Select("COLUMN_NAME").Where("TableName "," = ", tableName).Get().Cast<IDictionary<string, object>>();
            }
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


        
    }
}