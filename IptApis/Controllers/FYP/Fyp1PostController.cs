using Newtonsoft.Json;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Transactions;
using IptApis.Shared;
using SqlKata;
using SqlKata.Compilers;
using System.Linq;

namespace IptApis.Controllers.FYP
{
    public class Fyp1PostController : ApiController
    {

        public HttpResponseMessage AddProposalStudent(Object Proposal)
        {

            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(Proposal));

            object ProjectTitle;
            test.TryGetValue("ProjectTitle", out ProjectTitle);
            string _ProjectTitle = Convert.ToString(ProjectTitle);

            object ProjectType;
            test.TryGetValue("ProjectType", out ProjectType);
            string _ProjectType = Convert.ToString(ProjectType);

            object Abstract;
            test.TryGetValue("Abstract", out Abstract);
            string _Abstract = Convert.ToString(Abstract);

            object SupervisorID;
            test.TryGetValue("SupervisorID", out SupervisorID);
            int _SupervisorID = Convert.ToInt32(SupervisorID);

            object CoSupervisorID;
            test.TryGetValue("CoSupervisorID", out CoSupervisorID);
            int _CoSupervisorID = Convert.ToInt32(CoSupervisorID);

            object LeaderID;
            test.TryGetValue("LeaderID", out LeaderID);
            int _LeaderID = Convert.ToInt32(LeaderID);

            object Member1ID;
            test.TryGetValue("Member1ID", out Member1ID);
            double _Member1ID = 0;
            if (Member1ID.Equals(""))
            {
                _Member1ID = 0;
            }
            else
            {
                _Member1ID = Convert.ToDouble(Member1ID);
            }

            object Member2ID;
            test.TryGetValue("Member2ID", out Member2ID);
            double _Member2ID = 0;
            if (Member2ID.Equals(""))
            {
                _Member2ID = 0;
            }
            else
            {
                _Member2ID = Convert.ToDouble(Member2ID);
            }

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();


