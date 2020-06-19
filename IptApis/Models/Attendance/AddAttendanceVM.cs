﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.Attendance
{
    public class AddAttendanceVM
    {
        public List<Student> students { get; set; }
        public List<Attend> attendances { get; set; }

        public string EmpName { get; set; }
        public string CourseID { get; set; }

        public string SectionName { get; set; }

        public DateTime date { get; set; }

        public DateTime AttendanceDate { get; set; }
        public DateTime oldAttendanceDate { get; set; }
        public string AttendanceStatus { get; set; }
        public int ClassDuration { get; set; }

        public int EnrollmentID { get; set; }
    }
}