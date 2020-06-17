using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace IptApis.Models
{
    public class FacultySectionDetails
    {
        public int fsID { get; set; }
        public int sectionID { get; set; }
        public string sectionName { get; set; }
        public int empID { get; set; }
        public string empName { get; set; }
        public int courseID { get; set; }
        public int cfID { get; set; }
    }
}