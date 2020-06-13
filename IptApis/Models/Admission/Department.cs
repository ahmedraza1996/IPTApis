using System;
using System.Collections.Generic;

namespace IptApis.Models.Admission
{
    public partial class Department
    {
        public Department()
        {
            //Programme = new HashSet<Programme>();
        }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        //public virtual ICollection<Programme> Programme { get; set; }
    }
}
