using Dapper;
using IptApis.Models.JobPortal;
using IptApis.Shared;
using Newtonsoft.Json;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;

namespace IptApis.Controllers.JobPortal
{
    public class StudentProfileController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
//        [Route("api/upload/{newCV}")]
        public void UploadCV([FromBody]CV newCV)
        {
            
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                //cmd.CommandTimeout = 0;

                string commandText = "INSERT INTO CV VALUES(@studentId, @name, @data,@contentType )";
            
                cmd.CommandText = commandText;
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("@studentId", SqlDbType.Int);
                cmd.Parameters.Add("@name", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@contentType", SqlDbType.VarChar,50);
                cmd.Parameters.Add("@data", SqlDbType.VarBinary);

                cmd.Parameters["@studentId"].Value = newCV.studentId;
                cmd.Parameters["@name"].Value = newCV.name;
                cmd.Parameters["@contentType"].Value = newCV.contentType;                
                cmd.Parameters["@data"].Value = newCV.data;
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        [HttpGet]
        public HttpResponseMessage DownloadCV(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            CV myCV = db.Query("CV").Where("StudentId", id).Get<CV>().First();
            return Request.CreateResponse(HttpStatusCode.OK, myCV);
        }
        public HttpResponseMessage GetProjectsByID(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<Project> response = db.Query("AllProjects").Where("StudentID", id).Get<Project>();//;.Cast<ProjectModel>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        public HttpResponseMessage GetSkillsByID(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<Skill> response = db.Query("AllSkills").Where("StudentID", id).Get<Skill>();//;.Cast<ProjectModel>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        public HttpResponseMessage GetExperienceByID(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<Experience> response = db.Query("AllExperience").Where("StudentID", id).Get<Experience>();//;.Cast<ProjectModel>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
            

        }
        public HttpResponseMessage GetAllFrameworkName()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<Framework> response = db.Query("FrameworkLanguage").Get<Framework>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        public HttpResponseMessage GetOrganizations()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<Organization> response = db.Query("AffiliatedOrganization").Get<Organization>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        public HttpResponseMessage GetDomains()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<Domain> response = db.Query("Domain").Get<Domain>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage AddProject(Project test)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            int _frameworkID;
            int _DomainID;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if(test.FrameworkID == 0){
                        var frameworkID = db.Query("FrameworkLanguage").InsertGetId<int>(new{
                            Fname = test.Fname
                        });
                        _frameworkID = frameworkID;
                    }
                    else
                        _frameworkID = test.FrameworkID;

                    if (test.DomainID == 0)
                    {
                        var DomainID = db.Query("Domain").InsertGetId<int>(new{
                                DomainName = test.DomainName
                         });
                        _DomainID = DomainID;
                    }
                    else
                        _DomainID = test.DomainID;
                        
                    var _ProjectID = db.Query("StudentProject").InsertGetId<int>(new
                    {
                        ProjectName = test.ProjectName,
                        GitHubLink = test.GithubLink,
                        StudentID = test.StudentID,
                        CourseOfferedID = test.courseOfferedID
                    });
                     _ = db.Query("ProjectFramework").Insert(new
                        {
                            projectID = _ProjectID,
                            Status = test.Status,
                            FID = _frameworkID
                        }) ;
                    var SkillID = db.Query("Skill").InsertGetId<int>(new
                    {
                        SkillName = test.Skillvalue,
                        DomainID = test.DomainID
                    }) ;
                    _ = db.Query("ProjectSkills").Insert(new
                    {
                        SkillID = SkillID,
                        ProjectID = _ProjectID,
                        ApproveStatus = test.Status
                    });
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created,_ProjectID);//, new Dictionary<string, object>() { { "LastInsertedId", res } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();   //if there are any error, rollback the transaction
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }


            }
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage AddExperience(Experience newExp)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            int OrganizationID;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if (newExp.OrganizationID==0)
                    {
                        var OID = db.Query("AffiliatedOrganization").InsertGetId<int>(new
                        {
                            OrganizationName = newExp.OrganizationName
                        });
                        OrganizationID  = OID;
                    }
                    else
                    {
                        OrganizationID = newExp.OrganizationID;
                    }
                    var ExpID = db.Query("StudentExperience").InsertGetId<int>(new
                    {
                        startDate = newExp.StartDate,
                        endDate = newExp.EndDate,
                        designation = newExp.Designation,
                        JDescription = newExp.JDescription,
                        studentID = newExp.StudentID,
                        OrganizationID = OrganizationID
                    });
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created);//, new Dictionary<string, object>() { { "LastInsertedId", res } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();   //if there are any error, rollback the transaction
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteExperience(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            try
            {
                _ = db.Query("StudentExperience").Where("ExpID", "=", id).Delete();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch(Exception e) { 
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
       
        
        //[HttpDelete]
        //[AllowAnonymous]
        public HttpResponseMessage DeleteProject(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            try { 
                _ = db.Query("ProjectSkills").Where("projectID", "=", id).Delete();
                _ = db.Query("ProjectFramework").Where("projectID", "=", id).Delete();
                _ = db.Query("StudentProject").Where("projectID", "=", id).Delete();


                return Request.CreateResponse(HttpStatusCode.OK);
            }catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        public HttpResponseMessage getProjectSkills() {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<Object> response = db.Query("ProjectSkills").Get(); //.Cast<ProjectModel>();
            return Request.CreateResponse(HttpStatusCode.OK,response);
        }

        [HttpPost]
        [AllowAnonymous]
        public String ApproveProject(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    int studentID = db.Query("StudentProject").Where("ProjectID", id).Select("StudentID").Get<int>().First();
                    int skillID = db.Query("ProjectSkills").Where("ProjectID", id).Select("SkillID").Get<int>().First();

                    int affected = db.Query("ProjectSkills").Where("ProjectID", id).Update(new
                    {
                        ApproveStatus = "Accepted"
                    });
                    int affected2 = db.Query("ProjectFramework").Where("ProjectID", id).Update(new
                    {
                        status = "Approved"
                    });

                    var RefID = db.Query("StudentSkills").InsertGetId<int>(new
                    {
                        StudentID = studentID,
                        SkillID = skillID
                    });

                    IEnumerable<Skill> response = db.Query("AllSkills").Where("StudentID", studentID).Get<Skill>();//;.Cast<ProjectModel>();


                    return ("The Project has been approved and added as a skill of that candidate");

                }
                catch
                {
                    scope.Dispose();
                    return ("Error! Dubara Try kro ");

                }
            }
        }

    }
}