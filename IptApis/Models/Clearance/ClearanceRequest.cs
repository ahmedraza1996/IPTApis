using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.Clearance
{
    public class ClearanceRequest
    {
        object parentName1 { get; set; }
        object parentName2 { get; set; }
        DateTime RequestDate { get; set; }
        object parentCNIC1 { get; set; }
        object parentCNIC2 { get; set; }
        object RequestID { get; set; }
        object SemesterGraduation { get; set; }
        object StudentID{get; set;}

    }
}