using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.Attendance
{
    public class AllStudentCourses
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string CourseStatus { get; set; }
        public int StudentID { get; set; }
        public string SName { get; set; }
        public string Email { get; set; }

    }
}