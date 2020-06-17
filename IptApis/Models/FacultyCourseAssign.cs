using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models
{
    public class FacultyCourseAssign
    {
        public int courseOfferedID { get; set; }
        public int empID { get; set; }
        public int[] sectionID { get; set; }

        public FacultyCourseAssign()
        {
            this.sectionID =new int[] { };
        }
    }
}