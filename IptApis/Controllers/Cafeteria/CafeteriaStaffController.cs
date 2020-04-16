using Newtonsoft.Json;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Transactions;
using IptApis.Shared;

namespace IptApis.Controllers.Cafeteria
{
    public class CafeteriaStaffController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage AddProduct(Object Product)
        {


            // string username = Thread.CurrentPrincipal.Identity.Name;

            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(Product));
            object ItemName;
            test.TryGetValue("ItemName", out ItemName);
            string _ItemName = Convert.ToString(ItemName);
            object ItemStatus;
            test.TryGetValue("ItemStatus", out ItemStatus);
            string _ItemStatus = Convert.ToString(ItemStatus);
            object IDescription;
            test.TryGetValue("IDescription", out IDescription);
            string _IDescription = Convert.ToString(IDescription);

            object Price;
            test.TryGetValue("Price", out Price);
            int _Price = Convert.ToInt32(Price);
          

            var connection = DbUtils.GetDBConnection();
            var compiler = new SqlServerCompiler();
            var db = new QueryFactory(connection, compiler);
            db.Connection.Open();
           // using (var scope = db.Connection.BeginTransaction())
            using(TransactionScope scope = new TransactionScope())
            {
                try
                {
                    Dictionary<string, object> fooditemobj = new Dictionary<string, object>()
                    {

                        { "ItemName", _ItemName},
                        { "ItemStatus",_ItemStatus  },
                        { "IDescription",  _IDescription},
                        { "Price", _Price }
                    };
                var res = db.Query("fooditem").InsertGetId<int>(new
                {
                    ItemName = _ItemName,
                    ItemStatus = _ItemStatus,
                    IDescription = _IDescription,
                    Price = _Price
                });
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
                    scope.Complete();
                db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, object>() { { "LastInsertedId", res } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }


        }
    }
}
