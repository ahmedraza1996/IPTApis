using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.JobPortal
{
    public class Skill
    {
        public int RefId { get; set; }
        public int StudentID { get; set; }
        public string SkillName { get; set; }
        public string DomainName { get; set; }
    }
}