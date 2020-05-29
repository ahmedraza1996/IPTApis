using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.MarksModels
{
    public class Student_Details
    {
        public Student_Details()
        {
            DL= new List<Dist_list>();
        }
        public String CourseName { get; set; }
        public int StudentID { get; set; }
        public String StudentName { get; set; }
        public String Email { get; set; }
        public List<Dist_list> DL { get; set; }
    }
    public class Dist_list
    {
        public static int assign, quiz, pro, lab, pres, fyp;
        public int MDID { get; set; }
        public String title { get; set; }
        public double currentmarks { get; set; }
        public double weightage { get; set; }
        public double totalmarks { get; set; }
        public void title_setter(int data)
        {
            String temp = data.ToString();
            if (temp == "1") { this.title = "Assignment-" + (assign + 1); assign++; }
            else if (temp == "2") { this.title = "Quiz-" + (quiz + 1); quiz++; }
            else if (temp == "3") { this.title = "Project-" + (pro + 1); pro++; }
            else if (temp == "4") { this.title = "Lab-" + (lab + 1); lab++; }
            else if (temp == "5") { this.title = "Presentation-" + (pres + 1); pres++; }
            else if (temp == "6") { this.title = "FYP-" + (fyp + 1); fyp++; }

        }

    }
}