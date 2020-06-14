using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models
{
    public class SemesterInfo
    {
        public int semesterID { get; set; }
        public string semesterName { get; set; }
        public string semesterType { get; set; }
        public int semesterStartDate { get; set; }
        public int semesterEndDate { get; set; }
        public int registrationStartDate { get; set; }
        public int registrationEndDate { get; set; }
        public int creditLimit { get; set; }
        public int feedbackStartDate { get; set; }
        public int feedbackEndDate { get; set; }
        public int registrationStatus { get; set; }
    }
}