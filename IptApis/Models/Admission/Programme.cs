using System;
using System.Collections.Generic;

namespace IptApis.Models.Admission
{
    public partial class Programme
    {
        public int ProgrammeId { get; set; }
        public string ProgrammeName { get; set; }
        public int DepartmentId { get; set; }
    }
}
