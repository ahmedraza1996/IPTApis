using System;
using System.Collections.Generic;

namespace IptApis.Models.Admission
{
    public partial class ScandidateApplication
    {
        public int RefId { get; set; }
        public string QualifiedStatus { get; set; }
        public DateTime ApplyDate { get; set; }
        public int CandidateId { get; set; }
        public int SopeningId { get; set; }

        //public virtual CandidateStudent Candidate { get; set; }
        //public virtual StudentOpening Sopening { get; set; }
    }
}
