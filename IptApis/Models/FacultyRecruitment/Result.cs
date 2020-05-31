using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.FacultyRecruitment
{
    public class Result
    {
        public int ResultID { get; set; }
        public int AttemptedQuestions { get; set; }
        public int CorrectQuestions { get; set; }
        public int RefID { get; set; }
        public int ExamID { get; set; }
    }
}