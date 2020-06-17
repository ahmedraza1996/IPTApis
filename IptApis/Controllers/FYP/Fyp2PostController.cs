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
    public class Fyp2PostController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage AddMidEvaluationJury(Object MidEvaluation)
        {

            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(MidEvaluation));

            object FypID;
            test.TryGetValue("FypID", out FypID);
            int _FypID = Convert.ToInt32(FypID);

            object FormID;
            test.TryGetValue("FormID", out FormID);
            int _FormID = Convert.ToInt32(FormID);

            object ProjectProgress;
            test.TryGetValue("ProjectProgress", out ProjectProgress);
            int _ProjectProgress = Convert.ToInt32(ProjectProgress);

            object DocumentationStatus;
            test.TryGetValue("DocumentationStatus", out DocumentationStatus);
            int _DocumentationStatus = Convert.ToInt32(DocumentationStatus);

            object ProgressComments;
            test.TryGetValue("ProgressComments", out ProgressComments);
            string _ProgressComments = Convert.ToString(ProgressComments);

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

                        ProjectProgress = _ProjectProgress,
                        DocumentationStatus = _DocumentationStatus,
                        ProgressComments = _ProgressComments
                    });

                    var totalMarks = _ProjectProgress + _DocumentationStatus;

                    db.Query("FypMarks").Insert(new
                    {
                        StudentID = _LeaderID,
                        FormID = 3,
                        Marks = totalMarks
                    });

                    db.Query("FypMarks").Insert(new
                    {
                        StudentID = _Member1ID,
                        FormID = 3,
                        Marks = totalMarks
                    });

                    db.Query("FypMarks").Insert(new
                    {
                        StudentID = _Member2ID,
                        FormID = 3,
                        Marks = totalMarks
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
        public HttpResponseMessage AddFinalEvaluationJury(Object FinalEvaluation)
        {

            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(FinalEvaluation));

            object FypID;
            test.TryGetValue("FypID", out FypID);
            int _FypID = Convert.ToInt32(FypID);

            object FormID;
            test.TryGetValue("FormID", out FormID);
            int _FormID = Convert.ToInt32(FormID);

            object LeaderID;
            test.TryGetValue("LeaderID", out LeaderID);
            int _LeaderID = Convert.ToInt32(LeaderID);

            object Member1ID;
            test.TryGetValue("Member1ID", out Member1ID);
            int _Member1ID = Convert.ToInt32(Member1ID);

            object Member2ID;
            test.TryGetValue("Member2ID", out Member2ID);
            int _Member2ID = Convert.ToInt32(Member2ID);

            object leaderMarks;
            test.TryGetValue("leaderMarks", out leaderMarks);
            double _leaderMarks = Convert.ToDouble(leaderMarks);

            object member1marks;
            test.TryGetValue("member1marks", out member1marks);
            double _member1marks = Convert.ToDouble(member1marks);

            object member2marks;
            test.TryGetValue("member2marks", out member2marks);
            double _member2marks = Convert.ToDouble(member2marks);

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

                    });

                    db.Query("FypMarks").Insert(new
                    {
                        StudentID = _LeaderID,
                        FormID = _FormID,
                        Marks = _leaderMarks
                    });

                    db.Query("FypMarks").Insert(new
                    {
                        StudentID = _Member1ID,
                        FormID = _FormID,
                        Marks = _member1marks
                    });

                    db.Query("FypMarks").Insert(new
                    {
                        StudentID = _Member2ID,
                        FormID = _FormID,
                        Marks = _member2marks
                    });


                    scope.Complete();
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, object>() { { "Marks Updated", 0 } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }
        }

        [HttpPost]
        public HttpResponseMessage RegisterJury(Object JuryDetails)
        {

            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(JuryDetails));

            object Username;
            test.TryGetValue("Username", out Username);
            string _Username = Convert.ToString(Username);

            object Name;
            test.TryGetValue("Name", out Name);
            string _Name = Convert.ToString(Name);

            object Password;
            test.TryGetValue("Password", out Password);
            string _Password = Convert.ToString(Password);


            var db = DbUtils.GetDBConnection();
            db.Connection.Open();


            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    db.Query("FypExternalJuryy").Insert(new
                    {
                        Username = _Username,
                        Name = _Name,
                        Password = _Password,

                    });


                    scope.Complete();
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, object>() { { "External Jury added", 0 } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }
        }

        [HttpPost]
        public HttpResponseMessage AddFyp2FinalEvaluationJury(Object FinalEvaluation)
        {

            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(FinalEvaluation));

            object FypID;
            test.TryGetValue("FypID", out FypID);
            int _FypID = Convert.ToInt32(FypID);

            object FormID;
            test.TryGetValue("FormID", out FormID);
            int _FormID = Convert.ToInt32(FormID);

            object LeaderID;
            test.TryGetValue("LeaderID", out LeaderID);
            int _LeaderID = Convert.ToInt32(LeaderID);

            object Member1ID;
            test.TryGetValue("Member1ID", out Member1ID);
            int _Member1ID = Convert.ToInt32(Member1ID);

            object Member2ID;
            test.TryGetValue("Member2ID", out Member2ID);
            int _Member2ID = Convert.ToInt32(Member2ID);

            object leaderMarks;
            test.TryGetValue("leaderMarks", out leaderMarks);
            double _leaderMarks = Convert.ToDouble(leaderMarks);

            object member1marks;
            test.TryGetValue("member1marks", out member1marks);
            double _member1marks = Convert.ToDouble(member1marks);

            object member2marks;
            test.TryGetValue("member2marks", out member2marks);
            double _member2marks = Convert.ToDouble(member2marks);

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();


            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    db.Query("FypEvaluation").Insert(new
                    {
                        FypID = _FypID,
                        FormID = _FormID
                    });
                    db.Query("FypMarks").Insert(new
                    {
                        StudentID = _LeaderID,
                        FormID = _FormID,
                        Marks = _leaderMarks
                    });

                    db.Query("FypMarks").Insert(new
                    {
                        StudentID = _Member1ID,
                        FormID = _FormID,
                        Marks = _member1marks
                    });

                    db.Query("FypMarks").Insert(new
                    {
                        StudentID = _Member2ID,
                        FormID = _FormID,
                        Marks = _member2marks
                    });


                    scope.Complete();
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, object>() { { "Marks Updated", 0 } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

            }
        }

        [HttpPost]
        public HttpResponseMessage AddFyp2ExternalEvaluationJury(Object FinalEvaluation)
        {

            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(FinalEvaluation));

            object FypID;
            test.TryGetValue("FypID", out FypID);
            int _FypID = Convert.ToInt32(FypID);

            object FormID;
            test.TryGetValue("FormID", out FormID);
            int _FormID = Convert.ToInt32(FormID);

            object LeaderID;
            test.TryGetValue("LeaderID", out LeaderID);
            int _LeaderID = Convert.ToInt32(LeaderID);

            object Member1ID;
            test.TryGetValue("Member1ID", out Member1ID);
            int _Member1ID = Convert.ToInt32(Member1ID);

            object Member2ID;
            test.TryGetValue("Member2ID", out Member2ID);
            int _Member2ID = Convert.ToInt32(Member2ID);

            object leaderMarks;
            test.TryGetValue("leaderMarks", out leaderMarks);
            double _leaderMarks = Convert.ToDouble(leaderMarks);

            object member1marks;
            test.TryGetValue("member1marks", out member1marks);
            double _member1marks = Convert.ToDouble(member1marks);

            object member2marks;
            test.TryGetValue("member2marks", out member2marks);
            double _member2marks = Convert.ToDouble(member2marks);

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();


            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    db.Query("FypEvaluation").Insert(new
                    {
                        FypID = _FypID,
                        FormID = _FormID
                    });
                    db.Query("FypMarks").Insert(new
                    {
                        StudentID = _LeaderID,
                        FormID = _FormID,
                        Marks = _leaderMarks
                    });

                    db.Query("FypMarks").Insert(new
                    {
                        StudentID = _Member1ID,
                        FormID = _FormID,
                        Marks = _member1marks
                    });

                    db.Query("FypMarks").Insert(new
                    {
                        StudentID = _Member2ID,
                        FormID = _FormID,
                        Marks = _member2marks
                    });


                    scope.Complete();
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, object>() { { "Marks Updated", 0 } });
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