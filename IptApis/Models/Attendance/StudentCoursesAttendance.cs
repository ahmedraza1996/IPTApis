using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.Attendance
{
    public class StudentCoursesAttendance
    {
        public int AttendanceID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public int ClassDuration { get; set; }
        public string AttendanceStatus { get; set; }
        public int StudentID { get; set; }   
        public string RollNumber { get; set; }
        public string SName { get; set; }
        public int CourseID { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }


    }
}