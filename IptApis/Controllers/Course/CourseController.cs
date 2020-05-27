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
        public void Post([FromBody]dynamic course)
        {
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
