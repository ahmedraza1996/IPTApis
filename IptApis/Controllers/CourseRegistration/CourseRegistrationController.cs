using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using IptApis.Models;
using IptApis.Shared;
using SqlKata.Execution;
namespace IptApis.Controllers.CourseRegistration
{
    public class CourseRegistrationController : ApiController
    {

        QueryFactory db = DbUtils.GetDBConnection();   //GetDBConnection will return a DBFactory type object, which have establish sql connection

        public CourseRegistrationController()
        {
            db.Connection.Open();
        }
        // GET: api/CourseRegistration
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/CourseRegistration/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CourseRegistration
        public IHttpActionResult Post([FromBody]CourseRegistrationModel value)
        {
            IEnumerable<dynamic> sections = db.Query("CourseSectionDetails").Where(new
            {
                CourseOfferedID = value.courseOfferedID
            }).Get();

            dynamic section = sections.ToArray()[value.sectionID];

            int fsID = (int)section.FSID;

            OfferedCourse course = db.Query("coursedetails").Where("CourseOfferedID", value.courseOfferedID)
                .First<OfferedCourse>();


            IEnumerable<dynamic> records = db.Query("courseSectionsNum").Where("FSID", fsID).Get();
            if (records.ToArray().Length >= course.maxStdPerSection) return BadRequest("Sections are full!");

            value.fsID = fsID;
            value.courseStatus = "ENROLLED";

            int affected=db.Query("CourseEnrollment").Insert(new { 
            courseStatus=value.courseStatus,
            fsID=value.fsID,
            courseID=value.courseOfferedID,
            studentID=value.studentID
            });
            return Ok(affected);
        }

        // PUT: api/CourseRegistration/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CourseRegistration/5
        public void Delete(int id)
        {
        }

        [Route("api/CourseRegistration/drop")]
        [HttpPost]
        [ResponseType(typeof(IHttpActionResult))]
        public IHttpActionResult dropCourse([FromBody] int enrollmentID)
        {
            int affected = db.Query("CourseEnrollment").Where("enrollmentID", enrollmentID).Update(new
            {
                CourseStatus = "DROP"
            });

            return Ok(affected);
        }

        [Route("api/CourseRegistration/withdraw")]
        [HttpPost]
        [ResponseType(typeof(IHttpActionResult))]
        public IHttpActionResult withdrawCourse([FromBody] int enrollmentID)
        {
            int affected = db.Query("CourseEnrollment").Where("enrollmentID", enrollmentID).Update(new
            {
                CourseStatus = "WITHDRAW"
            });

            return Ok(affected);
        }

    }
}
