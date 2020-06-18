using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models
{
    public class SemesterConfig
    {
        public int semesterStartDate { get; set; }
        public int semesterEndDate { get; set; }
        public int registrationStartDate { get; set; }
        public int registrationEndDate { get; set; }
        public int creditLimit { get; set; }
        public int feedbackStartDate { get; set; }
        public int feedbackEndDate { get; set; }
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