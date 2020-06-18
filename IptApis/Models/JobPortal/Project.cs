using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.JobPortal
{
    public class Project
    {
        public int StudentID { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string GithubLink { get; set; }
        public int courseOfferedID { get; set; }
        public string Status { get; set; }
        public string Fname { get; set; }
        public int FrameworkID { get; set; }

        public int DomainID { get; set; }
        public string DomainName { get; set; }

        public string Skillvalue { get; set; }
    }
}