using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PointDataAccess;
namespace IptApis.Controllers.PointRegistration
{
    [Route("api/PointApp/Point")]
    public class PointController : ApiController
    {
        public IEnumerable<Point> Get()
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                return entities.Points.ToList();
            }
        }
        public Point Get(int PointID)
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                return entities.Points.FirstOrDefault(p => p.PointID == PointID);
            }
        }
        public HttpResponseMessage Post([FromBody] Point point)
        {
            using (PointDBEntities entities = new PointDBEntities())
            {

                try
                {
                    Driver driver = entities.Drivers.FirstOrDefault(d => d.CNIC == point.CNIC);
                    if (driver != null)
                    {
                        entities.Points.Add(point);
                        entities.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.Created, point);
                        message.Headers.Location = new Uri(Request.RequestUri +
                            point.PointID.ToString());

                        return message;
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Driver CNIC =  " + point.CNIC + " does not exist ");

                    }

                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }
        }
        public HttpResponseMessage Delete(int PointID)
        {
            try
            {
                using (PointDBEntities entities = new PointDBEntities())
                {
                    entities.Configuration.ProxyCreationEnabled = false;
                    var entity = entities.Points.FirstOrDefault(p => p.PointID == PointID);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Point with PointID = " + PointID.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Points.Remove(entity);
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
        public HttpResponseMessage Put(int PointID, [FromBody]Point point)
        {
            try
            {
                using (PointDBEntities entities = new PointDBEntities())
                {
                    var entity = entities.Points.FirstOrDefault(p => p.PointID == PointID);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Point with PointID " + PointID.ToString() + " not found to update");
                    }
                    else
                    {
                        Driver driver = entities.Drivers.FirstOrDefault(d => d.CNIC == point.CNIC);
                        if (driver != null)
                        {
                            entity.NumberPlate = point.NumberPlate;
                            entity.NumberOfSeats = point.NumberOfSeats;
                            entity.CNIC = point.CNIC;
                            entities.SaveChanges();

                            return Request.CreateResponse(HttpStatusCode.OK, entity);
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Driver CNIC = " + point.CNIC + " Does Not Exist");
                        }

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
