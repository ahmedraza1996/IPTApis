﻿using System;
using System.Collections.Generic;

namespace IptApis.Models.Admission
{
    public partial class CandidateStudent
    {
        public int CandidateId { get; set; }
        public string Cname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Caddress { get; set; }
        public string ContactInfo { get; set; }
        public string Hscresult { get; set; }
        public string Sscresult { get; set; }
    }
}
