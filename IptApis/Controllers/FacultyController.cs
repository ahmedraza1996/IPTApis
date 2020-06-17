using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IptApis.Models;
using IptApis.Shared;
using SqlKata.Execution;

namespace IptApis.Controllers
{
    public class FacultyController : ApiController
    {

        QueryFactory db = DbUtils.GetDBConnection();   //GetDBConnection will return a DBFactory type object, which have establish sql connection

        public FacultyController()
        {
            db.Connection.Open();
        }
        

        [Route("api/Faculty/{departmentID}")]
        [HttpGet]
        public IHttpActionResult getFacultyByDepartment(int departmentID)
        {
            try
            {
                IEnumerable<FacultyBasic> faculty = db.Query("Employee").Where("departmentID", departmentID).Get<FacultyBasic>();
                return Ok(faculty);
            }
            catch(Exception ex)
            {
                return InternalServerError();
            }
        }


        [Route("api/Faculty/facultySections")]
        [HttpGet]

        public IHttpActionResult getFacultySectionDetails()
        {
            try
            {
                IEnumerable<FacultySectionDetails> facultyDetails = db.Query("TeacherCourseSectionDetail").Get<FacultySectionDetails>();
                return Ok(facultyDetails);
            }
            catch(Exception ex)
            {
                return InternalServerError();
            }
        }

        [Route("api/Faculty/assignSection")]
        [HttpPost]

        public IHttpActionResult assignSection([FromBody] FacultyCourseAssign courseAssign)
        {
            try
            {
                int cfID=db.Query("CourseFaculty").InsertGetId<int>(new
                {
                    numberOfSection = courseAssign.sectionID.Length,
                    courseOfferedID = courseAssign.courseOfferedID,
                    empID = courseAssign.empID
                });

                for(int i = 0; i < courseAssign.sectionID.Length; i++)
                {
                    db.Query("FacultySections").Insert(new
                    {
                        cfID = cfID,
                        sectionID = courseAssign.sectionID[i]
                    });
                }

                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError();
            }
        }

        // GET: api/Faculty/5
        public string Get(int id)
        {
            return "value";
        }

        
        // PUT: api/Faculty/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Faculty/5
        public void Delete(int id)
        {
        }
    }
}
