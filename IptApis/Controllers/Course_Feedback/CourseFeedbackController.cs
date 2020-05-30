using IptApis.Shared;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IptApis.CourseFeedbackModels;
using IptApis.Models.CouseFeedbackModels;

namespace IptApis.Controllers
{
    public class CourseFeedbackController : ApiController
    {
        [HttpGet]
        public string test()
        {
            return "Hello this is cafeteria";
        }

        [HttpGet]
        public Student getCurrentStudent(string studentID)
        {
            //Will return the current student from the session(Logged in)
            //Currently no session is setup i.e why taking student id in parameter
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();
            IEnumerable<IDictionary<string, object>> responseStudent;
            responseStudent = db.Query("Student").Where("RollNumber", studentID).Get().Cast<IDictionary<string, object>>();
            Student x = new Student();
            foreach (var res in responseStudent)
            {
                x.Name = res["SName"].ToString();
                x.Email = res["Email"].ToString();
                x.MobileNo = res["MobileNumber"].ToString();
                x.RollNo = res["RollNumber"].ToString();
                x.StudentID = res["StudentID"].ToString();
            }
            Debug.WriteLine("AAA");
            return x;
        }

        [HttpGet]
        public string getAllCourses()
        {
            //Will return the list of courses current student is enrolled in
            return "AAA";
        }

        [HttpGet]
        public List<Questions> getQuestions(string courseName, string courseType)
        {
            //Will return the list of questions/options based on course/course type

            List<Questions> questions = new List<Questions>();

            if (courseType == "1")
            {
                //CourseType is Theory
                var db = DbUtils.GetDBConnection();
                db.Connection.Open();
                IEnumerable<IDictionary<string, object>> responseQuestions;
                IEnumerable<IDictionary<string, object>> responseOptions;

                responseQuestions = db.Query("Question").Where("CourseType", "1").Get().Cast<IDictionary<string, object>>();
                responseOptions = db.Query("options").Where("QuestionID", "3").Get().Cast<IDictionary<string, object>>();
                
                foreach (var res in responseQuestions)
                {
                    Questions x=new Questions();
                    Debug.WriteLine(res);
                    x.QuestionText = res["Question"].ToString();
                    x.QuestionType = res["QuestionType"].ToString();
                    x.CourseType = res["CourseType"].ToString();
                    if (x.QuestionType=="2")
                    {
                       foreach(var z in responseOptions)
                        {
                            x.options.Add(z["Opt"].ToString());
                        }
                    }
                    questions.Add(x);
                }

                //Debug.(response);
                Debug.WriteLine("AAA");
                return questions;
            }
            else
            {
                //CourseType is Lab
                return questions;

            }
        }


        [HttpPost]
        public string postAnswers()
        {
            //Will accept the answers as post request and add them to DB
            return "Sucess";
        }

        [HttpGet]
        public HttpResponseMessage getQuestionAnswerPairs()
        {
            //LATER TASK
            //For admin portal to view all question and answer
            try
            {
                //return Request.CreateResponse(HttpStatusCode.Created, new Dictionary<string, object>() { { "LastInsertedId", res } });
                return Request.CreateResponse(HttpStatusCode.OK, "test");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);

            }

        }
    }
}
