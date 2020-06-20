using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.Attendance
{
    public class DeleteAttendanceVM
    {
        public List<Student> students { get; set; }
        public List<Attend> attendances { get; set; }

        public string EmpName { get; set; }
        public string CourseID { get; set; }

        public string SectionName { get; set; }

        public DateTime AttendanceDate { get; set; }
        public string AttendanceStatus { get; set; }
        public int ClassDuration { get; set; }

        public int EnrollmentID { get; set; }
    }
}