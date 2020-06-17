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
    public class StudentController : ApiController
    {
        //Method to get student
            public HttpResponseMessage GetStudent()
            {
                var db = DbUtils.GetDBConnection();   //GetDBConnection will return a DBFactory type object, which have establish sql connection
                db.Connection.Open();
                //var res = db.Query("fooditem").Get();
                IEnumerable<IDictionary<string, object>> response;
                response = db.Query("Student").Get().Cast<IDictionary<string, object>>();   

                return Request.CreateResponse(HttpStatusCode.OK, response);   //send list of items and Status code =200
            }
        //Method to get student and his programme by roll number(16k3943)
        public HttpResponseMessage GetStudentbyID(string id)
            {

                var db = DbUtils.GetDBConnection();
                db.Connection.Open();
                IEnumerable<IDictionary<string, object>> response;
                response = db.Query("Student").Where("RollNumber", id).Join("Programme", "Programme.ProgrammeID", "Student.ProgrammeID").Get().Cast<IDictionary<string, object>>();  //get product by id=1
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }
    }



