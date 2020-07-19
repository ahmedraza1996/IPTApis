using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using IptApis.Models.PointRegistration.PointDataAccess;
namespace IptApis.Controllers.PointRegistration
{
    [Route("api/PointApp/Student")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StudentController : ApiController
    {
        public IEnumerable<Student> Get()
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                return entities.Students.ToList();
            }
        }
        public Student Get(int StudentID)
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                return entities.Students.FirstOrDefault(s => s.StudentID == StudentID);
            }
        }
    }
}
