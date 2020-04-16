using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IptApis.Shared
{
    public class DbUtils
    {

        public static SqlConnection GetDBConnection()
        {
            return (  new SqlConnection(
                            ConfigurationManager.AppSettings["AWS"].ToString()) 
                    );

        }

    }
}