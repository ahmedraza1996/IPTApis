using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.CourseFeedbackModels
{
    public class Questions
    {
        public string QuestionText { get; set; }
        public string CourseType { get; set; }
        public string QuestionType { get; set; }
        public List<String> options{ get;set; }

        public Questions(string _QuestionText,string _CourseType,string _QuestionType)
        {
            QuestionText = _QuestionText;
            CourseType = _CourseType;
            QuestionType = _QuestionType;
            options = new List<string>();
        }
        public Questions()
        {
            options = new List<string>();
        }
    }
}
