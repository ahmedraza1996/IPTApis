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
        public string semesterStartDate { get; set; }
        public string semesterEndDate { get; set; }
        public string registrationStartDate { get; set; }
        public string registrationEndDate { get; set; }
        public int creditLimit { get; set; }
        public string feedbackStartDate { get; set; }
        public string feedbackEndDate { get; set; }
    }
}