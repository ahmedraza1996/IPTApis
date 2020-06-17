using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.JobPortal
{
    public class JobDetails
    {
        public string Title { get; set; }
        public int JobID { get; set; }
        public string Organization { get; set; }
        public string LastApplyDate { get; set; }
        public string Designation { get; set; }
        public int MinExperience { get; set; }
        public string AttachmentPath { get; set; }
        public string ApplicationLink { get; set; }
        public string Contactperson { get; set; }
        public int DescID { get; set; }
        public string Description { get; set; }
        public List<string> DescriptionList { get; set; }
        //public string JobTagID { get; set; }
        //public string TagName { get; set; }
    }
}