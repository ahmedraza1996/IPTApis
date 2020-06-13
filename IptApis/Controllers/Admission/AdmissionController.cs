using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using IptApis.Data;
using IptApis.DTO;
using IptApis.Models.Admission;
using Microsoft.EntityFrameworkCore;

namespace IptApis.Controllers.Admission
{
    public class AdmissionController : ApiController
    {
        private readonly AdmissionDBContext DB = new AdmissionDBContext();

        [Route("api/admission")]
        public async Task<IHttpActionResult> GetCandidates() =>
            Ok(await DB.CandidateStudent.Select(x => new CandidateStudentDto(x)).ToListAsync());

        [Route("api/admission/candidate/{id}")]
        [ResponseType(typeof(CandidateStudentDto))]
        public async Task<IHttpActionResult> GetCandidate(int id)
        {
            var Candidate = await LoadCandidateAsync(id);
            if (Candidate is null)
                return NotFound();
            else
                return Ok(Candidate);
        }

        [Route("api/admission/test/{id}")]
        [ResponseType(typeof(AdmissionTestDto))]
        public async Task<IHttpActionResult> GetTest(int id)
        {
            var Test = await DB.AdmissionTest.FindAsync(id);
            if (Test is null)
                return NotFound();
            else
                return Ok(new AdmissionTestDto(Test));
        }

        [Route("api/admission/openings")]
        [ResponseType(typeof(DTO.StudentOpening))]
        public async Task<IHttpActionResult> GetOpenings()
        {
            int Year = DateTime.Now.Year;
            var Result = await DB.StudentOpening.Where(o => o.Syear == Year).Select(o => new DTO.StudentOpening(o)).ToListAsync();
            if (Result.Count > 0)
                return Ok(Result);
            else
                throw Error("No openings available for this year.");
        }

        [Route("api/admission/openings/{id}")]
        [ResponseType(typeof(DTO.StudentOpening))]
        public async Task<IHttpActionResult> GetOpening(int id)
        {
            var Result = await DB.StudentOpening.FindAsync(id);
            if (Result is null)
                return NotFound();
            else
                return Ok(new DTO.StudentOpening(Result));
        }

        [Route("api/admission/programme/{id}")]
        [ResponseType(typeof(DTO.Programme))]
        public async Task<IHttpActionResult> GetProgramme(int id)
        {
            Models.Admission.Programme Result = await DB.Programme.FindAsync(id);
            if (Result is null)
                return NotFound();

            return Ok(new DTO.Programme(Result)
            {
                DepartmentName = (await DB.Department.FindAsync(Result.DepartmentId)).DepartmentName
            });
        }

        [HttpGet]
        [Route("api/admission/auth/{email}/{password}")]
        [ResponseType(typeof(CandidateStudentDto))]
        public async Task<IHttpActionResult> AuthenticateCandidate(string email, string password)
        {
            CandidateStudent C = await DB.CandidateStudent.Where(c => c.Email == email).FirstOrDefaultAsync();
            if (C is null)
                throw Error("No user exists with this email.");

            CandidateStudentDto Candidate = await LoadCandidateAsync(C.CandidateId);
            if (Candidate is null)
                throw Error("Candidate with email has invalid ID.");

            if (Candidate.Password == password)
                return Ok(Candidate);
            else
                throw Error("Incorrect password.");
        }

        /// <summary>
        /// Only for use as argument to CreatedAtRoute().
        /// </summary>
        [Route(Name = "GetApp")]
        public async Task<IHttpActionResult> GetApplication(int id) =>
            Ok(new CandidateApplication(await DB.ScandidateApplication.FindAsync(id)));

        [Route("api/admission/newapp/{candidateId}")]
        [ResponseType(typeof(CandidateApplication))]
        public async Task<IHttpActionResult> RegisterApplication(int candidateId, CandidateApplication application)
        {
            CandidateStudentDto Candidate = await LoadCandidateAsync(candidateId);
            if (Candidate is null)
                return NotFound();

            if (!(Candidate.Application is null))
                throw Error("Candidate already has an application.");

            var App = new ScandidateApplication
            {
                ApplyDate = application.ApplyDate,
                QualifiedStatus = application.QualifiedStatus,
                SopeningId = application.OpeningId,
                CandidateId = candidateId
            };

            Models.Admission.StudentOpening Opening = await DB.StudentOpening.FindAsync(application.OpeningId);
            if (Opening is null)
                return NotFound();

            if (application.ApplyDate > Opening.LastApplicationDate)
                throw Error("Applications can not be submitted after last date of submission.");

            int AppCount = await DB.ScandidateApplication.CountAsync(app => app.SopeningId == Opening.SopeningId && app.QualifiedStatus == "Accepted");

            if (AppCount < Opening.MaxSeats)
            {
                DB.ScandidateApplication.Add(App);
                await DB.SaveChangesAsync();
                application.RefId = App.RefId;
                return CreatedAtRoute("GetApp", new { id = App.RefId }, application);
            }
            else
                throw Error("This opening is no longer accepting applications.");
        }

