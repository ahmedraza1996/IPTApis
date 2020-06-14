using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace IptApis.Models
{
    public class OfferedCourse:OfferedCourseBasic
    {
        public string courseName { get; set; }
        public string courseCode { get; set; }
        public int courseOfferedID { get; set; }        
        public int studentID { get; set; }
        public string courseStatus { get; set; }
        public int enrollmentID { get; set; }

    }
}