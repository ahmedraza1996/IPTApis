using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PointDataAccess;
namespace IptApis.Controllers.PointRegistration
{
    [Route("api/PointApp/PointFee")]
    public class PointFeeController : ApiController
    {
        public IEnumerable<PointFee> Get()
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                return entities.PointFees.ToList();
            }
        }
        public PointFee Get(int FeeID)
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                return entities.PointFees.FirstOrDefault(pf => pf.FeeID == FeeID);
            }
        }
        public HttpResponseMessage Post([FromBody] PointFee pointFee)
        {
            using (PointDBEntities entities = new PointDBEntities())
            {

                try
                {
                    Semester semester = entities.Semesters.FirstOrDefault(s => s.SemesterID == pointFee.SemesterID);
                    if (semester != null)
                    {
                        entities.PointFees.Add(pointFee);
                        entities.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.Created, pointFee);
                        message.Headers.Location = new Uri(Request.RequestUri +
                            pointFee.FeeID.ToString());

                        return message;
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Semester SemesterID = " + pointFee.SemesterID.ToString() + " does not exist ");
                    }

                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }
        }
        public HttpResponseMessage Delete(int FeeID)
        {
            try
            {
                using (PointDBEntities entities = new PointDBEntities())
                {
                    entities.Configuration.ProxyCreationEnabled = false;
                    var entity = entities.PointFees.FirstOrDefault(pf => pf.FeeID == FeeID);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "PointFee with FeeID = " + FeeID.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.PointFees.Remove(entity);
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
        public HttpResponseMessage Put(int FeeID, [FromBody]PointFee pointFee)
        {
            try
            {
                using (PointDBEntities entities = new PointDBEntities())
                {
                    var entity = entities.PointFees.FirstOrDefault(pf => pf.FeeID == FeeID);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "PointFee with FeeID " + FeeID.ToString() + " not found to update");
                    }
                    else
                    {
                        Semester semester = entities.Semesters.FirstOrDefault(s => s.SemesterID == pointFee.SemesterID);
                        if (semester != null)
                        {
                            pointFee.FeeID = entity.FeeID;
                            entity.DueDate = pointFee.DueDate;
                            entity.FineCharges = pointFee.FineCharges;
                            entity.TransportFee = pointFee.TransportFee;
                            entity.SemesterID = pointFee.SemesterID;

                            entities.SaveChanges();

                            return Request.CreateResponse(HttpStatusCode.OK, pointFee);
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Semester SemesterID = " + pointFee.SemesterID.ToString() + " does not exist ");

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
