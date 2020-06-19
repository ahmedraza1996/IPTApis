using IptApis.Models.AdminPanel;
using IptApis.Shared;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;

namespace IptApis.Controllers.AdminPanel
{

    [System.Web.Http.RoutePrefix("api")]

    public class facultyController : ApiController
    {

        private static string CONNSTRING = ConfigurationManager.AppSettings["SqlDBConn"];

        // GET: faculty
        /*public ActionResult Index()
        {
            return View();
        }*/

        [System.Web.Http.HttpPost]
        //[ValidateAntiForgeryToken]
        [System.Web.Http.Route("faculty/facultyLogin")]
        public HttpResponseMessage facultyLogin([FromBody] dynamic fc)
        {


            string query = $"SELECT * FROM Employee where Email='{fc.Email}' and EPassword='{fc.EPassword}'";
            using (SqlConnection sqlConnection = new SqlConnection(CONNSTRING))
            {
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                try
                {
                    String email = fc.Email;
                    String password = fc.EPassword;
                    sqlConnection.Open();
                    SqlDataReader facultyReader = sqlCommand.ExecuteReader();
                    faculties faclty = null;
                    while (facultyReader.Read())
                    {
                        faclty = new faculties(
                            facultyReader.GetString(facultyReader.GetOrdinal("EmpName")),
                            facultyReader.GetString(facultyReader.GetOrdinal("Email")),
                            facultyReader.GetString(facultyReader.GetOrdinal("MobileNumber")),
                            facultyReader.GetString(facultyReader.GetOrdinal("EPassword"))
                            );
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, faclty);

                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e);

                }
            }


        }









    }
}