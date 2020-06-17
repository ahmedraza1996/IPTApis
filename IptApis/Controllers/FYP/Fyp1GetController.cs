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
    public class Fyp1GetController : ApiController

    //Proposal
    {
        public HttpResponseMessage GetProposalsName()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("FypProposal").Select("ProjectTitle", "ProposalID").Where("SupervisorID", 123).OrWhere("CoSupervisorID", 1234).Get().Cast<IDictionary<string, object>>();


            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        public HttpResponseMessage GetProposalDetails([FromUri] int id)
        {
            string _StudentID = Convert.ToString(id);


            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;

            response = db.Query("FypProposal").Select("ProposalID", "ProjectTitle", "ProjectType", "Abstract", "SupervisorID", "CoSupervisorID", "LeaderID", "Member1ID", "Member2ID", "Comment", "Status")
               .Where("LeaderID", _StudentID).OrWhere("Member1ID", _StudentID).OrWhere("Member2ID", _StudentID)
               .Get()
               .Cast<IDictionary<string, object>>();


            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetProposalsNameSupervisor([FromUri] int id)
        {
            string _TeacherID = Convert.ToString(id);

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;

            response = db.Query("FypProposal").Select("ProposalID", "ProjectTitle", "ProjectType", "Abstract", "SupervisorID", "CoSupervisorID", "LeaderID", "Member1ID", "Member2ID")
               .Where("SupervisorID", _TeacherID).OrWhere("CoSupervisorID", _TeacherID)
               .Get()
               .Cast<IDictionary<string, object>>();


            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        public HttpResponseMessage ProposalStatus([FromUri] int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("FypProposal").Where("LeaderID", id).Where("Status", "Accepted").OrWhere("Status", "Rejected").AsCount()
                .Get().Cast<IDictionary<string, object>>();


            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        public HttpResponseMessage fyp1proposal_status([FromUri] int id)

        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("FypProposal").Select("Status").Where("LeaderID", id).OrWhere("Member1ID", id).OrWhere("Member2ID", id)
                .Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        //General

        public HttpResponseMessage GetFypNames([FromUri] int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Fyp").Select("Fyp.ProjectName", "Fyp.FypID").Where("FypJury.EmpID", id).Join("FypJury", "Fyp.FypID", "FypJury.FypID").Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetFypDetailsByTitle([FromUri] string title)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Fyp").Select("Fyp.FypID", "Fyp.CoSuperVisorID", "Fyp.SupervisorEmpID", "FypMembers.LeaderID", "FypMembers.Member1ID", "FypMembers.Member2ID").Where("Fyp.ProjectName", title).Join("FypMembers", "Fyp.FypID", "FypMembers.FypID").Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetFypDeliverablesDetailsByTitle([FromUri] string title)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Fyp").Select("FypID").Where("ProjectName", title).Get().Cast<IDictionary<string, object>>();

            var strResponse = response.ElementAt(0).ToString().Replace("DapperRow,", "").Replace("=", "").Replace("FypID", "").Replace("'", "").Replace("}", "").Replace("{", "");
            var result = Convert.ToInt32(strResponse);

            IEnumerable<IDictionary<string, object>> response1;
            response1 = db.Query("FypEvaluation").Select("Deliverables1", "Deliverables2", "Deliverables3", "Deliverables4", "Deliverables5").Where(new
            {
                FypID = result,
                FormID = 1
            }).Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response1);
        }

        //ProposalEvaluation

        public HttpResponseMessage GetProposalEvaluations([FromUri] int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Fyp").Select("Fyp.ProjectName", "FypMembers.LeaderID", "FypMembers.Member1ID", "FypMembers.Member2ID"
                , "Fyp.SuperVisorEmpID", "Fyp.CoSuperVisorID", "FypEvaluation.DefenceStatus", "FypEvaluation.Criteria1Marks", "FypEvaluation.Criteria2Marks",
                "FypEvaluation.Criteria3Marks", "FypEvaluation.Criteria4Marks", "FypEvaluation.Criteria5Marks", "FypEvaluation.Deliverables1",
                "FypEvaluation.Deliverables2", "FypEvaluation.Deliverables3", "FypEvaluation.Deliverables4", "FypEvaluation.Deliverables5",
                "FypEvaluation.ChangesRecommeneded").Where("FypEvaluation.FormID", 1).Where("FypMembers.LeaderID", id).OrWhere("FypEvaluation.FormID", 1)
                .Where("FypMembers.Member1ID", id).OrWhere("FypEvaluation.FormID", 1).Where("FypMembers.Member2ID", id)
                .Join("FypEvaluation", "Fyp.FypID", "FypEvaluation.FypID")
                .Join("FypMembers", "Fyp.FypID", "FypMembers.FypID")
                .Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        public HttpResponseMessage DefenseStatus([FromUri] int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("FypEvaluation").Where("FypID", id).Where("FormID", 1).AsCount()
                .Get().Cast<IDictionary<string, object>>();


            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        //FinalEvaluation
        [HttpGet]
        public HttpResponseMessage GetFinalEvaluations([FromUri] int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Fyp").Select("Fyp.ProjectName", "FypMembers.LeaderID", "FypMembers.Member1ID", "FypMembers.Member2ID",
                "Fyp.SuperVisorEmpID", "Fyp.CoSuperVisorID", "FypEvaluation.Deliverables1", "FypEvaluation.Deliverables2",
                "FypEvaluation.Deliverables3", "FypEvaluation.Deliverables4", "FypEvaluation.Deliverables5", "FypEvaluation.Deliverable1Completion",
                "FypEvaluation.Deliverable2Completion", "FypEvaluation.Deliverable3Completion", "FypEvaluation.Deliverable4Completion",
                "FypEvaluation.Deliverable5Completion", "Fyp2Deliverables")
                .Where("FypEvaluation.FormID", 2).Where("FypMembers.LeaderID", id).OrWhere("FypEvaluation.FormID", 2)
                .Where("FypMembers.Member1ID", id).OrWhere("FypEvaluation.FormID", 2).Where("FypMembers.Member2ID", id)
                .Join("FypEvaluation", "Fyp.FypID", "FypEvaluation.FypID")
                .Join("FypMembers", "Fyp.FypID", "FypMembers.FypID")
                .Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        public HttpResponseMessage Fyp1FinalStatus([FromUri] int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("FypEvaluation").Where("FypID", id).Where("FormID", 2).AsCount()
                .Get().Cast<IDictionary<string, object>>();


            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        //Get marks to show to student for fyp1 final
        [HttpGet]
        public HttpResponseMessage GetFyp1FinalMarks([FromUri] int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;

            response = db.Query("FypMarks").Select("Marks")
               .Where("StudentID", id).Where("FormID", 2)
               .Get()
               .Cast<IDictionary<string, object>>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetFinalEval([FromUri] int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Fyp").Select("FypMembers.LeaderID", "FypMembers.Member1ID", "FypMembers.Member2ID",
                "Fyp.SuperVisorEmpID", "Fyp.CoSuperVisorID", "FypEvaluation.Deliverables1", "FypEvaluation.Deliverables2",
                "FypEvaluation.Deliverables3", "FypEvaluation.Deliverables4", "FypEvaluation.Deliverables5", "FypEvaluation.Deliverable1Completion",
                "FypEvaluation.Deliverable2Completion", "FypEvaluation.Deliverable3Completion", "FypEvaluation.Deliverable4Completion",
                "FypEvaluation.Deliverable5Completion", "FypEvaluation.Fyp2Deliverables")
                .Where("FypEvaluation.FormID", 2).Where("FypEvaluation.FypID", id)
                .Join("FypEvaluation", "Fyp.FypID", "FypEvaluation.FypID")
                .Join("FypMembers", "Fyp.FypID", "FypMembers.FypID")
                .Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetFinalEvaluate([FromUri] int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Fyp").Select("FypEvaluation.FypID", "FypEvaluation.FormID",
                "Fyp.SuperVisorEmpID", "Fyp.CoSuperVisorID", "FypEvaluation.Deliverables1", "FypEvaluation.Deliverables2",
                "FypEvaluation.Deliverables3", "FypEvaluation.Deliverables4", "FypEvaluation.Deliverables5", "FypEvaluation.Fyp2Deliverables",
                "FypMembers.LeaderID", "FypMembers.Member1ID", "FypMembers.Member2ID")
                .Where("FypEvaluation.FormID", 2).Where("FypMembers.LeaderID", id).OrWhere("FypEvaluation.FormID", 2)
                .Where("FypMembers.Member1ID", id).OrWhere("FypEvaluation.FormID", 2).Where("FypMembers.Member2ID", id)
                .Join("FypEvaluation", "Fyp.FypID", "FypEvaluation.FypID")
                .Join("FypMembers", "Fyp.FypID", "FypMembers.FypID")
                .Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetProposalEval([FromUri] int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Fyp").Select("Fyp.FypID", "FypEvaluation.FormID", "FypEvaluation.Criteria1Marks", "FypEvaluation.Criteria2Marks",
                "FypEvaluation.Criteria3Marks", "FypEvaluation.Criteria4Marks", "FypEvaluation.Criteria5Marks", "FypEvaluation.Deliverables1",
                "FypEvaluation.Deliverables2", "FypEvaluation.Deliverables3", "FypEvaluation.Deliverables4", "FypEvaluation.Deliverables5",
                "FypEvaluation.ChangesRecommeneded", "FypEvaluation.DefenceStatus", "FypMembers.LeaderID", "FypMembers.Member1ID", "FypMembers.Member2ID").Where("FypEvaluation.FormID", 1).Where("FypEvaluation.FypID", id)
                .Join("FypEvaluation", "Fyp.FypID", "FypEvaluation.FypID")
                .Join("FypMembers", "Fyp.FypID", "FypMembers.FypID")
                .Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetProposalEvaluate([FromUri] int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Fyp").Select("Fyp.FypID", "FypEvaluation.FormID", "FypEvaluation.Criteria1Marks", "FypEvaluation.Criteria2Marks",
                "FypEvaluation.Criteria3Marks", "FypEvaluation.Criteria4Marks", "FypEvaluation.Criteria5Marks", "FypEvaluation.Deliverables1",
                "FypEvaluation.Deliverables2", "FypEvaluation.Deliverables3", "FypEvaluation.Deliverables4", "FypEvaluation.Deliverables5",
                "FypEvaluation.ChangesRecommeneded", "FypEvaluation.DefenceStatus", "FypMembers.LeaderID", "FypMembers.Member1ID", "FypMembers.Member2ID").Where("FypEvaluation.FormID", 1).Where("FypMembers.LeaderID", id).OrWhere("FypEvaluation.FormID", 1)
                .Where("FypMembers.Member1ID", id).OrWhere("FypEvaluation.FormID", 1).Where("FypMembers.Member2ID", id)
                .Join("FypEvaluation", "Fyp.FypID", "FypEvaluation.FypID")
                .Join("FypMembers", "Fyp.FypID", "FypMembers.FypID")
                .Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetFypDeliverablesDetailsByid([FromUri] int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Fyp").Select("FypID").Where("FypID", id).Get().Cast<IDictionary<string, object>>();

            var strResponse = response.ElementAt(0).ToString().Replace("DapperRow,", "").Replace("=", "").Replace("FypID", "").Replace("'", "").Replace("}", "").Replace("{", "");
            var result = Convert.ToInt32(strResponse);

            IEnumerable<IDictionary<string, object>> response1;
            response1 = db.Query("FypEvaluation").Select("Deliverables1", "Deliverables2", "Deliverables3", "Deliverables4", "Deliverables5").Where(new
            {
                FypID = result,
                FormID = 1
            }).Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response1);
        }

        public HttpResponseMessage GetFypDetailsBy([FromUri] int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Fyp").Select("Fyp.ProjectName", "Fyp.FypID", "Fyp.CoSuperVisorID", "Fyp.SupervisorEmpID", "FypMembers.LeaderID", "FypMembers.Member1ID", "FypMembers.Member2ID").Where("Fyp.FypID", id).Join("FypMembers", "Fyp.FypID", "FypMembers.FypID").Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage Getfypnamebyid([FromUri] int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Fyp").Select("Fyp.ProjectName", "Fyp.FypID")
            .Where("FypMembers.LeaderID", id).OrWhere("FypMembers.Member1ID", id).OrWhere("FypMembers.Member2ID", id)
            .Join("FypMembers", "Fyp.FypID", "FypMembers.FypID")
            .Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetNameFyp([FromUri] int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;
            response = db.Query("Fyp").Select("Fyp.ProjectName", "Fyp.SuperVisorEmpID", "Fyp.CoSuperVisorID").Where("Fyp.FypID", id).Get().Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpGet]
        public HttpResponseMessage Getsupervisorsproposal([FromUri] int id)
        {
            string _StudentID = Convert.ToString(id);

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IDictionary<string, object>> response;

            response = db.Query("FypProposal").Select("ProposalID", "ProjectTitle", "ProjectType", "Abstract", "SupervisorID", "CoSupervisorID", "LeaderID", "Member1ID", "Member2ID", "Comment", "Status")
               .Where("SupervisorID", _StudentID).OrWhere("CoSupervisorID", _StudentID)
               .Get()
               .Cast<IDictionary<string, object>>();


            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

    }
}