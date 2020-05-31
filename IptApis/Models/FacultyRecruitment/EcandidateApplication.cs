using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.FacultyRecruitment
{
    public class EcandidateApplication
    {
        public int RefID { get; set; }
        public string QualifiedStatus { get; set; }
        public string ApplyDate { get; set; }

        public string TestDate { get; set; }
        public int EcandidateID { get; set; }
        public int JobID { get; set; }


    }
}