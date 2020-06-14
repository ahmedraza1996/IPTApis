using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IptApis.Shared;
using SqlKata.Execution;
using IptApis.Models;
using IptApis.Shared.Constants;
using System.Linq.Expressions;

namespace IptApis.Controllers.Course
{
    public class CourseController : ApiController
    {
        QueryFactory db = DbUtils.GetDBConnection();   //GetDBConnection will return a DBFactory type object, which have establish sql connection

        public CourseController()
        {
            db.Connection.Open();
        }
        // GET: api/Course

        [Route("api/Course/student/{studentID}")]
        [HttpGet]
        public IHttpActionResult getStudentCourses(int studentID)
        {
            
            IEnumerable<OfferedCourse> courses = db.Query("coursedetails").Where("StudentID",studentID).Get<OfferedCourse>();
            return Ok(courses);
        }

        // GET: api/Course/5
        public IHttpActionResult Get(int id)
        {
           OfferedCourse course = db.Query("coursedetails").Where("CourseOfferedID",id).First<OfferedCourse>();
           return Ok(course);
        }

        // POST: api/Course
        public IHttpActionResult Post([FromBody]CourseModel course)
        {
            try
            {
                IEnumerable<CourseModel> courses=db.Query("Course").Where(new
                {
                    courseCode = course.courseCode,
                    courseName = course.courseName
                }).Get<CourseModel>();

                if (courses.ToArray().Length>0) return BadRequest(CourseConstants.COURSE_EXISTS);

                int affected=db.Query("Course").Insert(course);
                return Ok(affected);
                   
            }
            catch (Exception err)
            {
                return InternalServerError();
            }
        }

        [Route("api/Course/offerCourse")]
        [HttpPost]
        public IHttpActionResult addOfferedCourse([FromBody] OfferedCourseBasic course)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(General.FIELDS_MISSING);
                CourseModel courseCheck=db.Query("Course").Where("courseID", course.courseID).First<CourseModel>();

                if (courseCheck == null) return BadRequest(CourseConstants.COURSE_NOT_EXISTS);
                

                int affected=db.Query("CourseOffered").Insert(course);
                return Ok(affected);
            }
            catch(Exception ex)
            {
                return InternalServerError();
            }

        }

        [Route("api/Course/offeredCourses/semester/{semesterID}")]
        [HttpGet]

        public IHttpActionResult getOfferedCoursesBySemester(int semesterID)
        {
            try
            {
                IEnumerable<dynamic> courses = db.Query("OfferedCourseDetails").Get();
                return Ok(courses);
            }
            catch(Exception ex)
            {
                return InternalServerError();
            }
        }

        // PUT: api/Course/5
        public void Put(int id, [FromBody]dynamic course)
        {
        }

        // DELETE: api/Course/5
        public void Delete(int id)
        {
        }
    }
}
