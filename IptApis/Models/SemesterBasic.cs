using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models
{
    public class SemesterBasic
    {
    
        public string semesterName { get; set; }
        public string semesterType { get; set; }

        public SemesterBasic() { }

        public SemesterBasic(SemesterInfo semester)
        {
            this.semesterName = semester.semesterName;
            this.semesterType = semester.semesterType;
        }
    }
}