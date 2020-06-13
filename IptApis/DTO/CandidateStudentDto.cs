namespace IptApis.DTO
{
    public class CandidateStudentDto
    {
        public int CandidateId { get; set; }
        public string Cname { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Caddress { get; set; }
        public string ContactInfo { get; set; }
        public string Hscresult { get; set; }
        public string Sscresult { get; set; }
        public CandidateTestDetails TestDetails { get; set; }
        public CandidateApplication Application { get; set; }

        public CandidateStudentDto() { }

        public CandidateStudentDto(Models.Admission.CandidateStudent s)
        {
            (CandidateId, Cname, Email, Password, Caddress, ContactInfo, Hscresult, Sscresult) =
                (s.CandidateId, s.Cname, s.Email, s.Password, s.Caddress, s.ContactInfo, s.Hscresult, s.Sscresult);
        }
    }
}
