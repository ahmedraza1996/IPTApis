using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.Attendance
{
    public class CourseOffered
    {

        public int CourseOfferedID { get; set; }
        public int CreditHrs { get; set; }
        public int CourseNSections { get; set; }
        public int MaxStdPerSection { get; set; }
        public int BatchID { get; set; }
        public int SemesterID { get; set; }
        public int CourseID { get; set; }
    }
}