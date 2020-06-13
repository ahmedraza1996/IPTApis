using System;
using System.Collections.Generic;

namespace IptApis.Models.Admission
{
    public partial class CandidateStudent
    {
        public CandidateStudent()
        {
            //CandidateTestDetails = new HashSet<CandidateTestDetails>();
            //ScandidateApplication = new HashSet<ScandidateApplication>();
            //Student = new HashSet<Student>();
        }

        public int CandidateId { get; set; }
        public string Cname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Caddress { get; set; }
        public string ContactInfo { get; set; }
        public string Hscresult { get; set; }
        public string Sscresult { get; set; }

        //public virtual ICollection<CandidateTestDetails> CandidateTestDetails { get; set; }
        //public virtual ICollection<ScandidateApplication> ScandidateApplication { get; set; }
        //public virtual ICollection<Student> Student { get; set; }
    }
}
