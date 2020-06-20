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

    public class admin_facultyController : ApiController
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


        [System.Web.Http.HttpPost]
        //[ValidateAntiForgeryToken]
        [System.Web.Http.Route("admin_faculty/Login")]
        public HttpResponseMessage Login(Object User)
        {
            HttpStatusCode statusCode = HttpStatusCode.Unauthorized;
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(User));
            object Cred;
            test.TryGetValue("Cred", out Cred);
            object Password;
            test.TryGetValue("SPassword", out Password);
            string _Password = Password.ToString();

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Employee").Where("Email", Cred).Get().Cast<IDictionary<string, object>>();
            var strResponse = response.ElementAt(0).ToString().Replace("DapperRow,", "").Replace("=", ":");
            Dictionary<string, string> temp = JsonConvert.DeserializeObject<Dictionary<string, string>>(strResponse);
            bool hasData = (response != null) ? true : false;

            if (hasData)
            {
                string pass;
                temp.TryGetValue("EPassword", out pass);

                if (pass.Equals(_Password))
                {
                    statusCode = HttpStatusCode.OK;
                    return Request.CreateResponse(statusCode, response.ElementAt(0));
                }
                else
                {
                    return Request.CreateErrorResponse(statusCode, "Invalid Password");
                }

            }
            else
            {
                statusCode = HttpStatusCode.NotFound;
                return Request.CreateErrorResponse(statusCode, "User not found");
            }

        }







    }
}