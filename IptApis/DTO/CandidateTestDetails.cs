namespace IptApis.DTO
{
    public class CandidateTestDetails
    {
        public int DetailsId { get; set; }
        public int AssignedTestId { get; set; }
        public string TestScore { get; set; }

        public CandidateTestDetails() { }

        public CandidateTestDetails(Models.Admission.CandidateTestDetails details)
        {
            (DetailsId, TestScore, AssignedTestId) =
                (details.DetailsId, details.TestScore, details.AssignedTestId);
        }
    }
}
