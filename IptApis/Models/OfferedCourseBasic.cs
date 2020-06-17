using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models
{
    public class OfferedCourseBasic
    {
        public int courseID { get; set; }
        public int creditHrs { get; set; }
        public int courseNSections { get; set; }
        public int maxStdPerSection { get; set; }
        public int batchID { get; set; }
        public int semesterID { get; set; }
    }
}