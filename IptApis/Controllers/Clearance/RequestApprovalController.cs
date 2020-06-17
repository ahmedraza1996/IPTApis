using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Transactions;
using IptApis.Shared;
namespace IptApis.Controllers.Clearance
{
    public class RequestApprovalController : ApiController
    {
        //Method to add Request after approving by concerned person.

        [HttpPost]
        public HttpResponseMessage AddRequest(Object RequestApproval)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(RequestApproval));
            //below are optional,just to check if we get anynull value it will chage to their defaults
            object RequestID;
            test.TryGetValue("RequestID", out RequestID);   //hover onto trygetvalue, to see the method signature
            string _RequestID = Convert.ToString(RequestID);
            object RequestStatus;
            test.TryGetValue("RequestStatus", out RequestStatus);
            string _RequestStatus = Convert.ToString(RequestStatus);
            object Remarks;
            test.TryGetValue("Remarks", out Remarks);
            string _Remarks = Convert.ToString(Remarks);
            string date = DateTime.Now.ToString("MM/dd/yyyy");

            object EmpID;
            test.TryGetValue("EmpID", out EmpID);   //hover onto trygetvalue, to see the method signature
            string _EmpID = Convert.ToString(EmpID);

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            // using (var scope = db.Connection.BeginTransaction())
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {

                    var res = db.Query("RequestApproval").InsertGetId<int>(new
                    {
                        RequestID = _RequestID,
                        RequestStatus = _RequestStatus,
                        EmpID= _EmpID,
                        date=date,
                        Remarks=_Remarks
                    });   //specify each field in form of dictionary or object

                    #region 

                    //var query = db.Query("propertydetail").AsInsert(test);

                    //

                    ////Inject the Identity in the Compiled Query SQL object
                    //

                    ////Name Binding house the values that the insert query needs 
                    //
                    //var res = db.Query("fooditem").AsInsert(fooditemobj);
                    //SqlKata.SqlResult compiledQuery = compiler.Compile(res);
                    //var sql = compiledQuery.Sql + "; SELECT @@IDENTITY as ID;";
                    //var IdentityKey = db.Select<string>(sql, compiledQuery.NamedBindings).FirstOrDefault();
                    //var insertedIdString = db.Select("SELECT @@IDENTITY");
                    //var res = db.Query("fooditem").InsertGetId<int>(fooditemobj);
                    //var res = db.Query()
                    //scope.Commit();
                    #endregion
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, object>() { { "LastInsertedId", res } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();   //if there are any error, rollback the transaction
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }


        }
    }
}
