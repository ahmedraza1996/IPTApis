using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PointDataAccess;
namespace IptApis.Controllers.PointRegistration
{
    [Route("api/PointApp/Driver")]
    public class DriverController : ApiController
    {
        public IEnumerable<Driver> Get()
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                return entities.Drivers.ToList();
            }
        }
        public Driver Get(string CNIC)
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                return entities.Drivers.FirstOrDefault(d => d.CNIC == CNIC);
            }
        }
        public HttpResponseMessage Post([FromBody] Driver driver)
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                try
                {
                    entities.Drivers.Add(driver);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, driver);
                    message.Headers.Location = new Uri(Request.RequestUri +
                        driver.CNIC);

                    return message;
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }
        }
        public HttpResponseMessage Delete(string CNIC)
        {
            try
            {
                using (PointDBEntities entities = new PointDBEntities())
                {
                    var entity = entities.Drivers.FirstOrDefault(d => d.CNIC == CNIC);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Driver with CNIC = " + CNIC + " not found to delete");
                    }
                    else
                    {
                        entities.Drivers.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Put(string CNIC, [FromBody]Driver driver)
        {
            try
            {
                using (PointDBEntities entities = new PointDBEntities())
                {
                    var entity = entities.Drivers.FirstOrDefault(d => d.CNIC == CNIC);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Driver with CNIC " + CNIC + " not found to update");
                    }
                    else
                    {
                        if (entity.CNIC == driver.CNIC)
                        {
                            entity.DriverName = driver.DriverName;
                            entity.ContactNumber = driver.ContactNumber;
                        }
                        else
                        {
                            entities.Drivers.Remove(entity);
                            entities.Drivers.Add(driver);
                        }
                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, driver);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
