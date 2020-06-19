using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.FacultyRecruitment
{
    public class UserRole
    {
        public int id { get; set; }

        public int UserId { get; set; }

        public string Role { get; set; }
    }
}