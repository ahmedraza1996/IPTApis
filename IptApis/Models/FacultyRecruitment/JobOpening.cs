using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.FacultyRecruitment
{
    public class JobOpening
    {
        public int JobID { get; set; }
        public int MinExperience { get; set; }
        public string JobDescription { get; set; }
        public string DatePosted { get; set; }
        public string ExpectedStartDate { get; set; }
        public int DesignationID { get; set; }
    }
}