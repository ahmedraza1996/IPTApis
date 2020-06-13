using System;
using System.Collections.Generic;

namespace IptApis.Models.Admission
{
    public partial class AdmissionTest
    {
        public AdmissionTest()
        {
            //CandidateTestDetails = new HashSet<CandidateTestDetails>();
        }

        public int TestId { get; set; }
        public DateTime TestDate { get; set; }
        public int TestOpeningId { get; set; }

        //public virtual ICollection<CandidateTestDetails> CandidateTestDetails { get; set; }
    }
}
