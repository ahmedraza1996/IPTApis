namespace IptApis.DTO
{
    public class CandidateApplication
    {
        public int RefId { get; set; }
        public string QualifiedStatus { get; set; } = "";
        public System.DateTime ApplyDate { get; set; }
        public int OpeningId { get; set; }

        public CandidateApplication() { }

        public CandidateApplication(Models.Admission.ScandidateApplication application)
        {
            (RefId, QualifiedStatus, ApplyDate, OpeningId) =
                (application.RefId, application.QualifiedStatus, application.ApplyDate, application.SopeningId);
        }
    }
}
