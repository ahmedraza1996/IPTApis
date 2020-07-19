using IptApis.Models.Attendance;
using IptApis.Shared;
using Newtonsoft.Json;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IptApis.Controllers.Attendance
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AttendanceTeacherController : ApiController
    {
        public int teacherid;

        private AttendanceTeacherController()
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            //teacherid = db.Query("CourseFaculty").Where("EmpID", empId).Select("CFID").Get<int>().First();
        }

        //api/Attendanceteacher/GetEmployeeData/2
        [HttpPost]
        public HttpResponseMessage GetEmployeeData(int id)
        {
            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<Employee> response;
            response = db.Query("Employee").Where("EmpID", id)
                                           .Select("EmpID", "EmpName", "MobileNumber", "Email")
                                           .Get<Employee>();
            return Request.CreateResponse(HttpStatusCode.OK, response);
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
                                                    .Select("StudentID", "SName", "RollNumber", "EnrollmentID").Distinct()
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

            IEnumerable<Models.Attendance.Course> response;
            response = db.Query("TeacherCourseSectionDetail").Where("EmpID", empId).Select("CourseID", "CourseCode", "CourseName", "CFID").Distinct().Get<Models.Attendance.Course>();
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

            IEnumerable<Models.Attendance.Semester> response;
            response = db.Query("TeacherCourseSectionDetail").Where("EmpID", empId).Select("SemesterID", "SemesterName").Distinct().Get<Models.Attendance.Semester>();
            db.Connection.Close();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        //api/AttendanceTeacher/ViewTeacherAttendance
        public HttpResponseMessage ViewTeacherAttendance(AddAttendanceVM data)
        {
            string EmpName = data.EmpName;

            string SectionName = data.SectionName;

            //int CourseID = 7;
            Int32.TryParse(data.CourseID, out int CourseID);

            //int empId = id; // teacher login id

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<Attend> response;

            response = db.Query("TeacherAttendanceDetails").Where("EmpName", EmpName)
                                                    .Where("CourseID", CourseID)
                                                    .Where("SectionName", SectionName)
                                                    .Select("AttendanceDate", "ClassDuration")
                                                    .Distinct()
                                                    .Get<Attend>();
            db.Connection.Close();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        //api/AttendanceTeacher/AddStudentAttendance
        public HttpResponseMessage AddStudentAttendance(AddAttendanceVM data)
        {
            string EmpName = data.EmpName;
            string SectionName = data.SectionName;
            //int CourseID = 7;
            Int32.TryParse(data.CourseID, out int CourseID);

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            using (TransactionScope scope = new TransactionScope())
            {
                foreach (var item in data.attendances)
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

                IEnumerable<Attend> response;

                response = db.Query("TeacherAttendanceDetails").Where("EmpName", EmpName)
                                                        .Where("CourseID", CourseID)
                                                        .Where("SectionName", SectionName)
                                                        .Select("AttendanceDate", "EnrollmentID")
                                                        .Distinct()
                                                        .Get<Attend>();
                db.Connection.Close();
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }

        //DELETE COMPLETE ATTENDANCE date
        //[HttpDelete]
        //[AllowAnonymous]
        //api/AttendanceTeacher/DeleteStudentAttendance
        [HttpPost]
        public HttpResponseMessage DeleteStudentAttendance(AddAttendanceVM data)
        {
            string EmpName = data.EmpName;
            string SectionName = data.SectionName;
            //int CourseID = 7;
            Int32.TryParse(data.CourseID, out int CourseID);
            DateTime AttendanceDate = data.AttendanceDate;

            //int[] EnrollmentID;
            //int empId = id; // teacher login id

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<int> enrollmentID = db.Query("TeacherAttendanceDetails").Where("AttendanceDate", AttendanceDate)
                                                    .Where("EmpName", EmpName)
                                                    .Where("CourseID", CourseID)
                                                    .Where("SectionName", SectionName)
                                                    .Select("EnrollmentID").Distinct().Get<int>();

            foreach (int i in enrollmentID)
            {
                _ = db.Query("Attendance").Where("AttendanceDate", "=", AttendanceDate)
                                            .Where("EnrollmentID", "=", i).Delete();
            }

            IEnumerable<Attend> response;

            response = db.Query("TeacherAttendanceDetails").Where("EmpName", EmpName)
                                                    .Where("CourseID", CourseID)
                                                    .Where("SectionName", SectionName)
                                                    .Select("AttendanceDate")
                                                    .Distinct()
                                                    .Get<Attend>();
            db.Connection.Close();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        //api/AttendanceTeacher/EditStudentAttendance
        [HttpPost]
        public HttpResponseMessage EditStudentAttendance(AddAttendanceVM data)
        {
            string EmpName = data.EmpName;
            string SectionName = data.SectionName;
            Int32.TryParse(data.CourseID, out int CourseID);
            DateTime AttendanceDate = data.AttendanceDate;

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<Student> response;

            response = db.Query("TeacherAttendanceDetails").Where("AttendanceDate", AttendanceDate)
                                                    .Where("EmpName", EmpName)
                                                    .Where("CourseID", CourseID)
                                                    .Where("SectionName", SectionName)
                                                    .Select("StudentID", "SName", "RollNumber", "EnrollmentID", "AttendanceStatus", "ClassDuration", "AttendanceDate")
                                                    .Get<Student>();
            db.Connection.Close();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        //api/AttendanceTeacher/UpdateStudentAttendance
        public HttpResponseMessage UpdateStudentAttendance(AddAttendanceVM data) //List<Attend> data
        {
            string EmpName = data.EmpName;
            string SectionName = data.SectionName;
            //int CourseID = 7;
            Int32.TryParse(data.CourseID, out int CourseID);
            DateTime AttendanceDate = data.AttendanceDate;
            DateTime oldAttendanceDate = data.oldAttendanceDate;

            var db = DbUtils.GetDBConnection();
            db.Connection.Open();

            IEnumerable<int> enrollmentID = db.Query("TeacherAttendanceDetails").Where("AttendanceDate", oldAttendanceDate)
                                                    .Where("EmpName", EmpName)
                                                    .Where("CourseID", CourseID)
                                                    .Where("SectionName", SectionName)
                                                    .Select("EnrollmentID").Distinct().Get<int>();

            foreach (int i in enrollmentID)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (var item in data.attendances)
                    {
                        db.Query("Attendance").Where("EnrollmentID", i)
                                                .Where("AttendanceDate", oldAttendanceDate).Update(new
                                                {
                                                    AttendanceDate = item.AttendanceDate,
                                                    AttendanceStatus = item.AttendanceStatus,
                                                    ClassDuration = item.ClassDuration,
                                                });
                    }

                    scope.Complete();
                }
            }

            IEnumerable<Attend> response;
            response = db.Query("TeacherAttendanceDetails").Where("EmpName", EmpName)
                                                    .Where("CourseID", CourseID)
                                                    .Where("SectionName", SectionName)
                                                    .Select("AttendanceDate", "ClassDuration")
                                                    .Distinct()
                                                    .Get<Attend>();
            db.Connection.Close();
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}