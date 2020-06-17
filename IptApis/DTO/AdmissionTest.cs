namespace IptApis.DTO
{
    public class AdmissionTestDto
    {
        public int TestId { get; set; }
        public System.DateTime TestDate { get; set; }
        public int TestOpeningId { get; set; }

        public AdmissionTestDto()
        { }

        public AdmissionTestDto(Models.Admission.AdmissionTest test)
        {
            (TestId, TestDate, TestOpeningId) =
                (test.TestId, test.TestDate, test.TestOpeningId);
        }

    }
}
