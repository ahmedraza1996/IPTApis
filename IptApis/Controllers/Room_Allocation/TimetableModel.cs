using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Controllers.Room_Allocation
{
    public class TimetableModel
    {
        public String coursename { get; set; }
        public String Section { get; set; }
        public String CourseInstructor { get; set; }
        public String Room { get; set; }
        public int timeslot { get; set; }
        public String Batch { get; set; }
        public int credithour { get; set; }
        public int day { get; set; }

    }
}