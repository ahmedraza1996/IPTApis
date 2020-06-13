using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace IptApis.Controllers.Search_Module.QuerySearch
{
    public class SearchController : ApiController
    {
        public string Get()
        {
            return "Hello World";
        }
        [System.Web.Http.HttpGet]
        public string getInstructorDetailByName(String Name)
        {
            string queryString = "SELECT EmpID, EmpName, Email, MobileNumber  FROM dbo.Employee WHERE LOWER(EmpName) = @Name";

            string connectionString  = ConfigurationManager.AppSettings["SqlDBConn"].ToString();
            String result = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@Name", Name.ToLower());
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                
                try
                {
                    while (reader.Read())
                    {

                        result = reader.GetString(reader.GetOrdinal("EmpName"));
                       // TimetableModel temp = new TimetableModel();
                       // temp.coursename = reader.GetString(reader.GetOrdinal("CourseName"));
                       // temp.CourseInstructor = reader.GetString(reader.GetOrdinal("EmpName"));
                       // temp.Section = reader.GetString(reader.GetOrdinal("SectionName"));
                       // temp.Batch = reader.GetString(reader.GetOrdinal("BatchName"));
                       // temp.credithour = reader.GetInt32(reader.GetOrdinal("CreditHrs"));
                       // temp.day = 0;
                      //  temp.timeslot = 0;
                      //  TTM.Add(temp);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return result;
        }
    }
}