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
using System.Web.Http.Cors;

namespace IptApis.Controllers.FacultyRecruitment
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NucesJobAccountController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetRolesbyUserName(string id)
        {
            id = id.Replace('-', '.');
            //var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(data));

            //object username;
            //test.TryGetValue("username", out username);
            //string _username = Convert.ToString(username);
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<UserRole> response = db.Query("UserRole").Join("AllUsers", "AllUsers.id", "UserRole.UserId").Where("AllUsers.UserName", id).Select("UserRole.Role").Get<UserRole>();//;.Cast<ProjectModel>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

    }
}
