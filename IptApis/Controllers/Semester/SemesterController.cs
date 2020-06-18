using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.AccessControl;
using System.Web.Http;
using IptApis.Models;
using IptApis.Shared;
using IptApis.Shared.Constants;
using SqlKata;
using SqlKata.Execution;

namespace IptApis.Controllers.Semester2
{
    public class SemesterController : ApiController
    {
        QueryFactory db = DbUtils.GetDBConnection();   //GetDBConnection will return a DBFactory type object, which have establish sql connection
        public SemesterController()
        {
            db.Connection.Open();
        }
        // GET: api/Semester
        public IHttpActionResult Get()
        {
            try
            {
                IEnumerable<SemesterInfo> semesters = db.Query("SemesterDetails").Get<SemesterInfo>();
                return Ok(semesters);
            }catch(Exception err)
            {
                return InternalServerError();
            }
        }

        // GET: api/Semester/5
        public IHttpActionResult Get(int id)
        {
            try
            {
                SemesterInfo semester = db.Query("SemesterDetails").Where("semesterID", id).First<SemesterInfo>();
                return Ok(semester);

            }catch(Exception err)
            {
                return InternalServerError();
            }
        }

        // POST: api/Semester
        public IHttpActionResult Post([FromBody] SemesterInfo semester)
        {
            if (semester is null) return BadRequest(General.INVALID_INPUT);
            if (!HelperFunctions.checkDateFormat(semester.registrationStartDate.ToString())) return BadRequest(SemesterConstants.INVALID_REG_START_DATE);
            if (!HelperFunctions.checkDateFormat(semester.registrationEndDate.ToString())) return BadRequest(SemesterConstants.INVALID_REG_END_DATE);
            if (!HelperFunctions.checkDateFormat(semester.semesterStartDate.ToString())) return BadRequest(SemesterConstants.INVALID_REG_START_DATE);
            if (!HelperFunctions.checkDateFormat(semester.semesterEndDate.ToString())) return BadRequest(SemesterConstants.INVALID_SEM_END_DATE);

            if (semester.creditLimit <= 0) return BadRequest(SemesterConstants.INVALID_CREDIT_LIMIT);

            if (!HelperFunctions.dateVerfication(semester.registrationStartDate, semester.registrationEndDate)) return BadRequest(SemesterConstants.REG_START_DATE_GT_END_DATE);
            if (!HelperFunctions.dateVerfication(semester.semesterStartDate, semester.semesterEndDate)) return BadRequest(SemesterConstants.SEM_START_DATE_GT_END_DATE);
           
            try
            {

                SemesterBasic semesterBasic = new SemesterBasic(semester);
                int semesterID = db.Query("Semester").InsertGetId<int>(semesterBasic);

                semester.semesterID = semesterID;
                SemesterConfig semesterConfig = new SemesterConfig(semester);
                var affected = db.Query("Config").Insert(semesterConfig);
                return Ok();
            }
            catch (Exception err)
            {
                return InternalServerError(err) ;
            }



        }

        // PUT: api/Semester/5
        public void Put(int id, [FromBody]string value)
        {
            
        }

        // DELETE: api/Semester/5
        public void Delete(int id)
        {
        }


        [Route("api/Semester/registrationStatus/{semesterID}")]
        [HttpPatch]
        public IHttpActionResult changeRegistrationStatus(int semesterID, [FromBody] Boolean registrationStatus)
        {
            int status = Convert.ToInt32(registrationStatus);
            if (status > 1 || status < 0) return BadRequest(SemesterConstants.INVALID_REGISTRATION_STATUS);
            try
            {
                db.Query("Config").Where("semesterID", semesterID).Update(new
                {
                    RegistrationStatus = status
                });
                return Ok();
            }catch(Exception err)
            {
                return InternalServerError();
            }
        }


        [Route("api/Semester/current/{currentDate}")]
        [HttpGet]

        public IHttpActionResult getCurrentSemester(int currentDate)
        {
            if (!HelperFunctions.checkDateFormat(currentDate.ToString())) return BadRequest(General.INVALID_DATE_FORMAT);

            try
            {
                SemesterInfo semester = db.Query("SemesterDetails").Where("SemesterStartDate", "<=",currentDate)
                    .Where("SemesterEndDate", ">=",currentDate).First<SemesterInfo>();
                if (semester is null) return NotFound();
                return Ok(semester);
            }catch(Exception err)
            {
                return InternalServerError(err);
            }

        }


    }
}
