using IptApis.Models.FacultyRecruitment;
using IptApis.Shared;
using Newtonsoft.Json;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;


namespace IptApis.Controllers.FacultyRecruitment
{
    public class EmployeeController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetAllEmployees()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<Employee> response = db.Query("Employee").Get<Employee>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        public HttpResponseMessage GetEmployeeById(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<Employee> response = db.Query("Employee").Where("EmpID", id).Get<Employee>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        public HttpResponseMessage AddEmployee(Object employee)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(employee));

            object EmpName;
            test.TryGetValue("EmpName", out EmpName);
            string _EmpName = Convert.ToString(EmpName);

            object EPassword;
            test.TryGetValue("EPassword", out EPassword);
            string _Epassword = Convert.ToString(EPassword);


            object Email;
            test.TryGetValue("EMail", out Email);
            string _EMail = Convert.ToString(Email);

            object MobileNumber;
            test.TryGetValue("MobileNumber", out MobileNumber);
            string _MobileNumber = Convert.ToString(MobileNumber);

            object DesignationID;
            test.TryGetValue("DesignationID", out DesignationID);
            int _DesignationID = Convert.ToInt32(DesignationID);

            object DepartmentID;
            test.TryGetValue("DepartmentID", out DepartmentID);
            int _DepartmentID = Convert.ToInt32(DepartmentID);

            object RefID;
            test.TryGetValue("RefID", out RefID);
            int _RefID = Convert.ToInt32(RefID);
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var EmpID = db.Query("Employee").InsertGetId<int>(new
                    {

                       EmpName =_EmpName,
                       EPassword = _Epassword,
                       EMail = _EMail,
                       MobileNumber = _MobileNumber,
                       DesignationID = _DesignationID,
                       DepartmentID = _DepartmentID,
                       RefID = _RefID
                    });
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, EmpID);//, new Dictionary<string, object>() { { "LastInsertedId", res } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();   //if there are any error, rollback the transaction
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteEmployee(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            try
            {
                _ = db.Query("Employee").Where("EmpID", "=", id).Delete();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        public HttpResponseMessage UpdateEmployee(Object employee)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(employee));

            object EmpID;
            test.TryGetValue("EmpID", out EmpID);
            int _EmpID = Convert.ToInt32(EmpID);

            object EmpName;
            test.TryGetValue("EmpName", out EmpName);
            string _EmpName = Convert.ToString(EmpName);

            object EPassword;
            test.TryGetValue("EPassword", out EPassword);
            string _Epassword = Convert.ToString(EPassword);


            object Email;
            test.TryGetValue("EMail", out Email);
            string _EMail = Convert.ToString(Email);

            object MobileNumber;
            test.TryGetValue("MobileNumber", out MobileNumber);
            string _MobileNumber = Convert.ToString(MobileNumber);

            object DesignationID;
            test.TryGetValue("DesignationID", out DesignationID);
            int _DesignationID = Convert.ToInt32(DesignationID);

            object DepartmentID;
            test.TryGetValue("DepartmentID", out DepartmentID);
            int _DepartmentID = Convert.ToInt32(DepartmentID);

            object RefID;
            test.TryGetValue("RefID", out RefID);
            int _RefID = Convert.ToInt32(RefID);

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var affected = db.Query("Employee").Where("EmpID", _EmpID).Update(new
                    {

                        EmpName = _EmpName,
                        EPassword = _Epassword,
                        EMail = _EMail,
                        MobileNumber = _MobileNumber,
                        DesignationID = _DesignationID,
                        DepartmentID = _DepartmentID,
                        RefID = _RefID
                    });
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.OK);//, new Dictionary<string, object>() { { "LastInsertedId", res } });
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
