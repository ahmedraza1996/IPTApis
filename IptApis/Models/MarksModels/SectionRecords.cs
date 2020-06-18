using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.MarksModels
{
    public class SectionRecords
    {
        public static int assign, quiz, pro, lab, pres, fyp;
        public SectionRecords()
        {
            SRs = new List<StudentRecords>();
        }
        /*
         1-Assignment
        2-Quiz
        3-Project
        4-Lab
        5-Presentation
        6-FYP
        */
        public double max { get; set; }
        public double min { get; set; }
        public double average { get; set; }
        public int MDID { get; set; }
        public double weightage { get; set; }
        public double totalmarks { get; set; }
        public String title;
        public void title_setter(int data)
        {
            String temp = data.ToString();
            if (temp == "1") { this.title = "Assignment-" + (assign+1); assign++; }
            else if (temp == "2") { this.title = "Quiz-" + (quiz+1); quiz++; }
            else if (temp == "3") { this.title = "Project-" + (pro+1); pro++; }
            else if (temp == "4") { this.title = "Lab-" + (lab+1); lab++; }
            else if (temp == "5") { this.title = "Presentation-" + (pres+1); pres++; }
            else if (temp == "6") { this.title = "FYP-" + (fyp+1); fyp++; }

        }
        public String title_getter()
        {
            return this.title;
        }
        public List<StudentRecords> SRs;
    }
    public class StudentRecords
    {
        public int ID;
        public string Name;
        public double obtained;
    }
}