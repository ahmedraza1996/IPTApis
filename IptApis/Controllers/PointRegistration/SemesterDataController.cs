using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PointDataAccess;
namespace IptApis.Controllers.PointRegistration
{
    [Route("api/PointApp/Semester")]
    public class SemesterDataController : ApiController
    {
        public IEnumerable<Semester> Get()
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                return entities.Semesters.ToList();
            }
        }
        public Semester Get(int SemesterID)
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                return entities.Semesters.FirstOrDefault(s => s.SemesterID == SemesterID);
            }
        }
    }
}
