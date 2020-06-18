using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.AdminPanel
{
    [Serializable]
    public class faculties
    {

        public faculties(string EmpName,string Email,string MobileNumber,string EPassword)
        {
            this.EmpName = EmpName;
            this.Email = Email;
            this.MobileNumber = MobileNumber;
            this.EPassword = EPassword;
        }


        //public int EmpID { get; set; }

        public string EmpName { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public string EPassword { get; set; }


    }
}