        [Route("api/admission/newstudent")]
        [ResponseType(typeof(Student))]
        public async Task<IHttpActionResult> RegisterStudent(CandidateStudentDto candidate)
        {
            if (!CandidateExists(candidate))
                return NotFound();

            if (IsRegisteredStudent(candidate))
                throw Error("Candidate is already registered.");

            int Year = DateTime.Now.Year;
            string YearLast2Digits = Year.ToString().Remove(0, 2);
            string MaxRollNumber = DB.Student.Max(x => x.RollNumber) ?? YearLast2Digits + "K" + "0000";
            string ID = (Convert.ToInt32(MaxRollNumber.Remove(0, 3)) + 1).ToString().PadLeft(4, '0');
            string RollNumber = YearLast2Digits + "K" + ID;
            string Email = "k" + YearLast2Digits + ID + "@nu.edu.pk";
            int BatchId = await GetBatchId();

            var Student = new Student
            {
                Sname = candidate.Cname,
                Email = Email,
                MobileNumber = candidate.ContactInfo,
                RollNumber = RollNumber,
                CandidateId = candidate.CandidateId,
                BatchId = BatchId,
                Spassword = candidate.Password
            };

            DB.Student.Add(Student);
            await DB.SaveChangesAsync();
            return CreatedAtRoute("GetStudent", new { id = Student.CandidateId }, Student);

            async Task<int> GetBatchId()
            {
                var Batch = await DB.Batch.Where(b => b.BatchYear == Year).FirstOrDefaultAsync();
                if (Batch is null)
                {
                    Batch = new Batch { BatchName = "Batch" + YearLast2Digits, BatchYear = Year };
                    DB.Batch.Add(Batch);
                    await DB.SaveChangesAsync();
                }
                return Batch.BatchId;
            }
        }

        /// <summary>
        /// Only for use as argument to CreatedAtRoute().
        /// </summary>
        [Route(Name = "GetStudent")]
        public async Task<IHttpActionResult> GetStudent(int id) =>
            Ok(await DB.Student.FindAsync(id));

        [HttpPut]
        [Route("api/admission/update/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> UpdateCandidateDetails(int id, CandidateStudentDto candidate)
        {
            if (id != candidate.CandidateId)
                return BadRequest();

            var C = FromDTO(candidate);

            DB.Entry(C).State = EntityState.Modified;

            try
            {
                await DB.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CandidateExists(candidate))
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/admission")]
        [ResponseType(typeof(CandidateStudentDto))]
        public async Task<IHttpActionResult> RegisterCandidate(CandidateStudentDto candidate)
        {
            if (CandidateExists(candidate))
                throw Error("This email is already registered.");

            CandidateStudent C = FromDTO(candidate);

            DB.CandidateStudent.Add(C);
            await DB.SaveChangesAsync();

            candidate.CandidateId = C.CandidateId;
            return CreatedAtRoute(nameof(GetCandidate), new { id = C.CandidateId }, candidate);
        }

        private async Task<CandidateStudentDto> LoadCandidateAsync(int id)
        {
            CandidateStudent ModelCandidate = await DB.CandidateStudent.FindAsync(id);
            if (ModelCandidate is null)
                return null;

            var Candidate = new CandidateStudentDto(ModelCandidate);

            var TestDetails = await DB.CandidateTestDetails.Where(x => x.CandidateId == Candidate.CandidateId).FirstOrDefaultAsync();
            if (!(TestDetails is null))
                Candidate.TestDetails = new DTO.CandidateTestDetails(TestDetails);

            var Application = await DB.ScandidateApplication.Where(x => x.CandidateId == Candidate.CandidateId).FirstOrDefaultAsync();
            if (!(Application is null))
                Candidate.Application = new CandidateApplication(Application);

            return Candidate;
        }

        private CandidateStudent FromDTO(CandidateStudentDto candidate)
        {
            return new CandidateStudent
            {
                CandidateId = candidate.CandidateId,
                Cname = candidate.Cname,
                Email = candidate.Email,
                Password = candidate.Password,
                Caddress = candidate.Caddress,
                ContactInfo = candidate.ContactInfo,
                Hscresult = candidate.Hscresult,
                Sscresult = candidate.Sscresult,
            };
        }

        private bool CandidateExists(CandidateStudentDto candidate)
        {
            if (candidate.CandidateId > 0)
                return DB.CandidateStudent.Any(e => e.CandidateId == candidate.CandidateId);
            else
                return DB.CandidateStudent.Any(e => e.Email == candidate.Email);
        }

        private bool IsRegisteredStudent(CandidateStudentDto candidate) =>
            DB.Student.Any(e => e.CandidateId == candidate.CandidateId);

        private Exception Error(string message) => new Exception(message);
    }
}
