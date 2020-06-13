using System;
using System.Collections.Generic;

namespace IptApis.Models.Admission
{
    public partial class Programme
    {
        public Programme()
        {
            //StudentOpening = new HashSet<StudentOpening>();
        }

        public int ProgrammeId { get; set; }
        public string ProgrammeName { get; set; }
        public int DepartmentId { get; set; }

        //public virtual Department Department { get; set; }
        //public virtual ICollection<StudentOpening> StudentOpening { get; set; }
    }
}
