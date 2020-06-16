using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PointDataAccess;
namespace IptApis.Controllers.PointRegistration
{
    [Route("api/PointApp/PointPayment")]
    public class PointPaymentController : ApiController
    {
        public IEnumerable<PointPayment> Get()
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                return entities.PointPayments.ToList();
            }
        }
        public PointPayment Get(int PaymentID)
        {
            using (PointDBEntities entities = new PointDBEntities())
            {
                return entities.PointPayments.FirstOrDefault(pay => pay.PaymentID == PaymentID);
            }
        }
        public HttpResponseMessage Post([FromBody] PointPayment pointPayment)
        {
            using (PointDBEntities entities = new PointDBEntities())
            {

                try
                {
                    PointFee pointFee = entities.PointFees.FirstOrDefault(pf => pf.FeeID == pointPayment.FeeID);
                    if (pointFee != null)
                    {
                        int NumberOfDaysLate = Convert.ToInt32((pointPayment.DepositDate - pointFee.DueDate).TotalDays);
                        int TotalPayable = pointFee.TransportFee + NumberOfDaysLate * pointFee.FineCharges;
                        PointPayment p = new PointPayment()
                        {
                            StudentID = pointPayment.StudentID,
                            DepositDate = pointPayment.DepositDate,
                            NumberofDaysLate = NumberOfDaysLate,
                            TotalPayable = TotalPayable,
                            FeeID = pointFee.FeeID
                        };
                        entities.PointPayments.Add(p);
                        entities.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.Created, p);
                        message.Headers.Location = new Uri(Request.RequestUri +
                            p.PaymentID.ToString());

                        return message;
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Point Fee FeeID = " + pointPayment.FeeID.ToString() + " does not exist ");
                    }

                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }
        }
        public HttpResponseMessage Delete(int PaymentID)
        {
            try
            {
                using (PointDBEntities entities = new PointDBEntities())
                {
                    var entity = entities.PointPayments.FirstOrDefault(pay => pay.PaymentID == PaymentID);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "PointPayment with PaymentID = " + PaymentID.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.PointPayments.Remove(entity);
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
        public HttpResponseMessage Put(int PaymentID, [FromBody]PointPayment pointPayment)
        {
            try
            {
                using (PointDBEntities entities = new PointDBEntities())
                {
                    var entity = entities.PointPayments.FirstOrDefault(pay => pay.PaymentID == PaymentID);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "PointPayment with PaymentID " + PaymentID.ToString() + " not found to update");
                    }
                    else
                    {
                        PointFee pointFee = entities.PointFees.FirstOrDefault(pf => pf.FeeID == pointPayment.FeeID);
                        if (pointFee == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "FeeID = " + pointPayment.FeeID.ToString() + " does not exist ");
                        }
                        else
                        {
                            int NumberOfDaysLate = Convert.ToInt32((pointPayment.DepositDate - pointFee.DueDate).TotalDays);
                            int TotalPayable = pointFee.TransportFee + NumberOfDaysLate * pointFee.FineCharges;

                            entity.FeeID = pointFee.FeeID;
                            entity.StudentID = pointPayment.StudentID;
                            entity.NumberofDaysLate = NumberOfDaysLate;
                            entity.DepositDate = pointPayment.DepositDate;
                            entity.TotalPayable = TotalPayable;

                            entities.SaveChanges();

                            return Request.CreateResponse(HttpStatusCode.OK, entity);

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
