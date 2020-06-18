using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.Attendance
{
    public class CourseEnrollment
    {
        public int EnrollmentID { get; set; }
        public string CourseStatus { get; set; }
        public char IsFeedBackSubmit { get; set; }
        public int FSID { get; set; }
        public int StudentId { get; set; }
        public int CourseID { get; set; }
    }
}