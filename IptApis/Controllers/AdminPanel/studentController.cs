using IptApis.Models.AdminPanel;
using IptApis.Shared;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;

namespace IptApis.Controllers.AdminPanel
{

    [System.Web.Http.RoutePrefix("api")]

    public class studentController : ApiController
    {

        private static string CONNSTRING = ConfigurationManager.AppSettings["SqlDBConn"];

        // GET: student
        /*public ActionResult Index()
        {
            return View();
        }*/


        [System.Web.Http.HttpPost]
        [ValidateAntiForgeryToken]
        [System.Web.Http.Route("student/studentLogin")]
        public HttpResponseMessage studentLogin([FromBody] dynamic std)
        {


            string query = $"SELECT * FROM Student where Email='{std.Email}' and SPassword='{std.SPassword}'";
            using (SqlConnection sqlConnection = new SqlConnection(CONNSTRING))
            {
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                try
                {
                    String email = std.Email;
                    String password = std.SPassword;
                    sqlConnection.Open();
                    SqlDataReader studentReader = sqlCommand.ExecuteReader();
                    students stdnt = null;
                    while (studentReader.Read())
                    {
                        stdnt = new students(
                            studentReader.GetString(studentReader.GetOrdinal("EmpName")),
                            studentReader.GetString(studentReader.GetOrdinal("Email")),
                            studentReader.GetString(studentReader.GetOrdinal("MobileNumber")),
                            studentReader.GetString(studentReader.GetOrdinal("RollNumber")),
                            studentReader.GetString(studentReader.GetOrdinal("EPassword"))
                            );
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, stdnt);

                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e);

                }
            }




        }







    }
}