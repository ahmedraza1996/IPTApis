using IptApis.Models.FacultyRecruitment;
using IptApis.Shared;
using Newtonsoft.Json;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;

namespace IptApis.Controllers.FacultyRecruitment
{
    public class RecruitmentExamController : ApiController
    {
        // new controllers 
        // candidate application 
        //Get All EcandidateApplication

        [HttpGet]
        public HttpResponseMessage GetAllRecruitmentExams()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<RecruitmentExam> response = db.Query("RecruitmentExam").Get<RecruitmentExam>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetRecruitmentExamById(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<JobOpening> response = db.Query("RecruitmentExam").Where("ExamId", id).Get<JobOpening>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        public HttpResponseMessage AddRecruitmentExam(Object job)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(job));

            object Degree;
            test.TryGetValue("Degree", out Degree);
            string _Degree = Convert.ToString(Degree);

            object JobID;
            test.TryGetValue("JobID ", out JobID);
            int _JobID = Convert.ToInt32(JobID);





            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var id = db.Query("RecruitmentExam").InsertGetId<int>(new
                    {
                        Degree = _Degree,
                        JobID = _JobID,

                    });
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, id);//, new Dictionary<string, object>() { { "LastInsertedId", res } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();   //if there are any error, rollback the transaction
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }


        [HttpDelete]
        public HttpResponseMessage DeleteRecruitmentExam(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            try
            {
                _ = db.Query("Choice").Join("RecruitmentQuestion", "Choice.QuestionID", "RecruitmentQuestion.QuestionID").Where("RecruitmentQuestion.ExamID", "=", id).Delete();
                _ = db.Query("RecruitmentQuestion").Where("ExamID", "=", id).Delete();
                _ = db.Query("Result").Where("ExamId", "=", id).Delete();
                _ = db.Query("RecruitmentExam").Where("ExamId", "=", id).Delete();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateRecruitmentExam(Object job)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(job));

            object __ExamId;
            test.TryGetValue("ExamId", out __ExamId);
            int _ExamId = Convert.ToInt32(__ExamId);


            object __Degree;
            test.TryGetValue("MinExperience", out __Degree);
            string _Degree = Convert.ToString(__Degree);

            object __JobID;
            test.TryGetValue("JobDescription", out __JobID);
            int _JobID = Convert.ToInt32(__JobID);


            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {

                    var affected = db.Query("RecruitmentExam").Where("ExamId", _ExamId).Update(new
                    {
                        _Degree = __Degree,
                        JobID = _JobID,

                    });
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception e)
                {
                    scope.Dispose();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                }
            }
        }

        // new controllers 
        // RecruitmentQuestion
        //Get All RecruitmentQuestion

        [HttpGet]
        public HttpResponseMessage GetRecruitmentQuestion()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<RecruitmentQuestion> response = db.Query("RecruitmentQuestion").Get<RecruitmentQuestion>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetRecruitmentQuestionById(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<RecruitmentQuestion> response = db.Query("RecruitmentQuestion").Where("QuestionID", id).Get<RecruitmentQuestion>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        public HttpResponseMessage AddRecruitmentQuestion(Object job)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(job));

            object QuestionText;
            test.TryGetValue("QuestionText", out QuestionText);
            string _QuestionText = Convert.ToString(QuestionText);

            object CorrectAnswer;
            test.TryGetValue("CorrectAnswer", out CorrectAnswer);
            string _CorrectAnswer = Convert.ToString(CorrectAnswer);

            object ExamID;
            test.TryGetValue("DatePosted", out ExamID);
            int _ExamID = Convert.ToInt32(ExamID);


            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var id = db.Query("RecruitmentQuestion").InsertGetId<int>(new
                    {
                        QuestionText = _QuestionText,
                        CorrectAnswer = _CorrectAnswer,
                        ExamID = _ExamID,

                    });
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, id);//, new Dictionary<string, object>() { { "LastInsertedId", res } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();   //if there are any error, rollback the transaction
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteRecruitmentQuestion(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            try
            {
                _ = db.Query("RecruitmentQuestion").Where("QuestionID", "=", id).Delete();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateRecruitmentQuestion(Object job)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(job));

            object __QuestionID;
            test.TryGetValue("QuestionID", out __QuestionID);
            int _QuestionID = Convert.ToInt32(__QuestionID);


            object __QuestionText;
            test.TryGetValue("QuestionText", out __QuestionText);
            string _QuestionText = Convert.ToString(__QuestionText);

            object __CorrectAnswer;
            test.TryGetValue("CorrectAnswer", out __CorrectAnswer);
            string _CorrectAnswer = Convert.ToString(__CorrectAnswer);



            object __ExamID;
            test.TryGetValue("ExamID", out __ExamID);
            int _ExamID = Convert.ToInt32(__ExamID);


            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {

                    var affected = db.Query("RecruitmentQuestion").Where("QuestionID", _QuestionID).Update(new
                    {
                        QuestionText = _QuestionText,
                        CorrectAnswer = _CorrectAnswer,
                        ExamID = _ExamID,

                    });
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception e)
                {
                    scope.Dispose();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                }
            }
        }

        // new controllers 
        // candidate results 
        //Get All Result

        [HttpGet]
        public HttpResponseMessage GetResult()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<Result> response = db.Query("Result").Get<Result>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetResultById(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<Result> response = db.Query("Result").Where("ResultID", id).Get<Result>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [HttpPost]
        public HttpResponseMessage AddResult(Object job)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(job));

            object AttemptedQuestions;
            test.TryGetValue("AttemptedQuestions", out AttemptedQuestions);
            int _AttemptedQuestions = Convert.ToInt32(AttemptedQuestions);


            object CorrectQuestions;
            test.TryGetValue("CorrectQuestions", out CorrectQuestions);
            int _CorrectQuestions = Convert.ToInt32(CorrectQuestions);


            object RefID;
            test.TryGetValue("RefID", out RefID);
            int _RefID = Convert.ToInt32(RefID);


            object ExamID;
            test.TryGetValue("ExamID", out ExamID);
            int _ExamID = Convert.ToInt32(ExamID);

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var id = db.Query("Result").InsertGetId<int>(new
                    {
                        AttemptedQuestions = _AttemptedQuestions,
                        CorrectQuestions = _CorrectQuestions,
                        RefID = _RefID,
                        ExamID = _ExamID,

                    });
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, id);//, new Dictionary<string, object>() { { "LastInsertedId", res } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();   //if there are any error, rollback the transaction
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteResult(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            try
            {
                _ = db.Query("Result").Where("ResultID", "=", id).Delete();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateResult(Object job)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(job));

            object __ResultID;
            test.TryGetValue("ResultID", out __ResultID);
            int _ResultID = Convert.ToInt32(__ResultID);


            object __AttemptedQuestions;
            test.TryGetValue("AttemptedQuestions", out __AttemptedQuestions);
            int _AttemptedQuestions = Convert.ToInt32(__AttemptedQuestions);

            object __CorrectQuestions;
            test.TryGetValue("CorrectQuestions", out __CorrectQuestions);
            int _CorrectQuestions = Convert.ToInt32(__CorrectQuestions);

            object __RefID;
            test.TryGetValue("RefID", out __RefID);
            int _RefID = Convert.ToInt32(__RefID);

            object __ExamID;
            test.TryGetValue("ExamID", out __ExamID);
            int _ExamID = Convert.ToInt32(__ExamID);

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {

                    var affected = db.Query("Result").Where("ResultID", _ResultID).Update(new
                    {
                        AttemptedQuestions = _AttemptedQuestions,
                        CorrectQuestions = _CorrectQuestions,
                        RefID = _RefID,
                        ExamID = _ExamID,

                    });
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception e)
                {
                    scope.Dispose();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                }
            }
        }


        // new controllers 
        // candidate Choice
        //Get All Choice


        [HttpGet]
        public HttpResponseMessage GetChoice()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<Choice> response = db.Query("Choice").Get<Choice>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        public HttpResponseMessage GetChoiceById(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();//3870
            IEnumerable<Choice> response = db.Query("Choice").Where("QuestionID", id).Get<Choice>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        public HttpResponseMessage AddChoice(Object job)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(job));



            object Choice1;
            test.TryGetValue("Choice1", out Choice1);
            string _Choice1 = Convert.ToString(Choice1);


            object Choice2;
            test.TryGetValue("Choice2", out Choice2);
            string _Choice2 = Convert.ToString(Choice2);

            object Choice3;
            test.TryGetValue("Choice3", out Choice3);
            string _Choice3 = Convert.ToString(Choice3);

            object Choice4;
            test.TryGetValue("Choice4", out Choice4);
            string _Choice4 = Convert.ToString(Choice4);



            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var id = db.Query("Choice").InsertGetId<int>(new
                    {
                        Choice1 = _Choice1,
                        Choice2 = _Choice2,
                        Choice3 = _Choice3,
                        Choice4 = _Choice4,

                    });
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created, id);//, new Dictionary<string, object>() { { "LastInsertedId", res } });
                }
                catch (Exception ex)
                {
                    scope.Dispose();   //if there are any error, rollback the transaction
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteChoice(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            try
            {
                _ = db.Query("Choice").Where("QuestionID", "=", id).Delete();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateChoice(Object job)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(job));

            object __QuestionID;
            test.TryGetValue("QuestionID", out __QuestionID);
            int _QuestionID = Convert.ToInt32(__QuestionID);


            object __Choice1;
            test.TryGetValue("Choice1", out __Choice1);
            int _Choice1 = Convert.ToInt32(__Choice1);

            object __Choice2;
            test.TryGetValue("Choice2", out __Choice2);
            int _Choice2 = Convert.ToInt32(__Choice2);

            object __Choice3;
            test.TryGetValue("Choice3", out __Choice3);
            int _Choice3 = Convert.ToInt32(__Choice3);

            object __Choice4;
            test.TryGetValue("Choice4", out __Choice4);
            int _Choice4 = Convert.ToInt32(__Choice4);




            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {

                    var affected = db.Query("Choice").Where("QuestionID", _QuestionID).Update(new
                    {
                        Choice1 = _Choice1,
                        Choice2 = _Choice2,
                        Choice3 = _Choice3,
                        Choice4 = _Choice4,

                    });
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception e)
                {
                    scope.Dispose();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
                }
            }
        }

    }
}