            using (TransactionScope scope = new TransactionScope())
            {
                try
                {

                    var res = db.Query("FypProposal").Insert(new
                    {
                        ProjectTitle = _ProjectTitle,
                        ProjectType = _ProjectType,
                        Abstract = _Abstract,
                        SupervisorID = _SupervisorID,
                        CoSupervisorID = _CoSupervisorID,
                        LeaderID = _LeaderID,
                        Member1ID = _Member1ID,
                        Member2ID = _Member2ID,
                        Comment = "",
                        Status = "Pending"
                    });




                    scope.Complete();
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, object>() { { "LastInsertedId", res } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateProposalSupervisor(Object Proposal)
        {

            var data = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(Proposal));

            object ProjectTitle;
            data.TryGetValue("ProjectTitle", out ProjectTitle);
            string _ProjectTitle = Convert.ToString(ProjectTitle);

            object ProjectType;
            data.TryGetValue("ProjectType", out ProjectType);
            string _ProjectType = Convert.ToString(ProjectType);

            object Abstract;
            data.TryGetValue("Abstract", out Abstract);
            string _Abstract = Convert.ToString(Abstract);

            object SupervisorID;
            data.TryGetValue("SupervisorID", out SupervisorID);
            int _SupervisorID = Convert.ToInt32(SupervisorID);

            object CoSupervisorID;
            data.TryGetValue("CoSupervisorID", out CoSupervisorID);
            int _CoSupervisorID = Convert.ToInt32(CoSupervisorID);

            object Status;
            data.TryGetValue("Status", out Status);
            string _Status = Convert.ToString(Status);

            object Comment;
            data.TryGetValue("Comment", out Comment);
            string _Comment = Convert.ToString(Comment);

            object ProposalID;
            data.TryGetValue("ProposalID", out ProposalID);
            int _ProposalID = Convert.ToInt32(ProposalID);

            object LeaderID;
            data.TryGetValue("LeaderID", out LeaderID);
            int _LeaderID = Convert.ToInt32(LeaderID);

            object Member1ID;
            data.TryGetValue("Member1ID", out Member1ID);
            int _Member1ID = 0;
            if (Member1ID.Equals(""))
            {
                _Member1ID = 0;
            }
            else
            {
                _Member1ID = Convert.ToInt32(Member1ID);
            }

            object Member2ID;
            data.TryGetValue("Member2ID", out Member2ID);
            int _Member2ID = 0;
            if (Member2ID.Equals(""))
            {
                _Member2ID = 0;
            }
            else
            {
                _Member2ID = Convert.ToInt32(Member2ID);
            }

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if (_Status == "Accepted")
                    {

                        db.Query("FypProposal").Where("ProposalID", _ProposalID).Update(new
                        {
                            Status = _Status,
                            Comment = _Comment
                        });

                        var res = db.Query("Fyp").InsertGetId<int>(new
                        {
                            ProjectName = _ProjectTitle,
                            ProjectType = _ProjectType,
                            FypDescription = _Abstract,
                            SupervisorEmpID = _SupervisorID,
                            CoSuperVisorID = _CoSupervisorID,
                        });

                        db.Query("FypMembers").Insert(new
                        {
                            FypID = res,
                            LeaderID = _LeaderID,
                            Member1ID = _Member1ID,
                            Member2ID = _Member2ID,
                        });


                    }
                    else
                    {
                        db.Query("FypProposal").Where("ProposalID", _ProposalID).Delete();
                    }

                    scope.Complete();
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, object>() { { "Updated Succesfully", 0 } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }
        }

        [HttpPost]
        public HttpResponseMessage AddProposalEvaluationJury(Object Proposal)
        {

            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(Proposal));

            object FypID;
            test.TryGetValue("FypID", out FypID);
            int _FypID = Convert.ToInt32(FypID);

            object FormID;
            test.TryGetValue("FormID", out FormID);
            int _FormID = Convert.ToInt32(FormID);

            object Criteria1Marks;
            test.TryGetValue("Criteria1Marks", out Criteria1Marks);
            double _Criteria1Marks = Convert.ToDouble(Criteria1Marks);

            object Criteria2Marks;
            test.TryGetValue("Criteria2Marks", out Criteria2Marks);
            double _Criteria2Marks = Convert.ToDouble(Criteria2Marks);

            object Criteria3Marks;
            test.TryGetValue("Criteria3Marks", out Criteria3Marks);
            double _Criteria3Marks = Convert.ToDouble(Criteria3Marks);

            object Criteria4Marks;
            test.TryGetValue("Criteria4Marks", out Criteria4Marks);
            double _Criteria4Marks = Convert.ToDouble(Criteria4Marks);

            object Criteria5Marks;
            test.TryGetValue("Criteria5Marks", out Criteria5Marks);
            double _Criteria5Marks = Convert.ToDouble(Criteria5Marks);

            object Deliverables1;
            test.TryGetValue("Deliverables1", out Deliverables1);
            string _Deliverables1 = Convert.ToString(Deliverables1);

            object Deliverables2;
            test.TryGetValue("Deliverables2", out Deliverables2);
            string _Deliverables2 = "";
            if (Deliverables2.Equals(""))
            {
                _Deliverables2 = "";
            }
            else
            {
                _Deliverables2 = Convert.ToString(Deliverables2);
            }

            object Deliverables3;
            test.TryGetValue("Deliverables3", out Deliverables3);
            string _Deliverables3 = "";
            if (Deliverables3.Equals(""))
            {
                _Deliverables3 = "";
            }
            else
            {
                _Deliverables3 = Convert.ToString(Deliverables3);
            }

            object Deliverables4;
            test.TryGetValue("Deliverables4", out Deliverables4);
            string _Deliverables4 = "";
            if (Deliverables4.Equals(""))
            {
                _Deliverables4 = "";
            }
            else
            {
                _Deliverables4 = Convert.ToString(Deliverables4);
            }

            object Deliverables5;
            test.TryGetValue("Deliverables5", out Deliverables5);
            string _Deliverables5 = "";
            if (Deliverables5.Equals(""))
            {
                _Deliverables5 = "";
            }
            else
            {
                _Deliverables5 = Convert.ToString(Deliverables5);
            }

            object ChangesRecommeneded;
            test.TryGetValue("ChangesRecommeneded", out ChangesRecommeneded);
            string _ChangesRecommeneded = "";
            if (ChangesRecommeneded.Equals(""))
            {
                _ChangesRecommeneded = "";
            }
            else
            {
                _ChangesRecommeneded = Convert.ToString(ChangesRecommeneded);
            }

            object DefenceStatus;
            test.TryGetValue("DefenceStatus", out DefenceStatus);
            string _DefenceStatus = Convert.ToString(DefenceStatus);

            object LeaderID;
            test.TryGetValue("LeaderID", out LeaderID);
            int _LeaderID = Convert.ToInt32(LeaderID);

            object Member1ID;
            test.TryGetValue("Member1ID", out Member1ID);
            int _Member1ID = Convert.ToInt32(Member1ID);

            object Member2ID;
            test.TryGetValue("Member2ID", out Member2ID);
            int _Member2ID = Convert.ToInt32(Member2ID);


            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {

                    var res = db.Query("FypEvaluation").InsertGetId<int>(new
                    {
                        FypID = _FypID,
                        FormID = _FormID,
                        Criteria1Marks = _Criteria1Marks,
                        Criteria2Marks = _Criteria2Marks,
                        Criteria3Marks = _Criteria3Marks,
                        Criteria4Marks = _Criteria4Marks,
                        Criteria5Marks = _Criteria5Marks,
                        Deliverables1 = _Deliverables1,
                        Deliverables2 = _Deliverables2,
                        Deliverables3 = _Deliverables3,
                        Deliverables4 = _Deliverables4,
                        Deliverables5 = _Deliverables5,
                        ChangesRecommeneded = _ChangesRecommeneded,
                        DefenceStatus = _DefenceStatus
                    });

                    if (_DefenceStatus == "Rejected")
                    {
                        var compiler = new SqlServerCompiler();

                        IEnumerable<IDictionary<string, object>> response;
                        response = db.Query("Fyp").Select("ProjectName").Where("FypID", _FypID).Get().Cast<IDictionary<string, object>>();


                        var strResponse = response.ElementAt(0).ToString().Replace("DapperRow,", "").Replace("=", "").Replace("ProjectName", "").Replace("{", "").Replace("}", "").Replace("'", "");
                        var result = strResponse.ToString();

                        db.Query("FypProposal").Where("LeaderID", _LeaderID).Delete();
                        db.Query("Fyp").Where("FypID", _FypID).Delete();
                        db.Query("FypMembers").Where("FypID", _FypID).Delete();
                    }
                    else
                    {
                        var totalMarks = _Criteria1Marks + _Criteria2Marks + _Criteria3Marks + _Criteria4Marks + _Criteria5Marks;
                        db.Query("FypMarks").Insert(new
                        {
                            StudentID = _LeaderID,
                            FormID = 1,
                            Marks = totalMarks
                        });

                        db.Query("FypMarks").Insert(new
                        {
                            StudentID = _Member1ID,
                            FormID = 1,
                            Marks = totalMarks
                        });

                        db.Query("FypMarks").Insert(new
                        {
                            StudentID = _Member2ID,
                            FormID = 1,
                            Marks = totalMarks
                        });
                    }



                    scope.Complete();
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, object>() { { "LastInsertedId", res } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }
        }

        [HttpPost]
        public HttpResponseMessage AddFinalEvaluationJury(Object FinalEvaluation)
        {

            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(FinalEvaluation));

            object FypID;
            test.TryGetValue("FypID", out FypID);
            int _FypID = Convert.ToInt32(FypID);

            object FormID;
            test.TryGetValue("FormID", out FormID);
            int _FormID = Convert.ToInt32(FormID);

            object Deliverable1Completion;
            test.TryGetValue("Deliverable1Completion", out Deliverable1Completion);
            double _Deliverable1Completion = Convert.ToDouble(Deliverable1Completion);

            object Deliverable2Completion;
            test.TryGetValue("Deliverable2Completion", out Deliverable2Completion);

            double _Deliverable2Completion = 0;
            if (Deliverable2Completion.Equals(""))
            {
                _Deliverable2Completion = 0;
            }
            else
            {
                _Deliverable2Completion = Convert.ToDouble(Deliverable2Completion);
            }

            object Deliverable3Completion;
            test.TryGetValue("Deliverable3Completion", out Deliverable3Completion);
            double _Deliverable3Completion = 0;
            if (Deliverable3Completion.Equals(""))
            {
                _Deliverable3Completion = 0;
            }
            else
            {
                _Deliverable3Completion = Convert.ToDouble(Deliverable3Completion);
            }

            object Deliverable4Completion;
            test.TryGetValue("Deliverable4Completion", out Deliverable4Completion);
            double _Deliverable4Completion = 0;
            if (Deliverable4Completion.Equals(""))
            {
                _Deliverable4Completion = 0;
            }
            else
            {
                _Deliverable4Completion = Convert.ToDouble(Deliverable4Completion);
            }

            object Deliverable5Completion;
            test.TryGetValue("Deliverable5Completion", out Deliverable5Completion);
            double _Deliverable5Completion = 0;
            if (Deliverable5Completion.Equals(""))
            {
                _Deliverable5Completion = 0;
            }
            else
            {
                _Deliverable5Completion = Convert.ToDouble(Deliverable5Completion);
            }

            object Fyp2Deliverables;
            test.TryGetValue("Fyp2Deliverables", out Fyp2Deliverables);
            string _Fyp2Deliverables = Convert.ToString(Fyp2Deliverables);

            object LeaderID;
            test.TryGetValue("LeaderID", out LeaderID);
            int _LeaderID = Convert.ToInt32(LeaderID);

            object Member1ID;
            test.TryGetValue("Member1ID", out Member1ID);
            double _Member1ID = 0;
            if (Member1ID.Equals(""))
            {
                _Member1ID = 0;
            }
            else
            {
                _Member1ID = Convert.ToDouble(Member1ID);
            }

            object Member2ID;
            test.TryGetValue("Member2ID", out Member2ID);
            double _Member2ID = 0;
            if (Member2ID.Equals(""))
            {
                _Member2ID = 0;
            }
            else
            {
                _Member2ID = Convert.ToDouble(Member2ID);
            }

            object leaderMarks;
            test.TryGetValue("leaderMarks", out leaderMarks);
            double _leaderMarks = Convert.ToDouble(leaderMarks);

            object member1marks;
            test.TryGetValue("member1marks", out member1marks);
            double _member1marks = 0;
            if (member1marks.Equals(""))
            {
                _member1marks = 0;
            }
            else
            {
                _member1marks = Convert.ToDouble(member1marks);
            }

            object member2marks;
            test.TryGetValue("member2marks", out member2marks);
            double _member2marks = 0;
            if (member2marks.Equals(""))
            {
                _member2marks = 0;
            }
            else
            {
                _member2marks = Convert.ToDouble(member2marks);
            }

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {

                    var res = db.Query("FypEvaluation").InsertGetId<int>(new
                    {
                        FypID = _FypID,
                        FormID = _FormID,
                        Deliverable1Completion = _Deliverable1Completion,
                        Deliverable2Completion = _Deliverable2Completion,
                        Deliverable3Completion = _Deliverable3Completion,
                        Deliverable4Completion = _Deliverable4Completion,
                        Deliverable5Completion = _Deliverable5Completion,
                        Fyp2Deliverables = _Fyp2Deliverables,

                    });

                    db.Query("FypMarks").Insert(new
                    {
                        StudentID = _LeaderID,
                        FormID = 2,
                        Marks = _leaderMarks
                    });

                    db.Query("FypMarks").Insert(new
                    {
                        StudentID = _Member1ID,
                        FormID = 2,
                        Marks = _member1marks
                    });

                    db.Query("FypMarks").Insert(new
                    {
                        StudentID = _Member2ID,
                        FormID = 2,
                        Marks = _member2marks
                    });


                    scope.Complete();
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, object>() { { "LastInsertedId", res } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }
        }

    }
}