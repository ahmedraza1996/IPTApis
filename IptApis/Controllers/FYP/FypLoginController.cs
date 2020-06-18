using IptApis.Shared;
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

namespace IptApis.Controllers.FYP
{
    public class FypLoginController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage login([FromUri] string id, string password)
        {
            string _StudentID = Convert.ToString(id);
            string _StudentPassword = Convert.ToString(password);

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;

            response = db.Query("Student").Where(new
            {
                RollNumber = _StudentID,
                SPassword = _StudentPassword,
            })
            .AsCount().Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        public HttpResponseMessage login([FromUri] int id)
        {
            string _StudentID = Convert.ToString(id);
            _StudentID = _StudentID.Insert(2, "k");

            string newid = Convert.ToString(id);
            newid = newid.Insert(2, "K");

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;

            response = db.Query("Student").Select("SPassword").Where("RollNumber", _StudentID).OrWhere("RollNumber", newid).Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        [HttpGet]
        public HttpResponseMessage loginExternal([FromUri] string id)
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;

            response = db.Query("FypExternalJuryy").Select("Password").Where("Username", id).Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}

