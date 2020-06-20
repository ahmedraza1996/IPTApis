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
    public class admincontroller : ApiController
    {
        private static string CONNSTRING = ConfigurationManager.AppSettings["SqlDBConn"];

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpGet]
        //api/admin/loginadmin/1
        public HttpResponseMessage LoginAdmin(int id)
        {
            var db = DbUtils.GetDBConnection();   //GetDBConnection will return a DBFactory type object, which have establish sql connection
            db.Connection.Open();

            IEnumerable<admin> response;
            response = db.Query("admin").Where("adminId", id).Get<admin>();

            return Request.CreateResponse(HttpStatusCode.OK,response);





            //if (model == null || !ModelState.IsValid) return Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid credentials");
            //Tuple<HttpStatusCode, string> error;
            //var log = dbo.admins.Where(a => a.adminEmail.Equals(adm.adminEmail) && a.adminPassword.Equals(adm.adminPassword)).FirstOrDefault();
            //var response = Request.CreateResponse(HttpStatusCode.OK, admin);
            //response.Headers.Add("Authorization", token);

        }


        [System.Web.Http.HttpPost]
        //[ValidateAntiForgeryToken]
        [System.Web.Http.Route("admin/adminLogin")]
        public HttpResponseMessage adminLogin([FromBody] dynamic adm)
        {
            //System.Diagnostics.Debug.WriteLine("Called admin login route");
            //var db = DbUtils.GetDBConnection();   //GetDBConnection will return a DBFactory type object, which have establish sql connection
            //db.Connection.Open();

            ////var log = db.Query("admin").Where(a => a.adminEmail.Equals(adm.adminEmail) && a.adminPassword.Equals(adm.adminPassword)).FirstOrDefault();
            // //Object log = (db.Query("admin").Where("adminEmail",adm.adminEmail).Where("adminPassword", adm.adminPassword));
            ////response = db.Query("TeacherCourseSectionDetail").Where("EmpName", empId).Where("CourseID", cid)/*.Get<Section>();*/

            //IEnumerable < admin > log;
            //log = db.Query("admin").Where("adminEmail", adm.adminEmail).Where("adminPassword", adm.adminPassword).POST<admin>();
            //System.Diagnostics.Debug.WriteLine(log);
            //return Request.CreateResponse(HttpStatusCode.OK, log);


            string query = $"SELECT * FROM ADMIN where adminEmail='{adm.adminEmail}' and adminPassword='{adm.adminPassword}'";
            using (SqlConnection sqlConnection = new SqlConnection(CONNSTRING))
            {
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                try
                {
                    String email = adm.adminEmail;
                    String password = adm.adminPassword;
                    sqlConnection.Open();
                    SqlDataReader adminReader = sqlCommand.ExecuteReader();
                    admin admn = null;
                    while (adminReader.Read())
                    {
                        admn = new admin(
                            adminReader.GetString(adminReader.GetOrdinal("adminName")),
                            adminReader.GetString(adminReader.GetOrdinal("adminEmail")),
                            adminReader.GetString(adminReader.GetOrdinal("adminPassword")),
                            adminReader.GetString(adminReader.GetOrdinal("adminPhone"))
                            );
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, admn);
                    
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e);

                }
            }


        }




        /*
         internalError = null;
			string query = $"INSERT INTO RESTAURANTS(RName, RAddress, RPhoneNumber, REmail, RPassword) VALUES ('{model.Name}', '{model.Address}','{model.PhoneNumber}','{model.Email}','{model.Password}')";
			SqlConnection sqlConnection = new SqlConnection(CONNSTRING);
			try
			{
				sqlConnection.Open();
				SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
				sqlCommand.ExecuteNonQuery();
				sqlCommand.Dispose();
				sqlConnection.Close();

				return new Restaurant(model.Id, model.Name, model.Email, model.Address, model.PhoneNumber);
			}
			catch (Exception e)
			{
				internalError = e.Message;
				sqlConnection.Close();
				return null;
			}
             */














        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}