using IptApis.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IptApis.Controllers.Cafeteria
{
    public class TestCafeteriaController : ApiController
    {
        public string Get()
        {
            return "Hello this is cafeteria";
        }
        [HttpGet]
        public string testconnection()
        {
            var connection =DbUtils.GetDBConnection();

            connection.Open();
            if(connection.State== ConnectionState.Open)
            {
                return ("connection success");
            }
            else
            {
                connection.Open();
            }
            return ("test connection");

        }
    }
}
