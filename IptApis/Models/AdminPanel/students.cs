using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.Models.AdminPanel
{
    public class students
    {

        public students(string SName, string Email, string MobileNumber,string RollNumber, string SPassword)
        {
            this.SName = SName;
            this.Email = Email;
            this.MobileNumber = MobileNumber;
            this.RollNumber = RollNumber;
            this.SPassword = SPassword;
        }


        //public int StudentID { get; set; }

        public string SName { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public string RollNumber { get; set; }

        public string SPassword { get; set; }



    }
}