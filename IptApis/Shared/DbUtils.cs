using SqlKata.Compilers;
using SqlKata.Execution;
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

        public static QueryFactory GetDBConnection()
        {
            var connection = new SqlConnection(
                           ConfigurationManager.AppSettings["SqlDBConn"].ToString());
             

            var compiler = new SqlServerCompiler();
            var db = new QueryFactory(connection, compiler);

            return ( db  );

        }

    }
}