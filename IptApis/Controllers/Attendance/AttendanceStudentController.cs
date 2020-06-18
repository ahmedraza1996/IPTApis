using IptApis.Models.Attendance;
using IptApis.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SqlKata.Execution;
using System.Configuration;
using System.Data.SqlClient;

namespace IptApis.Controllers.Attendance
{
    public class AttendanceStudentController : ApiController
    {

        //return student course given student id
        //api/AttendanceStudent/GetStudentCourse/10
        [HttpGet]
        public HttpResponseMessage GetStudentCourse(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<AllStudentCourses> response;
            //response = db.Query("CourseEnrollment").Where("StudentID", id).Get<CourseEnrollment>();
            response = db.Query("AllStudentCourses").Where("StudentID", id).Get<AllStudentCourses>();
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        //return student course attendance when student is login in and course id is passed
        //api/AttendanceStudent/GetStudentAttendance/27
        public HttpResponseMessage GetStudentAttendance(int id)//courseid
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            int student = 10; //student login id
            IEnumerable<StudentCoursesAttendance> response;
            //response = db.Query("CourseEnrollment").Where("StudentID", id).Get<CourseEnrollment>();
            response = db.Query("StudentCoursesAttendance").Where("StudentID", student)
                                                            .Where("CourseID", id).Get<StudentCoursesAttendance>();
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


    }
}