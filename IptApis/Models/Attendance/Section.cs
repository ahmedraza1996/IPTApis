using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.Attendance
{
    public class Section
    {
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public int BatchID { get; set; }
        public int FSID { get; internal set; }
    }
}