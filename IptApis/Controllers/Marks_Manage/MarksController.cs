using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Data.SqlClient;

using System.Web.Script.Serialization;
using System.IO;
using IptApis.MarksModels;

namespace IptApis.Controllers.Marks_Manage
{

    public class MarksController : ApiController
    {
        /*
        [HttpGet]
        public List<TestingModel> Testing()
        {
            //populate this list will the items needed from the DB
            //List<TimetableModel> TTM = new List<TimetableModel>();
            List<TestingModel> temp = new List<TestingModel>();


            string query = "select * from MarksDistribution";

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        TimetableModel temp = new TimetableModel();
                        temp.coursename = reader.GetString(reader.GetOrdinal("CourseName"));
                        temp.CourseInstructor = reader.GetString(reader.GetOrdinal("EmpName"));
                        temp.Section = reader.GetString(reader.GetOrdinal("SectionName"));
                        temp.Batch = reader.GetString(reader.GetOrdinal("BatchName"));
                        temp.credithour = reader.GetInt32(reader.GetOrdinal("CreditHrs"));
                        temp.day = 0;
                        temp.timeslot = 0;
                        TTM.Add(temp);
                        TestingModel test = new TestingModel();
                        test.MDID = reader.GetInt32(reader.GetOrdinal("MDID"));
                        test.Title= reader.GetInt32(reader.GetOrdinal("Title"));
                        test.FSID = reader.GetInt32(reader.GetOrdinal("FSID"));
                        test.Weightage = reader.GetDouble(reader.GetOrdinal("Weigtage"));
                        test.TotalMarks = reader.GetDouble(reader.GetOrdinal("TotalMarks"));
                        temp.Add(test);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return temp;
        }
        */
        [HttpGet]
        //Given teacher ID it will return the teachers courses
        public List<Courses> Get_Courses(int id)
        {
            //return teacher_id.ToString();
            List<Courses> records = new List<Courses>();
            string query = "select CourseFaculty.CFID,Course.CourseName " +
                           "from CourseFaculty,CourseOffered,Course " +
                           "where CourseFaculty.CourseOfferedID=CourseOffered.CourseOfferedID and " +
                           "CourseOffered.CourseID=Course.CourseID and " +
                           "CourseFaculty.EmpID=" + id;

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Courses temp = new Courses();
                        temp.CourseName = reader.GetString(reader.GetOrdinal("CourseName"));
                        temp.CFID = reader.GetInt32(reader.GetOrdinal("CFID"));
                        records.Add(temp);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return records;
    
        }
        [HttpGet]
        //Given CFID(Unqiuely identifies all courses) it will return that course's sections
        public List<Sections> Get_Section(int id)
        {
            //return teacher_id.ToString();
            List<Sections> records = new List<Sections>();
            string query = "select FacultySections.FSID,Section.SectionName " +
                           "from CourseFaculty,FacultySections,Section " +
                           "where CourseFaculty.CFID=FacultySections.CFID and FacultySections.SectionID = Section.SectionID and CourseFaculty.CFID ="+ id;

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Sections temp = new Sections();
                        temp.SectionName = reader.GetString(reader.GetOrdinal("SectionName"));
                        temp.FSID = reader.GetInt32(reader.GetOrdinal("FSID"));
                        records.Add(temp);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return records;

        }
        [HttpGet]
        //Given FSIC(Unqiuely identifies all sections) it will return that section's student list
        public List<Section_Students> Get_Student_Given_Section(int id)
        {
            //return teacher_id.ToString();
            List<Section_Students> records = new List<Section_Students>();
            string query = "select  Student.StudentID,Student.SName " +
                           "from FacultySections,CourseEnrollment,Student " +
                           "where FacultySections.FSID=CourseEnrollment.FSID and Student.StudentID=CourseEnrollment.StudentID and FacultySections.FSID=" + id;

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Section_Students temp = new Section_Students();
                        temp.StudentID= reader.GetInt32(reader.GetOrdinal("StudentID"));
                        temp.StudentName= reader.GetString(reader.GetOrdinal("SName"));
                        records.Add(temp);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return records;

        }
        [HttpGet]
        //Given FSID this will fetch all the current mark progress of said section
        public List<SectionRecords> Get_Section_Records(int id)
        {
   
            //return teacher_id.ToString();
            List<SectionRecords> records = new List<SectionRecords>();
            string query = "select MarksDistribution.MDID,MarksDistribution.Weigtage,MarksDistribution.TotalMarks,MarksDistribution.Title  " +
                           "from MarksDistribution,FacultySections " +
                           "where MarksDistribution.FSID = FacultySections.FSID and MarksDistribution.FSID = " + id;

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        SectionRecords temp = new SectionRecords();
                        temp.weightage = reader.GetDouble(reader.GetOrdinal("Weigtage"));
                        temp.MDID = reader.GetInt32(reader.GetOrdinal("MDID"));
                        temp.totalmarks = reader.GetDouble(reader.GetOrdinal("TotalMarks"));
                        temp.title_setter(reader.GetInt32(reader.GetOrdinal("Title")));
                        /*query= "Student.SName,Student.StudentID,MarksRecord.ObtainedMarks "+
                               "from MarksDistribution,MarksRecord,Student "+
                               "where MarksDistribution.MDID=MarksRecord.MDID and MarksRecord.StudentID=Student.StudentID and MarksDistribution.MDID="+temp.MDID;
                        */
                          /*string connectionString2 = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

                        using (SqlConnection connection2 = new SqlConnection(connectionString2))
                        {
                            SqlCommand command2 = new SqlCommand(query, connection2);
                            command2.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                            SqlDataReader reader2 = command2.ExecuteReader();
                            try
                            {
                                while (reader2.Read())
                                {
                                    StudentRecords temp2 = new StudentRecords();
                                    temp2.ID = reader.GetInt32(reader.GetOrdinal("StudentID"));
                                    temp2.Name = reader.GetString(reader.GetOrdinal("SName"));
                                    temp2.obtained = reader.GetDouble(reader.GetOrdinal("ObtainedMarks"));
                                    temp.SRs.Add(temp2);
                                }
                            }
                            finally
                            {
                                reader2.Close();
                            }
                        }*/
                        records.Add(temp);
                    }
                }
                finally
                {
                    reader.Close();
                }
                for(int i=0;i<records.Count;i++)
                {
                    int count = 0;
                    double max = 0,min= records[i].totalmarks, average=0;bool no_records_found = true;
                    query = "Select Student.SName,Student.StudentID,MarksRecord.ObtainedMarks " +
                               "from MarksDistribution,MarksRecord,Student " +
                               "where MarksDistribution.MDID=MarksRecord.MDID and MarksRecord.StudentID=Student.StudentID and MarksDistribution.MDID=" + records[i].MDID;
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                    reader = command.ExecuteReader();
                    try
                    {
                        while(reader.Read())
                        {
                            StudentRecords temp2 = new StudentRecords();
                            temp2.ID = reader.GetInt32(reader.GetOrdinal("StudentID"));
                            temp2.Name = reader.GetString(reader.GetOrdinal("SName"));
                            temp2.obtained = reader.GetDouble(reader.GetOrdinal("ObtainedMarks"));
                            if (temp2.obtained > max) max = temp2.obtained;
                            if (temp2.obtained < min) min = temp2.obtained;
                            average += temp2.obtained;
                            records[i].SRs.Add(temp2);
                            count++;
                            no_records_found = false;
                        }
                        if(no_records_found)
                        {
                            min = 0;
                            average = 0;
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }
                    records[i].max = max; records[i].min = min; records[i].average = average/count;
                }
            }
            SectionRecords.assign = 0; SectionRecords.quiz = 0; SectionRecords.pro = 0; SectionRecords.fyp = 0; SectionRecords.lab = 0; SectionRecords.pres = 0;
            return records;
        }
        [HttpGet]
        //just a function for some testing
        public List<SectionRecords> test()
        {
            List<SectionRecords> test = new List<SectionRecords>();
            string query = "select MarksDistribution.Title from MarksDistribution ";
            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        SectionRecords yo = new SectionRecords();
                        yo.title_setter((reader.GetInt32(reader.GetOrdinal("Title"))));
                        test.Add(yo);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return test;

        }
        [HttpPost]
        public String insert_weightage(iWeightage temp)
        {
            bool check_FSID=false;double current_weightage=0;bool check_weightage = false;
            if (temp.weightage < 0 || temp.Total_marks < 0 || temp.title_getter() == 0) return "Value Error"; 
            string query = "select FacultySections.FSID from FacultySections where FacultySections.FSID = "+temp.FSID;
            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();
            //checking if foreign key FSID exists
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        check_FSID = true;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            //checking if weightage doesn't overflow
            query = "select isnull(sum(MarksDistribution.Weigtage),0) as Weightage_Sum from MarksDistribution, FacultySections where MarksDistribution.FSID = FacultySections.FSID and FacultySections.FSID = " + temp.FSID;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        current_weightage = reader.GetDouble(reader.GetOrdinal("Weightage_Sum"));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            if (current_weightage + temp.weightage <= 100) check_weightage = true;
            if (check_FSID && check_weightage)
            {
                query = "insert into MarksDistribution values(" + temp.title_getter().ToString() + "," + temp.weightage.ToString() + "," + temp.Total_marks.ToString() + "," + temp.FSID + ")";
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                int return_value = cmd.ExecuteNonQuery();
                connection.Close();
            }
            else return "Failed";
            return "Success";
        }
        [HttpPost]
        //Given list of MDID and student marks uploads it to the database
        public String insert_marks(iMarks temp)
        {
            string query= "select MarksDistribution.TotalMarks from MarksDistribution where MarksDistribution.MDID = "+temp.MDID.ToString();bool all_checked=true;
            bool no_repeat=true;
            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();
            double total_marks = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        total_marks = reader.GetDouble(reader.GetOrdinal("TotalMarks"));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            //check if MDID not already inserted then use update request
            query = " select distinct(MarksRecord.MDID) from MarksRecord where MarksRecord.MDID = "+temp.MDID.ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                /*bool any_record = true;*/
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        no_repeat = false;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            if(no_repeat== false)
            {
                return "Please use update query cause record already exists";
            }
            //check if their marks < total and student exist in course
            foreach (var data in temp.SMs)
            {
                /*
                query= " select CourseEnrollment.StudentID from MarksRecord, MarksDistribution, FacultySections, CourseEnrollment where MarksRecord.MDID = MarksDistribution.MDID and MarksDistribution.FSID = FacultySections.FSID and FacultySections.FSID = CourseEnrollment.FSID and MarksRecord.StudentID = CourseEnrollment.StudentID and"+
                    " MarksDistribution.TotalMarks <= "+data.ObtainedMarks.ToString()+" and MarksDistribution.MDID = "+temp.MDID.ToString()+" and CourseEnrollment.StudentID = "+data.StudentID.ToString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    bool any_record = false;
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            any_record = true;
                        }
                        if(any_record)
                        {
                            all_checked = false;
                            break;
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
                */
                if ((data.ObtainedMarks > total_marks) || (data.ObtainedMarks < 0 )) return "One or more of the Students Marks are greater than total marks";
            }

            if (all_checked)
            {
                foreach (var data in temp.SMs)
                {
                    query = "insert into MarksRecord values (" + data.ObtainedMarks.ToString() + "," + temp.MDID.ToString() + "," + data.StudentID.ToString() + ")";
                    SqlConnection connection = new SqlConnection(connectionString);
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(query, connection);
                    int return_value = cmd.ExecuteNonQuery();
                    connection.Close();
                }
                return "Success";
            }
            else return "Failed";
        }
        [HttpPut]
        public String update_distribution(Update_Distribution temp)
        {
            /*bool check_weightage_100 = true;bool check_total_overflow = false;*/double sum_weightage = 0;double current_weightage = 0;double current_min_marks = 0;
            if (temp.weightage < 0 || temp.Total_marks < 0) return "Value Error";
            String query = "select isnull(sum(MarksDistribution.Weigtage),0) as SUM from MarksDistribution where MarksDistribution.FSID = "+temp.FSID.ToString();
            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        sum_weightage = reader.GetDouble(reader.GetOrdinal("SUM"));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            query = "select MarksDistribution.Weigtage from MarksDistribution where MarksDistribution.MDID = " + temp.MDID.ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        current_weightage = reader.GetDouble(reader.GetOrdinal("Weigtage"));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            query = "select isnull(min(MarksRecord.ObtainedMarks),0) as min from MarksRecord where MarksRecord.MDID = " + temp.MDID.ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        current_min_marks = reader.GetDouble(reader.GetOrdinal("min"));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            if ((sum_weightage - current_weightage + temp.weightage) <= 100 && (temp.Total_marks >= current_min_marks))
            {
                query = "update MarksDistribution set TotalMarks = " + temp.Total_marks.ToString() + ", Weigtage = " + temp.weightage.ToString() + " where MDID = " + temp.MDID.ToString();
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                int return_value = cmd.ExecuteNonQuery();
                connection.Close();
                return "Success";
            }
            else return "Failed";
        }
        [HttpPut]
        public String update_marks(iMarks temp)
        {
            string query = "select MarksDistribution.TotalMarks from MarksDistribution where MarksDistribution.MDID = " + temp.MDID.ToString(); bool all_checked = true;
            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();
            double total_marks = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        total_marks = reader.GetDouble(reader.GetOrdinal("TotalMarks"));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            foreach (var data in temp.SMs)
            {
                /*
                query = " select CourseEnrollment.StudentID from MarksRecord, MarksDistribution, FacultySections, CourseEnrollment where MarksRecord.MDID = MarksDistribution.MDID and MarksDistribution.FSID = FacultySections.FSID and FacultySections.FSID = CourseEnrollment.FSID and MarksRecord.StudentID = CourseEnrollment.StudentID and" +
                    " MarksDistribution.TotalMarks <= " + data.ObtainedMarks.ToString() + " and MarksDistribution.MDID = " + temp.MDID.ToString() + " and CourseEnrollment.StudentID = " + data.StudentID.ToString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    bool any_record = false;
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            any_record = true;
                        }
                        if (any_record)
                        {
                            all_checked = false;
                            break;
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
                */
                if ((data.ObtainedMarks > total_marks) || (data.ObtainedMarks < 0)) return "One or more of the Students Marks are greater than total marks";
            }
            if (all_checked)
            {
                foreach(var data in temp.SMs)
                {
                    query = "update MarksRecord set ObtainedMarks = "+data.ObtainedMarks.ToString()+ " where MarksRecord.MDID ="+temp.MDID.ToString()+" and MarksRecord.StudentID="+data.StudentID.ToString();
                    SqlConnection connection = new SqlConnection(connectionString);
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(query, connection);
                    int return_value = cmd.ExecuteNonQuery();
                    connection.Close();
                }
                return "Success";
            }
            else return "Failed";
        }
        [HttpDelete]
        //here id is the MDID
        public String Delete_Records(int id)
        {
            String query = "delete from MarksRecord where MarksRecord.MDID =" +id.ToString();
            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            int return_value = cmd.ExecuteNonQuery();
            connection.Close();
            query = "delete from MarksDistribution where MarksDistribution.MDID =" + id.ToString();
            connection = new SqlConnection(connectionString);
            connection.Open();
            cmd = new SqlCommand(query, connection);
            return_value = cmd.ExecuteNonQuery();
            connection.Close();
            return "Success";
        }
        [HttpGet]
        //Given FSID
        public List<Student_Details> Student_Summary(int id)
        {   
            //i did not put this in appsetting as this is a merged project and this function is only used by our project's win service
            //String File_Output = "D:\\IPT\\Project";
            List<Student_Details> SDs = new List<Student_Details>();
            string query = "select  Student.StudentID,Student.SName,Student.Email " +
                           "from FacultySections,CourseEnrollment,Student " +
                           "where FacultySections.FSID=CourseEnrollment.FSID and Student.StudentID=CourseEnrollment.StudentID and FacultySections.FSID=" + id.ToString();

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Student_Details sds = new Student_Details();
                        sds.StudentID = reader.GetInt32(reader.GetOrdinal("StudentID"));
                        sds.StudentName = reader.GetString(reader.GetOrdinal("SName"));
                        sds.Email = reader.GetString(reader.GetOrdinal("Email"));

                        SDs.Add(sds);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            for (int i=0;i<SDs.Count;i++)
            {
                Dist_list.assign = 0; Dist_list.fyp = 0; Dist_list.pro = 0; Dist_list.lab = 0; Dist_list.pres = 0; Dist_list.quiz = 0;

                query = "select Course.CourseName " +
                "from FacultySections, CourseFaculty, CourseOffered, Course " +
                "where FacultySections.CFID = CourseFaculty.CFID and CourseFaculty.CourseOfferedID = CourseOffered.CourseOfferedID and CourseOffered.CourseID = Course.CourseID "
                + "and FacultySections.FSID = " + id.ToString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            SDs[i].CourseName = reader.GetString(reader.GetOrdinal("CourseName"));
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }
                }

                query = "select MarksDistribution.MDID,MarksDistribution.Weigtage,MarksDistribution.TotalMarks,MarksDistribution.Title,MarksRecord.ObtainedMarks"+
                        " from MarksDistribution, MarksRecord"+
                        " where MarksDistribution.MDID = MarksRecord.MDID and MarksDistribution.FSID = "+id.ToString()+"  and MarksRecord.StudentID = " + SDs[i].StudentID.ToString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        while (reader.Read())
                        {
                            Dist_list temp = new Dist_list();
                            temp.MDID = (reader.GetInt32(reader.GetOrdinal("MDID")));
                            temp.title_setter(reader.GetInt32(reader.GetOrdinal("Title")));
                            temp.currentmarks = reader.GetDouble(reader.GetOrdinal("ObtainedMarks"));
                            temp.totalmarks= reader.GetDouble(reader.GetOrdinal("TotalMarks"));
                            temp.weightage= reader.GetDouble(reader.GetOrdinal("Weigtage"));
                            SDs[i].DL.Add(temp);
                        }

                    }
                    finally
                    {
                        reader.Close();
                    }
                }
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //var data = serializer.Serialize(SDs);
            //File.WriteAllText(File_Output + "\\Summary.json", data);
            //File.WriteAllText(File_Output + "\\Check", "New Data");
            return SDs;
        }
    }

}
