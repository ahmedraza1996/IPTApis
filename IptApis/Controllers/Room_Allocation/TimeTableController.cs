using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IptApis.Controllers.Room_Allocation
{
    public class TimeTableController : ApiController
    {
        //asd
        private List<TimetableModel> complete = new List<TimetableModel>();
        [System.Web.Http.HttpGet]
        public List<TimetableModel> GetCourses()
        {
            //populate this list will the items needed from the DB
            List<TimetableModel> TTM = new List<TimetableModel>();


            string queryString = "select CourseOffered.CreditHrs,Course.CourseName,Employee.EmpName,Section.SectionName,Batch.BatchName from Employee, Section, Batch, FacultySections, CourseFaculty, Course, CourseOffered " +
                "where CourseFaculty.EmpID = Employee.EmpID and CourseFaculty.CFID = FacultySections.CFID and Section.SectionID = FacultySections.SectionID and Batch.BatchID = Section.BatchID " +
                "and Course.CourseID = CourseOffered.CourseID and CourseOffered.CourseOfferedID = CourseFaculty.CourseOfferedID and CourseOffered.CreditHrs = 3; ";

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
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
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return TTM;
        }

        public List<String> GetInstructor()
        {
            //populate this list will the items needed from the DB
            List<String> TTM = new List<String>();


            string queryString = "select distinct(Employee.EmpName) from Employee, Section, Batch, FacultySections, CourseFaculty, Course, CourseOffered " +
                "where CourseFaculty.EmpID = Employee.EmpID and CourseFaculty.CFID = FacultySections.CFID and Section.SectionID = FacultySections.SectionID and Batch.BatchID = Section.BatchID " +
                "and Course.CourseID = CourseOffered.CourseID and CourseOffered.CourseOfferedID = CourseFaculty.CourseOfferedID and CourseOffered.CreditHrs = 3; ";

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {                     
                        String CourseInstructor = reader.GetString(reader.GetOrdinal("EmpName"));
                        TTM.Add(CourseInstructor);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return TTM;
        }

        [System.Web.Http.HttpGet]
        public TimeTableData extraclass(String course, String Section, int day, String instructor)
        {
            List<TimeTableData> TTD = FetchTimetable();
            int checker = 0;
            for (int ts = 1; ts < 9; ts++)
            {
                checker = 0;
                for (int i = 0; i < TTD.Count; i++)
                {
                    if (TTD[i].day == 1 && TTD[i].course.Equals(course) && TTD[i].section.Equals(Section) && TTD[i].timeslot == ts && TTD[i].empname.Equals(instructor))
                    {
                        checker = 1;
                        break;
                    }
                }
                if (checker == 0)
                {
                    TimeTableData td = new TimeTableData();
                    td.course = course;
                    td.day = day;
                    td.empname = instructor;
                    td.timeslot = ts;
                    td.section = Section;
                    td.room = "" + (roomempty(day, ts) + 1);

                    string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();



                    SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                    String batcher = "Batch16";
                    for(int i=0;i<TTD.Count;i++)
                    {
                        if(TTD[i].course.Equals(td.course))
                        {
                            batcher = TTD[i].batch;
                            break;
                        }
                    }
                    int rooms = roomempty(day, ts) + 1;
                    int batchid = getBatchID(batcher);
                    SqlCommand cmd = new SqlCommand("insert into Timetable(empID,dayID,courseID,sectionID,batchID,roomID,timeslotID) values (" + getinstructorID(td.empname) + "," + td.day + "" +
                        "," + getCourseID(td.course) + "," + getSectionID(td.section, batchid) + "," + batchid + "," + rooms + "," + td.timeslot + ")", con);

                    int asd = cmd.ExecuteNonQuery();

                    con.Close();
                    td.batch = batcher;
                    return td;
                }
            }

            return null;
        }

        [System.Web.Http.HttpGet]
        public bool reallocateclassroom(String course, String Section, int day)
        {
            String roomname;
            int timeslot = 1;
            int timetableid = 0;
            string queryString = "select Rooms.Roomname, Timeslot.Slot,Timetable.timetableID " +
            "from Employee,Course,Batch,daysofweek,Section,Rooms,Timeslot,Timetable " +
            "where Timetable.empID = Employee.EmpID and Timetable.courseID = Course.CourseID and " +
            "Section.SectionID = Timetable.sectionID and Batch.BatchID = Timetable.batchID and " +
            "Timetable.batchID = Section.BatchID and Rooms.RoomID = Timetable.roomID and " +
            "Timetable.timeslotID = Timeslot.TimeslotID and Timetable.dayID = daysofweek.dayID" +
            " and daysofweek.dayvalue = " + day + " and SectionName like '" + Section + "' and CourseName like '" + course + "' ;";

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        timetableid = reader.GetInt32(reader.GetOrdinal("timetableID"));
                        roomname = reader.GetString(reader.GetOrdinal("Roomname"));
                        timeslot = reader.GetInt32(reader.GetOrdinal("Slot"));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }

            int newroom = roomempty(day, timeslot) + 1;

            connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("update Timetable set Timetable.roomID = " + newroom + " where Timetable.timetableID = " + timetableid + ";", con);

            int asd = cmd.ExecuteNonQuery();
            con.Close();
            return false;
        }
        private int roomempty(int day, int timeslot)
        {
            List<String> OuccupiedRooms = new List<string>();

            List<String> Allrooms = GetRooms();

            string queryString = "select Rooms.Roomname " +
            "from Employee,Course,Batch,daysofweek,Section,Rooms,Timeslot,Timetable " +
            "where Timetable.empID = Employee.EmpID and Timetable.courseID = Course.CourseID and " +
            "Section.SectionID = Timetable.sectionID and Batch.BatchID = Timetable.batchID and " +
            "Timetable.batchID = Section.BatchID and Rooms.RoomID = Timetable.roomID and " +
            "Timetable.timeslotID = Timeslot.TimeslotID and Timetable.dayID = daysofweek.dayID" +
            " and daysofweek.dayvalue = " + day + " and Timeslot.Slot = " + timeslot + ";";

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {

                        OuccupiedRooms.Add(reader.GetString(reader.GetOrdinal("Roomname")));

                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            int checker = 0;
            for (int i = 0; i < Allrooms.Count; i++)
            {
                checker = 0;
                for (int j = 0; j < OuccupiedRooms.Count; j++)
                {

                    if (Allrooms[i].Equals(OuccupiedRooms[j]))
                    {
                        checker = 1;
                        break;
                    }
                }
                if (checker == 0)
                {
                    return i;
                }
            }

            return 0;
        }



        [System.Web.Http.HttpGet]
        public TimeTableData FetchClassroom(String course, String Section, int day)
        {
            TimeTableData temp = new TimeTableData();

            string queryString = "select Employee.EmpName,Course.CourseName,Section.SectionName,Batch.BatchName, daysofweek.dayvalue,Rooms.Roomname,Timeslot.Slot " +
            "from Employee,Course,Batch,daysofweek,Section,Rooms,Timeslot,Timetable " +
            "where Timetable.empID = Employee.EmpID and Timetable.courseID = Course.CourseID and " +
            "Section.SectionID = Timetable.sectionID and Batch.BatchID = Timetable.batchID and " +
            "Timetable.batchID = Section.BatchID and Rooms.RoomID = Timetable.roomID and " +
            "Timetable.timeslotID = Timeslot.TimeslotID and Timetable.dayID = daysofweek.dayID" +
            " and daysofweek.dayvalue = " + day + " and SectionName like '" + Section + "' and CourseName like '" + course + "' ;";

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {

                        temp.course = reader.GetString(reader.GetOrdinal("CourseName"));
                        temp.empname = reader.GetString(reader.GetOrdinal("EmpName"));
                        temp.section = reader.GetString(reader.GetOrdinal("SectionName"));
                        temp.batch = reader.GetString(reader.GetOrdinal("BatchName"));
                        temp.room = reader.GetString(reader.GetOrdinal("Roomname"));
                        temp.day = reader.GetInt32(reader.GetOrdinal("dayvalue"));
                        temp.timeslot = reader.GetInt32(reader.GetOrdinal("Slot"));

                    }
                }
                finally
                {
                    reader.Close();
                }
            }

            return temp;
        }
        [System.Web.Http.HttpGet]
        public void ClearDB()
        {
            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();
            SqlConnection con = new SqlConnection(connectionString);

            con.Open();
            
            SqlCommand cmd = new SqlCommand("DBCC CHECKIDENT('Timetable', RESEED, 0);", con);
            int poe = cmd.ExecuteNonQuery();
            cmd = new SqlCommand("delete from Timetable", con);
            int asd = cmd.ExecuteNonQuery();
            con.Close();
        }

        [System.Web.Http.HttpGet]
        public List<TimeTableData> FetchTimetable()
        {
            //populate this list will the items needed from the DB
            List<TimeTableData> TTM = new List<TimeTableData>();


            string queryString = "select Employee.EmpName,Course.CourseName,Section.SectionName,Batch.BatchName, daysofweek.dayvalue,Rooms.Roomname,Timeslot.Slot " +
                "from Employee,Course,Batch,daysofweek,Section,Rooms,Timeslot,Timetable " +
                "where Timetable.empID = Employee.EmpID and Timetable.courseID = Course.CourseID and " +
                "Section.SectionID = Timetable.sectionID and Batch.BatchID = Timetable.batchID and " +
                "Timetable.batchID = Section.BatchID and Rooms.RoomID = Timetable.roomID and " +
                "Timetable.timeslotID = Timeslot.TimeslotID and Timetable.dayID = daysofweek.dayID;";

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    int i = 0;
                    while (reader.Read()   )
                    {
                        //i++;
                        TimeTableData temp = new TimeTableData();
                        temp.course = reader.GetString(reader.GetOrdinal("CourseName"));
                        temp.empname = reader.GetString(reader.GetOrdinal("EmpName"));
                        temp.section = reader.GetString(reader.GetOrdinal("SectionName"));
                        temp.batch = reader.GetString(reader.GetOrdinal("BatchName"));
                        temp.room = reader.GetString(reader.GetOrdinal("Roomname"));
                        temp.day = reader.GetInt32(reader.GetOrdinal("dayvalue"));
                        temp.timeslot = reader.GetInt32(reader.GetOrdinal("Slot"));
                        TTM.Add(temp);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return TTM;
        }

        public List<TimetableModel> GetLabCourses()
        {
            //populate this list will the items needed from the DB
            List<TimetableModel> TTM = new List<TimetableModel>();


            string queryString = "select CourseOffered.CreditHrs,Course.CourseName,Employee.EmpName,Section.SectionName,Batch.BatchName from Employee, Section, Batch, FacultySections, CourseFaculty, Course, CourseOffered " +
                "where CourseFaculty.EmpID = Employee.EmpID and CourseFaculty.CFID = FacultySections.CFID and Section.SectionID = FacultySections.SectionID and Batch.BatchID = Section.BatchID " +
                "and Course.CourseID = CourseOffered.CourseID and CourseOffered.CourseOfferedID = CourseFaculty.CourseOfferedID and CourseOffered.CreditHrs = 1; ";

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
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
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return TTM;
        }

        public List<String> GetLabs()
        {
            List<String> roomnames = new List<string>();

            string queryString = "select Roomname from Rooms where Roomname like 'Lab%' ";

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        roomnames.Add(reader.GetString(reader.GetOrdinal("Roomname")));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return roomnames;
        }

        public List<String> GetRooms()
        {
            List<String> roomnames = new List<string>();

            string queryString = "select Roomname from Rooms where Roomname not like 'Lab%' ";

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        roomnames.Add(reader.GetString(reader.GetOrdinal("Roomname")));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return roomnames;
        }
        [System.Web.Http.HttpGet]
        public List<TimetableModel> classtimetalbe()
        {
            List<TimetableModel> TTM = GetCourses();

            int count = TTM.Count;
            for(int i=0;i<count;i++)
            {
                TTM[i].Room = "Empty";
                
            }
           
            
            count = TTM.Count;
            TTM = TTM.OrderBy(o => o.coursename).ToList();
            //list of all classrooms
            List<String> classrooms = GetRooms();
            //counter for checking the status of the room that is has been allocated
            int counter = 0;
            int day = 1; // days vales is  1 - 5
            int timeslot = 4; // timeslot vales are 4-8
            int roomcounter = 0; // number of classrooms
            while (counter < count)
            {

                if (timeslot > 8)
                {
                    day++;
                    timeslot = 4;
                }
                if (day > 5 || day < 1)
                {
                    day = 1;
                }
                if (roomcounter > classrooms.Count - 1)
                {
                    roomcounter = 0;
                }

                if (checksection(TTM[counter].Section, TTM[counter].Batch, TTM, day, timeslot) && instructorFree(TTM[counter].CourseInstructor, TTM, day, timeslot))
                {
                    System.Diagnostics.Debug.WriteLine("Total size = " + counter);
                    TTM[counter].Room = classrooms[roomcounter];
                    TTM[counter].timeslot = timeslot;
                    TTM[counter].day = day;
                    roomcounter++;
                    counter++;
                    //timeslot++;
                }
                else
                {
                    timeslot++;
                }

            }
            List<TimetableModel> labs = maketimetable();
            for (int m = 0; m < labs.Count; m++)
            {
                TTM.Add(labs[m]);
            }
            TTM = TTM.OrderBy(o => o.day).ToList();
            System.Diagnostics.Debug.WriteLine("Total size = " + TTM.Count);
            return TTM;
        }

        [System.Web.Http.HttpGet]
        public void storeindb()
        {
            List<TimetableModel> TTM = classtimetalbe();
            //the complete timetable
            //now get ID from their tables
            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();



            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            for (int i = 0; i < TTM.Count; i++)
            {
                int batchid = getBatchID(TTM[i].Batch);
                SqlCommand cmd = new SqlCommand("insert into Timetable(empID,dayID,courseID,sectionID,batchID,roomID,timeslotID) values (" + getinstructorID(TTM[i].CourseInstructor) + "," + TTM[i].day + "" +
                    "," + getCourseID(TTM[i].coursename) + "," + getSectionID(TTM[i].Section, batchid) + "," + batchid + "," + getRoomID(TTM[i].Room) + "," + TTM[i].timeslot + ")", con);

                int asd = cmd.ExecuteNonQuery();

            }

            con.Close();




        }

        private int getinstructorID(String name)
        {
            int id = 0;

            string queryString = "select * from Employee where EmpName like '" + name + "' ";

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(reader.GetOrdinal("EmpID"));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return id;
        }

        private int getRoomID(String name)
        {
            int id = 0;

            string queryString = "select * from Rooms where Roomname like '" + name + "' ";

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(reader.GetOrdinal("RoomID"));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return id;
        }

        private int getCourseID(String name)
        {
            int id = 0;

            string queryString = "select * from Course where CourseName like '" + name + "' ";

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(reader.GetOrdinal("CourseID"));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return id;
        }

        private int getBatchID(String name)
        {
            int id = 0;

            string queryString = "select * from Batch where BatchName like '" + name + "' ";

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(reader.GetOrdinal("BatchID"));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return id;
        }

        private int getSectionID(String name, int batchid)
        {
            int id = 0;

            string queryString = "select * from Section where SectionName like '" + name + "' and BatchID = " + batchid + "";

            string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(reader.GetOrdinal("SectionID"));
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return id;
        }
        public bool instructorFree(String name, List<TimetableModel> timetables, int day, int timeslot)
        {
            for (int i = 0; i < timetables.Count; i++)
            {
                if (timetables[i].CourseInstructor.Equals(name) && timetables[i].day == day && timetables[i].timeslot == timeslot && timetables[i].Room.Equals("Empty"))
                {
                    System.Diagnostics.Debug.WriteLine("Room Rejected");

                    return false;
                }
            }
            return true;
        }

        public int sectionFree(String section, String batch, List<TimetableModel> timetables, int day)
        {
            for (int i = 0; i < timetables.Count; i++)
            {
                if (timetables[i].Section.Equals(section) && timetables[i].Batch.Equals(batch) && timetables[i].timeslot == 1 && timetables[i].day == day)
                {
                    if (timetables[i].Section.Equals(section) && timetables[i].Batch.Equals(batch) && timetables[i].timeslot == 2 && timetables[i].day == day)
                    {
                        return 2;
                    }
                    else
                    {
                        return 2;
                    }
                }
            }
            return 1;
        }
        [System.Web.Http.HttpGet]
        public List<TimetableModel> maketimetable()
        {
            //list of all courses
            List<TimetableModel> TTM = GetLabCourses();
            //list of all labs
            List<String> labs = GetLabs();
            //list of all classrooms
            List<String> classrooms = GetRooms();

            int day = 1;
            int timeslot = 1;
            int i = 0;
            while (i < TTM.Count)
            {
                if (instructorFree(TTM[i].CourseInstructor, TTM, day))
                {
                    System.Diagnostics.Debug.WriteLine("Instrctor found?");
                    if (sectionFree(TTM[i].Section, TTM[i].Batch, TTM, day) != 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Section found?");
                        int m = sectionFree(TTM[i].Section, TTM[i].Batch, TTM, day);
                        if (labFree(TTM, labs, day, m) != null || true)
                        {
                            System.Diagnostics.Debug.WriteLine("Completed");
                            TTM[i].day = day;
                            TTM[i].timeslot = m;
                            //TTM[i].Room = labFree(TTM, labs, day, m);
                            i++;
                            day++;
                        }
                        else
                        {
                            day++;
                        }
                    }
                    else
                    {
                        day++;
                    }
                }
                else
                {
                    day++;
                }
                if (day > 5)
                {
                    System.Diagnostics.Debug.WriteLine("Breaking condition");
                    day = 1;
                }
            }
            List<TimetableModel> SortedList = TTM.OrderBy(o => o.day).ToList();
            for (int i1 = 0; i1 < SortedList.Count; i1++)
            {
                SortedList[i1].Room = labFree1(labs);
            }

            int x = SortedList.Count;
            for (int n = 0; n < x; n++)
            {
                SortedList.Add(SortedList[n]);
                SortedList.Add(SortedList[n]);
            }
            SortedList = SortedList.OrderBy(o => o.day).ToList();

            return SortedList;
        }
        
        private bool checksection(String section, String Batch, List<TimetableModel> timetables, int day, int timeslot)
        {
            for (int i = 0; i < timetables.Count; i++)
            {
                if (timetables[i].Section.Equals(section) && timetables[i].day == day && !timetables[i].Room.Equals("Empty") && timetables[i].Batch.Equals(Batch) && timetables[i].timeslot == timeslot)
                {
                    System.Diagnostics.Debug.WriteLine("Room Rejected");

                    return false;
                }
            }
            return true;
        }
        
       


        public bool instructorFree(String name, List<TimetableModel> timetables, int day)
        {
            for (int i = 0; i < timetables.Count; i++)
            {
                if (timetables[i].CourseInstructor.Equals(name) && timetables[i].day == day && !timetables[i].Room.Equals("Empty"))
                {
                    System.Diagnostics.Debug.WriteLine("Room Rejected instructor");

                    return false;
                }
            }
            return true;
        }

      
        static int counter = 0;
        public string labFree(List<TimetableModel> timetables, List<String> labs, int day, int timeslot)
        {
            if (counter > 8)
            {
                counter = 0;
            }

            for (int i = 0; i < timetables.Count; i++)
            {
                //System.Diagnostics.Debug.WriteLine("Entered ROom function");
                /*if (timetables[i].Room == null && timetables[i].timeslot ==  timeslot && timetables[i].day == day)
                {
                    System.Diagnostics.Debug.WriteLine("Room found?");
                    if (counter > 8)
                    {
                        counter = 0;
                    }
                    
                    return labs[counter];
                }*/
                if (timetables[i].Room != null && timetables[i].timeslot == timeslot && timetables[i].day == day)
                {
                    System.Diagnostics.Debug.WriteLine("Not found?");
                    for (int j = 0; j < labs.Count; j++)
                    {
                        if (counter > 8)
                        {
                            counter = 0;
                        }
                        if (timetables[i].Room.Equals(labs[counter]) && timetables[i].timeslot == timeslot && timetables[i].day == day)
                        {
                            counter++;
                        }
                        else
                        {
                            counter++;
                            if (counter > 8)
                            {
                                counter = 0;
                            }
                            return labs[counter];
                        }
                    }
                    break;
                }
                else if (timetables[i].Room == null && timetables[i].timeslot == 0 && timetables[i].day == 0)
                {
                    counter++;
                    if (counter > 8)
                    {
                        counter = 0;
                    }
                    return labs[counter];
                }
                counter++;
            }

            return null;
        }
        static int counter1 = 0;

        public string labFree1(List<String> labs)
        {
            counter1++;
            if (counter1 > 8)
            {
                counter1 = 0;
            }
            return labs[counter1];
        }
    }
}

