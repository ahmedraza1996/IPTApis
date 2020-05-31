using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.FacultyRecruitment
{
    public class Employee
    {
        public int EmpID { get; set; }
        public string EmpName { get; set; }

        //public string password { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int DesignationID { get; set; }
        public int DepartmentID { get; set; }
        public int RefID { get; set; }
    }
}