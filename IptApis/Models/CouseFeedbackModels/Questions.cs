using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.CourseFeedbackModels
{
    public class Questions
    {
        public string QuestionID { get; set; }
        public string QuestionText { get; set; }
        public string CourseType { get; set; }
        public string QuestionType { get; set; }
        public List<String> options{ get;set; }

        public Questions(string _QuestionText,string _CourseType,string _QuestionType,string _QuestionID)
        {
            QuestionText = _QuestionText;
            CourseType = _CourseType;
            QuestionType = _QuestionType;
            options = new List<string>();
            QuestionID = _QuestionID;
        }
        public Questions()
        {
            options = new List<string>();
        }
    }
}
