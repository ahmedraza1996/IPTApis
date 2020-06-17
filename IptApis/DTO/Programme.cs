namespace IptApis.DTO
{
    public class Programme
    {
        public int ProgrammeId { get; set; }
        public string ProgrammeName { get; set; } = "";
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public Programme() { }

        public Programme(Models.Admission.Programme programme)
        {
            (ProgrammeId, ProgrammeName, DepartmentId) =
                (programme.ProgrammeId, programme.ProgrammeName, programme.DepartmentId);
        }
    }
}
