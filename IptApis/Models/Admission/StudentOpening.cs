using System;
using System.Collections.Generic;

namespace IptApis.Models.Admission
{
    public partial class StudentOpening
    {
        public StudentOpening()
        {
            //ScandidateApplication = new HashSet<ScandidateApplication>();
        }

        public int SopeningId { get; set; }
        public int Syear { get; set; }
        public int MaxSeats { get; set; }
        public DateTime LastApplicationDate { get; set; }
        public int ProgrammeId { get; set; }

        //public virtual Programme Programme { get; set; }
        //public virtual ICollection<ScandidateApplication> ScandidateApplication { get; set; }
    }
}
