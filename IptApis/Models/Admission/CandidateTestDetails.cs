using System;
using System.Collections.Generic;

namespace IptApis.Models.Admission
{
    public partial class CandidateTestDetails
    {
        public int DetailsId { get; set; }
        public int CandidateId { get; set; }
        public int AssignedTestId { get; set; }
        public string TestScore { get; set; }

        //public virtual AdmissionTest AssignedTest { get; set; }
        //public virtual CandidateStudent Candidate { get; set; }
    }
}
