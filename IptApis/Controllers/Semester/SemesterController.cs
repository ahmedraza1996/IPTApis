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

namespace IptApis.Controllers.Semester
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
            if (!HelperFunctions.checkDateFormat(semester.registrationStartDate)) return BadRequest(SemesterConstants.INVALID_REG_START_DATE);
            if (!HelperFunctions.checkDateFormat(semester.registrationEndDate)) return BadRequest(SemesterConstants.INVALID_REG_END_DATE);
            if (!HelperFunctions.checkDateFormat(semester.semesterStartDate)) return BadRequest(SemesterConstants.INVALID_REG_START_DATE);
            if (!HelperFunctions.checkDateFormat(semester.semesterEndDate)) return BadRequest(SemesterConstants.INVALID_SEM_END_DATE);

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
    }
}
