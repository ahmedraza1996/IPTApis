using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PointDataAccess;
namespace IptApis.Controllers.PointRegistration
{
    [Route("api/PointApp/Route")]
    public class RouteController : ApiController
    {
        public IEnumerable<Route> Get()
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                return entities.Routes.ToList();
            }
        }
        public Route Get(int RouteID)
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                return entities.Routes.FirstOrDefault(r => r.RouteID == RouteID);
            }
        }
        public HttpResponseMessage Post([FromBody] Route route)
        {
            try
            {
                using (PointDBEntities entities = new PointDBEntities())
                {
                    Point point = entities.Points.FirstOrDefault(p => p.PointID == route.PointID);
                    if (point != null)
                    {
                        entities.Routes.Add(route);
                        entities.SaveChanges();

                        var message = Request.CreateResponse(HttpStatusCode.Created, route);
                        message.Headers.Location = new Uri(Request.RequestUri +
                            route.RouteID.ToString());

                        return message;

                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Point PointID =  " + route.PointID.ToString() + " does not exist ");

                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Delete(int RouteID)
        {
            try
            {
                using (PointDBEntities entities = new PointDBEntities())
                {
                    entities.Configuration.ProxyCreationEnabled = false;
                    var entity = entities.Routes.FirstOrDefault(r => r.RouteID == RouteID);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Route with RouteID = " + RouteID.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Routes.Remove(entity);
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
        public HttpResponseMessage Put(int RouteID, [FromBody]Route route)
        {
            try
            {
                using (PointDBEntities entities = new PointDBEntities())
                {
                    var entity = entities.Routes.FirstOrDefault(r => r.RouteID == RouteID);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Route with RouteID " + RouteID.ToString() + " not found to update");
                    }
                    else
                    {
                        Point point = entities.Points.FirstOrDefault(p => p.PointID == route.PointID);
                        if (point != null)
                        {
                            entity.PointID = route.PointID;
                            entity.RouteStop = route.RouteStop;
                            entity.RouteTime = route.RouteTime;
                            entities.SaveChanges();

                            return Request.CreateResponse(HttpStatusCode.OK, entity);
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Point PointID =  " + route.PointID.ToString() + " does not exist ");

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
