using System;
using System.Collections.Generic;

namespace IptApis.Models.Admission
{
    public partial class AdmissionTest
    {
        public int TestId { get; set; }
        public DateTime TestDate { get; set; }
        public int TestOpeningId { get; set; }
    }
}
