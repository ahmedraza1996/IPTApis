using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.MarksModels
{
    public class Update_Distribution
    {
        public int MDID;
        public int FSID { get; set; }
        public double Total_marks { get; set; }
        public double weightage { get; set; }
    }
}