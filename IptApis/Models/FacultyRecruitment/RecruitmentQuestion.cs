using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.FacultyRecruitment
{
    public class RecruitmentQuestion
    {
        public int QuestionID { get; set; }
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public int ExamID { get; set; }

    }
}