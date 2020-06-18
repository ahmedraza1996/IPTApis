using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.Attendance
{
    public class AddAttendanceVM
    {
        public List<Student> students { get; set; }

        public string EmpName { get; set; }
        public string CourseID { get; set; }

        public string SectionName { get; set; }

        public DateTime date { get; set; }
    }
}