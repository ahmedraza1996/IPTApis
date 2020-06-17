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
    public class ClearanceRequestController : ApiController
    {
        //Method to add "Approved" to Status column of clearance request table on approval from Director.
        [HttpPost]
        public HttpResponseMessage AddFinalStatus(Object ClearanceRequest)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(ClearanceRequest));
            object status;
            test.TryGetValue("Status", out status);
            string _status = Convert.ToString(status);

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            // using (var scope = db.Connection.BeginTransaction())
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {

                    var res = db.Query("ClearanceRequest").InsertGetId<int>(new
                    {
                        Status = _status
                    }) ;   //specify each field in form of dictionary or object

                    #region 

                    #endregion
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, object>() { { "LastInsertedId", res
    }
});
                }
                catch (Exception ex)
                {
                    scope.Dispose();   //if there are any error, rollback the transaction
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        //Method to add clearance request of student
        [HttpPost]
        public HttpResponseMessage AddRequest(Object ClearanceRequest)
        {
            

            // string username = Thread.CurrentPrincipal.Identity.Name;

            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(ClearanceRequest));
            //below are optional,just to check if we get anynull value it will chage to their defaults

            object ParentName1;
            test.TryGetValue("ParentName1", out ParentName1);
            string _ParentName1 = Convert.ToString(ParentName1);
            object Parent1CNIC;
            test.TryGetValue("Parent1CNIC", out Parent1CNIC);
            string _Parent1CNIC = Convert.ToString(Parent1CNIC);
            object ParentName2;
            test.TryGetValue("ParentName2", out ParentName2);
            string _ParentName2 = Convert.ToString(ParentName2);
            object Parent2CNIC;
            test.TryGetValue("Parent2CNIC", out Parent2CNIC);
            string _Parent2CNIC = Convert.ToString(Parent2CNIC);

            object SemesterGraduation;
            test.TryGetValue("SemesterGraduation", out SemesterGraduation);
            string _SemesterGraduation = Convert.ToString(SemesterGraduation);

            string date = DateTime.Now.ToString("MM/dd/yyyy");

            object StudentID;
            test.TryGetValue("StudentID", out StudentID);   //hover onto trygetvalue, to see the method signature
            string _StudentID = Convert.ToString(StudentID);

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            // using (var scope = db.Connection.BeginTransaction())
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {

                    var res = db.Query("ClearanceRequest").InsertGetId<int>(new
                    {
                        ParentName1 = _ParentName1,
                        Parent1CNIC = _Parent1CNIC,
                        Parent2Name2 = _ParentName2,
                        Parent2CNIC = _Parent2CNIC,
                        SemesterGraduation = _SemesterGraduation,
                        RequestDate=date,
                        StudentID = _StudentID,
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
                    return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, object>() { { "LastInsertedId", res
    }
});
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
