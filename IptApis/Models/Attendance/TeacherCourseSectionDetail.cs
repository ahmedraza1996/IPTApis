using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.Attendance
{
    public class TeacherCourseSectionDetail
    {
        public int FSID { get; set; }
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public int EmpID { get; set; }
        public string EmpName { get; set; }
        public int CourseID { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int CFID { get; set; }
        public int BatchID { get; set; }
        public string BatchName { get; set; }
        public int BatchYear { get; set; }
        public int SemesterID { get; set; }
        public string SemesterName { get; set; }
        public string SemesterType { get; set; }
    }
}