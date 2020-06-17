using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models
{
    public class CourseRegistrationModel
    {
        public int courseOfferedID { get; set; }
        public int studentID { get; set; }
        public int sectionID { get; set; }
        public string courseStatus { get; set; }
        public int fsID { get; set; }
    }
}