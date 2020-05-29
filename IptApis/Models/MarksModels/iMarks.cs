using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IptApis.MarksModels
{
    public class iMarks
    {
        public int MDID;
        public List<Student_Marks> SMs;
    }
    public class Student_Marks
    {
        public int StudentID;
        public double ObtainedMarks;
    }
}