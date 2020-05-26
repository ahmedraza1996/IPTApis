using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IptApis.Shared;
using SqlKata.Execution;
using IptApis.Models;
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
            
            IEnumerable<CourseModel> courses = db.Query("coursedetails").Where("StudentID",studentID).Get<CourseModel>();
            return Ok(courses);
        }

        // GET: api/Course/5
        public IHttpActionResult Get(int id)
        {
           CourseModel course = db.Query("coursedetails").Where("CourseOfferedID",id).First<CourseModel>();
           return Ok(course);
        }

        // POST: api/Course
        public void Post([FromBody]CourseModel course)
        {
        }

        // PUT: api/Course/5
        public void Put(int id, [FromBody]CourseModel course)
        {
        }

        // DELETE: api/Course/5
        public void Delete(int id)
        {
        }
    }
}
