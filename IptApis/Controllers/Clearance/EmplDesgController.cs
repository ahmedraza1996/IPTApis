using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IptApis.Shared;
using Newtonsoft.Json;
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Configuration;
using System.Data.SqlClient;

namespace IptApis.Controllers.Clearance
{
    public class EmplDesgController : ApiController
    {
        //Method to get employee and his designation by ID
        public HttpResponseMessage GetEmployeeDesignationbyID(int id)
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Employee").Select("EmpName","Email","Designation.DesignationTitle").Where("EmpID", id).Join("Designation", "Designation.DesignationID", "Employee.DesignationID").Get().Cast<IDictionary<string, object>>();  //get product by id=1
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

       /* public HttpResponseMessage GetDesignationbyID(string id)
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Designation").Where("DesignationID", id).Get().Cast<IDictionary<string, object>>();  //get product by id=1
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }*/
    }
}
