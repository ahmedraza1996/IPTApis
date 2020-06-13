using System;
using System.Collections.Generic;

namespace IptApis.Models.Admission
{
    public partial class Batch
    {
        public Batch()
        {
            //Student = new HashSet<Student>();
        }

        public int BatchId { get; set; }
        public string BatchName { get; set; }
        public int BatchYear { get; set; }

        //public virtual ICollection<Student> Student { get; set; }
    }
}
