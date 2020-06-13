namespace IptApis.DTO
{
    public class StudentOpening
    {
        public int OpeningId { get; set; }
        public int Syear { get; set; }
        public int MaxSeats { get; set; }
        public System.DateTime LastApplicationDate { get; set; }
        public int ProgrammeId { get; set; }

        public StudentOpening() { }

        public StudentOpening(Models.Admission.StudentOpening opening)
        {
            (OpeningId, Syear, MaxSeats, LastApplicationDate, ProgrammeId) =
                (opening.SopeningId, opening.Syear, opening.MaxSeats, opening.LastApplicationDate, opening.ProgrammeId);
        }
    }
}
