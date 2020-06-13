namespace IptApis.Models.Admission
{
    public partial class Student
    {
        public int StudentId { get; set; }
        public string Sname { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string RollNumber { get; set; }
        public int CandidateId { get; set; }
        public int BatchId { get; set; }
        public string Spassword { get; set; }
    }
}
