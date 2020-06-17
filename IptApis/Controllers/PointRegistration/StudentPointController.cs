using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PointDataAccess;
namespace IptApis.Controllers.PointRegistration
{
    [Route("api/PointApp/StudentPoint")]
    public class StudentPointController : ApiController
    {
        public IEnumerable<StudentPoint> Get()
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                return entities.StudentPoints.ToList();
            }
        }
        public StudentPoint Get(int SPID)
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                return entities.StudentPoints.FirstOrDefault(sp => sp.StudentPointId == SPID);
            }
        }
        public HttpResponseMessage Post([FromBody] StudentPoint studentPoint)
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                try
                {
                    Point point = entities.Points.FirstOrDefault(p => p.PointID == studentPoint.PointID);
                    if (point != null)
                    {
                        if (point.NumberOfSeats > 0)
                        {
                            entities.StudentPoints.Add(studentPoint);
                            point.NumberOfSeats = point.NumberOfSeats - 1;
                            entities.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.Created, studentPoint);
                            message.Headers.Location = new Uri(Request.RequestUri +
                                studentPoint.StudentPointId.ToString());

                            return message;
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Seats Available");
                        }
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "PointID = " + studentPoint.PointID.ToString() + " Does not Exist ");

                    }


                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

                }

            }
        }
        public HttpResponseMessage Delete(int SPID)
        {
            try
            {
                using (PointDBEntities entities = new PointDBEntities())
                {
                    entities.Configuration.ProxyCreationEnabled = false;
                    var entity = entities.StudentPoints.FirstOrDefault(sp => sp.StudentPointId == SPID);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "StudentPoint with StudentPointId = " + SPID.ToString() + " not found to delete");
                    }
                    else
                    {
                        if (entity.PointID != null)
                        {
                            Point point = entities.Points.FirstOrDefault(p => p.PointID == entity.PointID);
                            point.NumberOfSeats = point.NumberOfSeats + 1;
                            entities.StudentPoints.Remove(entity);
                            entities.SaveChanges();

                        }
                        else
                        {
                            entities.StudentPoints.Remove(entity);
                            entities.SaveChanges();
                        }

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Put(int SPID, [FromBody]StudentPoint studentPoint)
        {
            try
            {
                using (PointDBEntities entities = new PointDBEntities())
                {
                    var entity = entities.StudentPoints.FirstOrDefault(sp => sp.StudentPointId == SPID);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "StudentPoint with StudentPointId = " + SPID.ToString() + " not found to update");
                    }
                    else
                    {
                        Point point = entities.Points.FirstOrDefault(p => p.PointID == studentPoint.PointID);
                        if (point != null)
                        {
                            if (point.NumberOfSeats > 0)
                            {
                                point.NumberOfSeats = (entity.PointID == point.PointID
                                    ? point.NumberOfSeats : point.NumberOfSeats - 1);
                                studentPoint.StudentPointId = entity.StudentPointId;
                                entity.PickUpAddress = studentPoint.PickUpAddress;
                                entity.PointID = studentPoint.PointID;
                                entity.SemesterID = studentPoint.SemesterID;
                                entity.StudentID = studentPoint.StudentID;
                                entities.SaveChanges();

                                return Request.CreateResponse(HttpStatusCode.OK, studentPoint);
                            }
                            else
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Seats Available");
                            }
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "PointID = " + studentPoint.PointID.ToString() + " Does not Exist ");
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
