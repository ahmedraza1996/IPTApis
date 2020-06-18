using IptApis.Controllers.Search_Module.Content;
using IptApis.Controllers.Search_Module.Indexes;
using IptApis.Controllers.Search_Module.QueryParser;
using IptApis.Shared;
using Newtonsoft.Json;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace IptApis.Controllers.Search_Module.SearchFYP
{
    
    //[EnableCors(origins: "*", headers: "*", methods: "*")]

    public class SearchFYPController : ApiController
    {
        
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Hello World");
        }

        public HttpResponseMessage GetAllProjects()
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("FypProposal")
                .Select("ProposalID", "ProjectTitle", "ProjectType", "Abstract", "SupervisorID", "CoSupervisorID", "LeaderID", "Member1ID", "Member2ID", "Comment", "Status")
                .Where("Status", "Accepted")
                .Get()
                .Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        public HttpResponseMessage UpdateIndex()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> responses;
            responses = db.Query("FypProposal")
                .Select("ProposalID", "ProjectTitle", "ProjectType", "Abstract", "SupervisorID", "CoSupervisorID", "LeaderID", "Member1ID", "Member2ID", "Comment", "Status")
                .Where("Status", "Accepted")
                .Get()
                .Cast<IDictionary<string, object>>();

            DataStorage dataStorage = DataStorage.GetInstance();

            foreach (var response in responses)
            {
                int id = (int)response["ProposalID"];

                if (!dataStorage.FYP_Data.ContainsKey(id))
                {
                    string title = (string)response["ProjectTitle"];
                    string supervisor = ((int)response["SupervisorID"]).ToString();
                    string member1 = ((int)response["LeaderID"]).ToString();
                    string member2 = ((int)response["Member1ID"]).ToString();
                    string member3 = ((int)response["Member2ID"]).ToString();
                    string description = (string)response["Abstract"];

                    dataStorage.FYP_Data.Add(id, new FYPSearchModel(supervisor, member1, member2, member3, title, description));
                    Vector vector = new Vector(description);
                    dataStorage.wordsVector.Update(id, vector);
                    dataStorage.documentsVector.Update(id, vector);
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Indexes Updated");
        }

        [HttpPost]
        public HttpResponseMessage GetSearchResult(object data)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            DataStorage dataStorage = DataStorage.GetInstance();

            var jsonData = JsonConvert.SerializeObject(data);

            var dictJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);
            object queryObect;
            object userName;
            dictJson.TryGetValue("queried_by_username", out userName);

            dictJson.TryGetValue("query", out queryObect);
            string Query = queryObect.ToString();

            try
            {
                var parser = QueryParser.QueryParser.GetInstance();
                var indexes = parser.Parse(Query, dataStorage.wordsVector, dataStorage.documentsVector, dataStorage.documentsVector.DocumentsSet());

                var resultant = new List<IDictionary<string, object>>();
                foreach (var index in indexes)
                {
                    if (dataStorage.FYP_Data.ContainsKey(index))
                        resultant.Add(dataStorage.FYP_Data[index].cast());
                }


                SqlConnection dbConnection = new SqlConnection(ConfigurationManager.AppSettings["SqlDBConn"].ToString());

                try
                {
                    string query = "INSERT INTO dbo.SearchLog(input_query, actionName, queried_by_username) VALUES(@input_query,@actionName,@queried_by_username)";
                    using (SqlCommand command = new SqlCommand(query, dbConnection))
                    {
                        command.Parameters.AddWithValue("@input_query", queryObect);
                        command.Parameters.AddWithValue("@actionName", "FYPSearch");
                        command.Parameters.AddWithValue("@queried_by_username", userName.ToString());

                        dbConnection.Open();
                        int result = command.ExecuteNonQuery();

                        // Check Error
                        if (result < 0)
                            Console.WriteLine("Error inserting data into Database!");
                    }

                }
                catch (Exception e)
                {


                }




                return Request.CreateResponse(HttpStatusCode.OK, resultant);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(e.Message));
            }

        }

    }
}