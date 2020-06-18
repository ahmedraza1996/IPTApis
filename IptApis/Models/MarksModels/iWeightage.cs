using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.MarksModels
{
    public class iWeightage
    {
        /*
         1-Assignment
        2-Quiz
        3-Project
        4-Lab
        5-Presentation
        6-FYP
        */
        public String title { get; set; }
        public int FSID { get; set; }
        public double Total_marks { get; set; }
        public double weightage { get; set; }
        public int title_getter()
        {
            var temp = title;
            if (temp.Contains("Assignment")) return 1;
            else if (temp.Contains("Quiz")) return 2;
            else if (temp.Contains("Project")) return 3;
            else if (temp.Contains("Lab")) return 4;
            else if (temp.Contains("Presentation")) return 5;
            else if (temp.Contains("FYP")) return 6;
            return 0;
        }
    }
}