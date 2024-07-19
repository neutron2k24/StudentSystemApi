
using Microsoft.AspNetCore.Mvc;
using StudentSystem.Models;
using Bogus;
using StudentSystem.Enums;
using StudentSystem.Data;

namespace StudentSystem.Controllers {
    /// <summary>
    /// API End Point to generate Bogus Test Data.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BogusDataController : ControllerBase {

        private readonly ApplicationDbContext _context;
        public BogusDataController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpPost("Students")]
        public async Task<IActionResult> PopulateDummyStudents(int count) {
            for (int x = 0; x < count; x++) {
                Student s = new Faker<Student>()
                    .Ignore(s => s.StudentId)
                    .RuleFor(s => s.Gender, f => f.PickRandom<BirthGender>())
                    .RuleFor(s => s.Forename, (f, u) => f.Name.FirstName(u.Gender == BirthGender.Male ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female))
                    .RuleFor(s => s.Surname, (f, u) => f.Name.LastName(u.Gender == BirthGender.Male ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female))
                    .RuleFor(s => s.EmailAddress, (f, u) => f.Internet.Email(u.Forename, u.Surname))
                    .RuleFor(s => s.DateOfBirth, f => f.Date.Between(DateTime.Now.AddYears(-16), DateTime.Now.AddYears(-60)));
                _context.Students.Add(s);
            }
            await _context.SaveChangesAsync();
            return Ok($"{count} Random students generated");
        }


        [HttpPost("Enrollments")]
        public async Task<IActionResult> PopulateDummyEnrollments() {

            //Delete all current enrollments
            foreach(CourseEnrollment ce in _context.CourseEnrollments) {
                _context.Entry(ce).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }
            await _context.SaveChangesAsync();

            List<Student> students = _context.Students.ToList();
            List<Course> courses = _context.Courses.ToList();
 
            foreach(Student s in  students) {
                Random r = new Random();
                int generateCount = r.Next(1, 2);

                for (int x = 0; x < generateCount; x++) {
                    CourseEnrollment enrollment = new Faker<CourseEnrollment>()
                        .Ignore(e => e.CourseEnrollmentId)
                        .RuleFor(e => e.EnrollmentDate, f => f.Date.Between(DateTime.Now, DateTime.Now.AddYears(-5)))
                        .RuleFor(e => e.StudentId, s.StudentId)
                        .RuleFor(e => e.CourseId, (f, u) => f.PickRandom<Course>(courses).CourseId);

                    _context.CourseEnrollments.Add(enrollment);
                }
            }
            await _context.SaveChangesAsync();
            return Ok("Enrollments Generated for students and courses.");
        }


    }
}
