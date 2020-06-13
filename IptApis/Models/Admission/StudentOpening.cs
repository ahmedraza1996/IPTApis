using System;
using System.Collections.Generic;

namespace IptApis.Models.Admission
{
    public partial class StudentOpening
    {
        public int SopeningId { get; set; }
        public int Syear { get; set; }
        public int MaxSeats { get; set; }
        public DateTime LastApplicationDate { get; set; }
        public int ProgrammeId { get; set; }
    }
}
