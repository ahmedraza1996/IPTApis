﻿using IptApis.Shared;
using Newtonsoft.Json;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace IptApis.Controllers.Search_Module.QuerySearch
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class SearchController : ApiController
    {
        public string Get()
        {
            return "Hello World";
        }

        [HttpPost]
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

            string input_queryapi = "";

            object userName;
            dictJson.TryGetValue("queried_by_username",out userName);
            

            if (Action.Equals("GetTableSchema"))
            {
                object tableName;
                dictJson.TryGetValue("TableName", out tableName);
                input_queryapi = tableName.ToString();
                response = db.Query("INFORMATION_SCHEMA.COLUMNS").Select("COLUMN_NAME").WhereRaw("TABLE_NAME  = ?",tableName).Get().Cast<IDictionary<string, object>>();
                return Request.CreateResponse(HttpStatusCode.OK, response);

            }
            if (Action.Equals("GetInstructorByName"))
            {
                object empName;
                dictJson.TryGetValue("EmpName", out empName);
                input_queryapi = empName.ToString();
                response = db.Query("Employee").WhereRaw("lower(EmpName) = ?", empName).Get().Cast<IDictionary<string, object>>();
                //response = db.Query("Employee").WhereRaw("lower(EmpName) = ?", "dr. abdul aziz").Get().Cast<IDictionary<string, object>>();

            }
            else if (Action.Equals("GetInstructorByEmail"))
            {
                object email;
                dictJson.TryGetValue("Email", out email);
                input_queryapi = email.ToString();
                response = db.Query("Employee").WhereRaw("lower(Email) = ?", email).Get().Cast<IDictionary<string, object>>();
            }
            else if (Action.Equals("GetInstructorByRank"))
            {
                object designationTitle;
                dictJson.TryGetValue("DesignationTitle", out designationTitle);
                input_queryapi = designationTitle.ToString();
                response = db.Query("Employee").Join("Designation", "Employee.DesignationID", "Designation.DesignationID").Where("DesignationTitle", designationTitle).Get().Cast<IDictionary<string, object>>();  //get product by id=1
            }
            else if (Action.Equals("GetInstructorByDepartment"))
            {
                object departmentID;
                dictJson.TryGetValue("DepartmentID", out departmentID);
                input_queryapi = departmentID.ToString();
                response = db.Query("Employee").Join("Department", "Employee.DepartmentID", "Department.DepartmentID").Where("Department.DepartmentID", departmentID).Get().Cast<IDictionary<string, object>>();  //get product by id=1

            }
            else if (Action.Equals("GetInstructorByNameStartsWith"))
            {
                object startNamePrefix;
                dictJson.TryGetValue("EmpName", out startNamePrefix);
                string prefixString = startNamePrefix.ToString() + "%";
                input_queryapi = startNamePrefix.ToString();
                response = db.Query("Employee").WhereLike("EmpName", prefixString).Get().Cast<IDictionary<string, object>>();  //get product by id=1

            }
            else if (Action.Equals("GetInstructorByNameContains"))
            {
                object startNamePrefix;
                dictJson.TryGetValue("EmpName", out startNamePrefix);
                string prefixString = "%" + startNamePrefix.ToString() + "%";
                input_queryapi = startNamePrefix.ToString();
                response = db.Query("Employee").WhereLike("EmpName", prefixString).Get().Cast<IDictionary<string, object>>();  //get product by id=1

            }
            else if (Action.Equals("GetInstructorByCourseName"))
            {
                object courseName;
                dictJson.TryGetValue("CourseName", out courseName);
                input_queryapi = courseName.ToString();
                response = db.Query("Employee").Join("CourseFaculty", "Employee.EmpID", "CourseFaculty.EmpID").Join("CourseOffered", "CourseOffered.CourseOfferedID", "CourseFaculty.CourseOfferedID").Join("Course", "CourseOffered.CourseID", "Course.CourseID").Where("CourseName", courseName).Get().Cast<IDictionary<string, object>>();  //get product by id=1

            }
            else if (Action.Equals("GetInstructorByCourseCode"))
            {
                object courseCode;
                dictJson.TryGetValue("CourseCode", out courseCode);
                input_queryapi = courseCode.ToString();
                response = db.Query("Employee").Join("CourseFaculty", "Employee.EmpID", "CourseFaculty.EmpID").Join("CourseOffered", "CourseOffered.CourseOfferedID", "CourseFaculty.CourseOfferedID").Join("Course", "CourseOffered.CourseID", "Course.CourseID").Where("CourseCode", courseCode).Get().Cast<IDictionary<string, object>>();  //get product by id=1

            }
            else if (Action.Equals("GetCoursesTaughtByASpecificInstructor"))
            {
                object EmpName;
                dictJson.TryGetValue("EmpName", out EmpName);
                input_queryapi = EmpName.ToString();
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
                input_queryapi = empName.ToString() + " " + semesterName.ToString();
                response = db.Query("Semester").Join("CourseOffered", "Semester.SemesterID", "CourseOffered.SemesterID").Join("Course", "Course.CourseID", "CourseOffered.CourseID")
                 .Join("CourseFaculty", "CourseFaculty.CourseOfferedID", "CourseOffered.CourseOfferedID")
                .Join("Employee", "Employee.EmpID", "CourseFaculty.EmpID")
                .Where("SemesterName", semesterName)
                .Where("EmpName", empName).Get().Cast<IDictionary<string, object>>();  //get product by id=1
            }
            // STUDENT QUERIES
            else if (Action.Equals("GetStudentByName"))
            {
                object SName;
                dictJson.TryGetValue("SName", out SName);
                input_queryapi = SName.ToString();
                response = db.Query("Student").WhereRaw("lower(SName) = ?", SName).Get().Cast<IDictionary<string, object>>();
            }
            else if (Action.Equals("GetStudentByStudentID"))
            {
                object RollNumber;
                dictJson.TryGetValue("RollNumber", out RollNumber);
                input_queryapi = RollNumber.ToString();
                response = db.Query("Student").WhereRaw("lower(RollNumber) = ?", RollNumber).Get().Cast<IDictionary<string, object>>();
            }
            else if (Action.Equals("GetStudentByBatchID"))
            {
                object BatchID;
                dictJson.TryGetValue("BatchID", out BatchID);
                input_queryapi = BatchID.ToString();
                response = db.Query("Student").WhereRaw("BatchID = ?", BatchID).Get().Cast<IDictionary<string, object>>();
            }
            else if (Action.Equals("GetStudentsByDepartment"))
            {
                object DepartmentName;
                dictJson.TryGetValue("DepartmentName", out DepartmentName);
                input_queryapi = DepartmentName.ToString();
                response = db.Query("Student").Join("Programme", "Programme.ProgrammeID", "Student.ProgrammeID").Join("Department", "Department.DepartmentID", "Programme.DepartmentID")
                .Where("DepartmentName", DepartmentName).Get().Cast<IDictionary<string, object>>();
            }
            else if (Action.Equals("GetStudentsBySection"))
            {
                object SectionName;
                dictJson.TryGetValue("SectionName", out SectionName);
                input_queryapi = SectionName.ToString();
                response = db.Query("Student").Join("Section", "Section.BatchID", "Student.BatchID")
                .Where("SectionName", SectionName).Get().Cast<IDictionary<string, object>>();
            }
            else if (Action.Equals("GetStudentByNUEmail"))
            {
                object Email;
                dictJson.TryGetValue("Email", out Email);
                input_queryapi = Email.ToString();
                response = db.Query("Student").Join("CandidateStudent", "CandidateStudent.CandidateID", "Student.CandidateID")
                .Where("Student.Email", Email).Get().Cast<IDictionary<string, object>>();
            }
            else if (Action.Equals("GetStudentByPrimaryEmail"))
            {
                object Email;
                dictJson.TryGetValue("Email", out Email);
                input_queryapi = Email.ToString();
                response = db.Query("Student").Join("CandidateStudent", "CandidateStudent.CandidateID", "Student.CandidateID")
                .Where("CandidateStudent.Email", Email).Get().Cast<IDictionary<string, object>>();
            }
            else if (Action.Equals("GetStudentsByNameStartsWith"))
            {
                object startNamePrefix;
                dictJson.TryGetValue("Sname", out startNamePrefix);
                input_queryapi = startNamePrefix.ToString();
                string prefixString = startNamePrefix.ToString() + "%";
                response = db.Query("Student").WhereLike("Sname", prefixString).Get().Cast<IDictionary<string, object>>();  //get product by id=1

            }
            else if (Action.Equals("GetStudentsByNameContains"))
            {
                object startNamePrefix;
                dictJson.TryGetValue("EmpName", out startNamePrefix);
                input_queryapi = startNamePrefix.ToString();
                string prefixString = "%" + startNamePrefix.ToString() + "%";
                response = db.Query("Student").WhereLike("Sname", prefixString).Get().Cast<IDictionary<string, object>>();  //get product by id=1

            }
            else if (Action.Equals("GetStudentsByCourseName"))
            {
                object CourseName;
                dictJson.TryGetValue("CourseName", out CourseName);
                input_queryapi = CourseName.ToString();
                response = db.Query("Student").Join("CourseEnrollment", "CourseEnrollment.StudentID", "Student.StudentID").Join("Course", "Course.CourseID", "CourseEnrollment.CourseID")
                .Where("CourseName", CourseName).Get().Cast<IDictionary<string, object>>();
            }
            else if (Action.Equals("GetStudentsByCourseCode"))
            {
                object CourseCode;
                dictJson.TryGetValue("CourseCode", out CourseCode);
                input_queryapi = CourseCode.ToString();

                response = db.Query("Student").Join("CourseEnrollment", "CourseEnrollment.StudentID", "Student.StudentID").Join("Course", "Course.CourseID", "CourseEnrollment.CourseID")
                .Where("CourseCode", CourseCode).Get().Cast<IDictionary<string, object>>();
            }
            else if (Action.Equals("GetStudentsBySkill"))
            {
                
                response = db.Query("StudentSkills").Join("Skill", "Skill.SkillID", "StudentSkills.SkillID").Join("Student", "StudentSkills.StudentID", "Student.StudentID")
                .Get().Cast<IDictionary<string, object>>();
            }
            else if (Action.Equals("GetStudentsByDomain"))
            {
                
                response = db.Query("Domain").Join("Skill", "Skill.DomainID", "Domain.DomainID").Join("StudentSkills", "StudentSkills.SkillID", "Skill.SkillID").Join("Student", "Student.StudentID", "StudentSkills.StudentID")
                .Get().Cast<IDictionary<string, object>>();
            }



            //var db1 = DbUtils.GetDBConnection();
            //db1.Connection.Open();

            SqlConnection dbConnection = new SqlConnection (ConfigurationManager.AppSettings["SqlDBConn"].ToString());
            


            try
            {
                /**
                db1.Query("dbo.SearchLog").AsInsert(new
                {
                    input_query = input_queryapi,
                    actionName = Action,
                    queried_by_username = userName.ToString()
                });
                **/

                string query = "INSERT INTO dbo.SearchLog(input_query, actionName, queried_by_username) VALUES(@input_query,@actionName,@queried_by_username)";
                using (SqlCommand command = new SqlCommand(query, dbConnection))
                {
                    command.Parameters.AddWithValue("@input_query", input_queryapi);
                    command.Parameters.AddWithValue("@actionName", Action);
                    command.Parameters.AddWithValue("@queried_by_username", userName.ToString());

                    dbConnection.Open();
                    int result = command.ExecuteNonQuery();

                    // Check Error
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database!");
                }

            }
            catch(Exception e)
            {
               

            }




            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        
    }
}