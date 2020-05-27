using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models
{
    public class SemesterConfig
    {
        public string semesterStartDate { get; set; }
        public string semesterEndDate { get; set; }
        public string registrationStartDate { get; set; }
        public string registrationEndDate { get; set; }
        public int creditLimit { get; set; }
        public string feedbackStartDate { get; set; }
        public string feedbackEndDate { get; set; }
        public int semesterID { get; set; }

        public SemesterConfig() { }

        public SemesterConfig(SemesterInfo semester)
        {
            this.semesterStartDate = semester.semesterStartDate;
            this.semesterEndDate = semester.semesterEndDate;
            this.registrationStartDate = semester.registrationStartDate;
            this.registrationEndDate = semester.registrationEndDate;
            this.creditLimit = semester.creditLimit;
            this.semesterID = semester.semesterID;
        }
    }
}