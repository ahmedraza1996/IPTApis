using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.JobPortal
{
    public class CV
    {
        public int studentId { get; set; }
        public string name { get; set; }
        public string contentType { get; set; }
        public byte[] data { get; set; }
    }
}