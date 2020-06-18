
//using IptApis.Models.Attendance;
using IptApis.Models.Attendance;

using IptApis.Shared;
using Newtonsoft.Json;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;

namespace IptApis.Controllers.Attendance
{
    public class AttendanceTeacherController : ApiController
    {
        public int teacherid;

        AttendanceTeacherController() {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            //teacherid = db.Query("CourseFaculty").Where("EmpID", empId).Select("CFID").Get<int>().First();
        }
        

        //api/Attendanceteacher/GetAttendancebyEnroll/2
        //will return attendance status by enrollment id
        public HttpResponseMessage GetAttendancebyEnroll(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<Attend> response = db.Query("Attendance").Where("EnrollmentID", id).Get<Attend>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        //[HttpGet]
        //public List<Attend> GetAttendList(int id)
        //{
        //    List<Attend> records = new List<Attend>();
        //    string query = "select Attend.AttendanceID, Attend.AttendanceStatus " +
        //                   "from FacultySections,CourseEnrollment,Student, Attendance " +
        //                   "where FacultySections.FSID=CourseEnrollment.FSID and CourseEnrollment.EnrollmentID= Attendance.EnrollmentID and FacultySections.FSID=" + id;

        //    string connectionString = ConfigurationManager.AppSettings["SqlDBConn"].ToString();
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand command = new SqlCommand(query, connection);
        //        command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();
        //        try
        //        {
        //            while (reader.Read())
        //            {
        //                Attend temp = new Attend();
        //                temp.AttendanceID = reader.GetInt32(reader.GetOrdinal("AttendanceID"));

        //                temp.AttendanceStatus = reader.GetString(reader.GetOrdinal("AttendanceStatus"));
        //                records.Add(temp);
        //            }
        //        }
        //        finally
        //        {
        //            reader.Close();
        //        }
        //    }
        //    return records;
        //}

        //api/Attendanceteacher/getcoursestudents/1
        //Will return all students in the course

        /*[HttpGet]
        //api/Attendanceteacher/getcoursesection/1
        //will return that course's sections
        public List<Section> Get_CourseSection(int id)
        {
            //return teacher_id.ToString();
            List<Section> records = new List<Section>();
            string query = "select FacultySections.FSID,Section.SectionName " +
                           "from CourseFaculty,FacultySections,Section " +
                           "where CourseFaculty.CFID=FacultySections.CFID and FacultySections.SectionID = Section.SectionID and CourseFaculty.CFID =" + id;

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
                        Section temp = new Section();
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
*/
         


        [HttpPost]
        //it will give all students of that teacher course
        //api/AttendanceTeacher/GetTeacherStudents/11
        public HttpResponseMessage GetTeacherStudents(AddAttendanceVM data)//courseid
        {



            //int FSID = 13;


            //int SemesterID = 1;
            string EmpName = data.EmpName;

            string SectionName = data.SectionName;

            //int CourseID = 7;
            Int32.TryParse(data.CourseID, out int CourseID);

            //int empId = id; // teacher login id


            var db = DbUtils.GetDBConnection();
            db.Connection.Open();


            //IEnumerable<FacultySections> response0;
            //response = db.Query("CourseEnrollment").Where("StudentID", id).Get<CourseEnrollment>();
            //response0 = db.Query("TeacherCourseSectionDetail").Where("EmpID", empId).Select("FSID").Get<FacultySections>();
            //return Request.CreateResponse(HttpStatusCode.OK, response0);                                    

            //int CFID = db.Query("CourseFaculty").Where("EmpID", empId).Select("CFID").Get<int>().First();
            //int FSID = db.Query("FacultySections").Where("CFID", CFID).Select("FSID").Get<int>().First();

            //return FSID;

            IEnumerable<Student> response;
           
            response = db.Query("TeacherAttendanceDetails").Where("EmpName", EmpName)
                                                    .Where("CourseID", CourseID)                                                   
                                                    .Where("SectionName", SectionName)                                              
                                                    .Select("StudentID","SName","RollNumber","EnrollmentID").Distinct()
                                                    .Get<Student>();
            db.Connection.Close();
            return Request.CreateResponse(HttpStatusCode.OK, response);



        }

        [HttpPost]
        //it will give section of that teacher course
        //api/AttendanceTeacher/GetTeacherSections/7
        public HttpResponseMessage GetTeacherSections(DataVM data) //course id
        {
            //int id = 11;
            string empId = data.empName; // teacher login id
            Int32.TryParse(data.courseID, out int cid);

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<Section> response;
            //response = db.Query("CourseEnrollment").Where("StudentID", id).Get<CourseEnrollment>();
            response = db.Query("TeacherCourseSectionDetail").Where("EmpName", empId).Where("CourseID", cid).Get<Section>();
            db.Connection.Close();
            return Request.CreateResponse(HttpStatusCode.OK, response);


        }

        [HttpGet]
        //return teacher courses
        //api/AttendanceTeacher/GetTeacherCourses/11
        public HttpResponseMessage GetTeacherCourses(int id)
        {

            int empId = id; // teacher login id
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IptApis.Models.Attendance.Course> response;
            response = db.Query("TeacherCourseSectionDetail").Where("EmpID", empId).Select("CourseID", "CourseCode", "CourseName", "CFID").Distinct().Get<IptApis.Models.Attendance.Course>();
            db.Connection.Close();

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        [HttpGet]
        //return teacher's name
        //api/AttendanceTeacher/GetTeacherName/11
        public HttpResponseMessage GetTeacherName(int id)
        {

            int empId = id; // teacher login id
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<Employee> response;
            response = db.Query("Employee").Where("EmpID", empId).Get<Employee>();
            db.Connection.Close();

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

        [HttpGet]
        //return Semester
        //api/AttendanceTeacher/GetTeacherSemester/11
        public HttpResponseMessage GetTeacherSemester(int id)
        {

            int empId = id; // teacher login id
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<IptApis.Models.Attendance.Semester> response;
            response = db.Query("TeacherCourseSectionDetail").Where("EmpID", empId).Select("SemesterID", "SemesterName").Distinct().Get<IptApis.Models.Attendance.Semester>();
            db.Connection.Close();

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }





        [HttpPost]
        //api/AttendanceTeacher/AddStudentAttendance
        public HttpResponseMessage AddStudentAttendance(List<Attend> data)
        {

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();


            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach(var item in data)
                    {
                        var query = db.Query("Attendance").InsertGetId<int>(new
                        {
                            AttendanceDate = item.AttendanceDate,
                            AttendanceStatus = item.AttendanceStatus,
                            ClassDuration = item.ClassDuration,
                            EnrollmentID = item.EnrollmentID
                        });
                    }
                    
                    scope.Complete();  // if record is entered successfully , transaction will be committed
                    db.Connection.Close();
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                catch (Exception ex)
                {
                    scope.Dispose();   //if there are any error, rollback the transaction
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }




            //int FSID = 13;


            //int SemesterID = 1;
            //string EmpName = data.EmpName;

            //string SectionName = data.SectionName;

            //int CourseID = 7;
            //Int32.TryParse(data.CourseID, out int CourseID);

            //int empId = id; // teacher login id




            //IEnumerable<FacultySections> response0;
            //response = db.Query("CourseEnrollment").Where("StudentID", id).Get<CourseEnrollment>();
            //response0 = db.Query("TeacherCourseSectionDetail").Where("EmpID", empId).Select("FSID").Get<FacultySections>();
            //return Request.CreateResponse(HttpStatusCode.OK, response0);                                    

            //int CFID = db.Query("CourseFaculty").Where("EmpID", empId).Select("CFID").Get<int>().First();
            //int FSID = db.Query("FacultySection/
            //return FSID;

            //IEnumerable<Student> response;

            //response = db.Query("TeacherAttendanceDetails").Where("EmpName", EmpName)
            //                                        .Where("CourseID", CourseID)
            //                                        .Where("SectionName", SectionName)
            //                                        .Select("StudentID", "SName", "RollNumber").Distinct()
            //                                        .Get<Student>();
            //db.Connection.Close();
            //return Request.CreateResponse(HttpStatusCode.OK, response);


            /*
                        var db = DbUtils.GetDBConnection();
                        db.Connection.Open();

                        int attendid = db.Query("Attendance").InsertGetId<int>(new
                        {
                            newAttend.AttendanceDate,
                            newAttend.AttendanceStatus,
                            newAttend.ClassDuration,
                            newAttend.EnrollmentID

                        });
                        *//*foreach (string description in newAttend.DescriptionList)
                        {
                            db.Query("JobDescription").InsertGetId<int>(new { attendid, description });
                        }*//*

                        db.Connection.Close();
                        return attendid;*/



            //int empId = 11;
            //var db = DbUtils.GetDBConnection();
            //db.Connection.Open();


            //int CFID = db.Query("CourseFaculty").Where("EmpID", empId).Select("CFID").Get<int>().First();
            //int FSID = db.Query("FacultySections").Where("CFID", CFID).Select("FSID").Get<int>().First();

            ////string AttendanceDate = "2020-04-23";



            //int SemesterID = 1;

            //int SectionID = 1;

            //int CourseID = 7;

            //int studentid = 9; //student id aayegi

            //int enrollID = db.Query("TeacherAttendanceDetails").Where("EmpID", empId)
            //                                                .Where("CourseID", CourseID)
            //                                                .Where("SectionID", SectionID)
            //                                                .Where("SemesterID", SemesterID)
            //                                                .Where("StudentID", studentid).Select("EnrollmentID").Get<int>().First();

            //return enrollID;

            //IEnumerable<TeacherAttendanceDetails> response;
            //response = db.Query("TeacherAttendanceDetails").Where("EmpID", empId)
            //                                                .Where("CourseID", CourseID)
            //                                                .Where("SectionID", SectionID)
            //                                                .Where("SemesterID", SemesterID).Get<TeacherAttendanceDetails>();


            //db.Query("Attendance").Insert(new Attend
            //{

            //});
            ////db.Query("Attendance").Add()


            return Request.CreateResponse(HttpStatusCode.OK, "hello");


        }

    
        //DELETE COMPLETE ATTENDANCE date
        [HttpDelete]
        [AllowAnonymous]

        //api/AttendanceTeacher/DeleteStudentAttendance?date=2020-04-28
        public HttpResponseMessage DeleteStudentAttendance(string date)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            try
            {
                _ = db.Query("Attendance").Where("AttendanceDate", "=", date).Delete();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message);
            }
           

        }
    }
}