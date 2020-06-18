using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.FacultyRecruitment
{
    public class CandidateEmployee
    {
        public int ECandidateID { get; set; }
        public string EName { get; set; }
        public string Email { get; set; }
        public string Epassword { get; set; }
        public string EAddress { get; set; }
        public string MobileNumber { get; set; }
        public string EducationalLevel { get; set; }
        public int ExperienceYears { get; set; }
        public string CVPath { get; set; }
        public string CoverLetterPath { get; set; }

    }
}