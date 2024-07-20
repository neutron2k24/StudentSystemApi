using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentSystem.Configuration;
using StudentSystem.Data;
using StudentSystem.Dto;
using StudentSystem.Interfaces;
using StudentSystem.Models;
using StudentSystem.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseEnrollmentsController : ControllerBase {
        //Common error messages
        private const string GENERIC_SERVER_ERROR_MESSAGE = "An internal server error occured whilst attempting to process the Course Enrollment details";

        //Application db context
        private readonly ApplicationDbContext _dbContext;
        private readonly IDtoMappingService _dtoMappingService;
        private readonly IDtoGeneratorService _dtoGeneratorService;

        public CourseEnrollmentsController(ApplicationDbContext dbContext, IDtoMappingService dtoMappingService, IDtoGeneratorService dtoGeneratorService) {
            _dbContext = dbContext;
            _dtoMappingService = dtoMappingService;
            _dtoGeneratorService = dtoGeneratorService;
        }

        /// <summary>
        /// Retrieve a list of all Course Enrollments from the database
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedCollectionResultDto<CourseEnrollmentDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetCourseEnrollments(int pageIndex = 0, int pageSize = AppSettings.DEFAULT_PAGE_SIZE) {
            IQueryable<CourseEnrollment> query = _dbContext.CourseEnrollments;

            //Count all matching records before paging.
            int totalCount = await query.CountAsync();
            if (pageSize > 0) query = query.Skip(pageIndex * pageSize).Take(pageSize).AsQueryable();
            
            List<CourseEnrollment> enrollments = await query.Include(c => c.Course).Include(c => c.Student).ToListAsync();
            if (enrollments != null && enrollments.Count > 0) { 
                List<CourseEnrollmentDto> courseEnrollmentDtos = new List<CourseEnrollmentDto>();
                enrollments.ForEach(enrollment => {
                    CourseEnrollmentDto? courseEnrollmentDto = _dtoGeneratorService.GetCourseEnrollmentDtoForCourseEnrollmentEntity(enrollment);
                    if (courseEnrollmentDto != null) courseEnrollmentDtos.Add(courseEnrollmentDto);
                });
                return Ok(_dtoGeneratorService.GetPagedCollectionResultDto(pageIndex, pageSize, totalCount, courseEnrollmentDtos));
            }
            return Ok(null);
        }

        /// <summary>
        /// Retrieve a list of all Enrolled Students from the database for a specific course.
        /// </summary>
        [HttpGet("Course/{courseId}")]
        [ProducesResponseType(typeof(List<EnrolledStudentDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetCourseEnrollmentsForCourse(int courseId) {
            List<CourseEnrollment> enrollments = await _dbContext.CourseEnrollments.Where(e => e.CourseId == courseId).Include(e => e.Course).Include(e => e.Student).ToListAsync();
            if (enrollments != null && enrollments.Count > 0) {
                List<EnrolledStudentDto> enrolledStudents = new List<EnrolledStudentDto>();
                enrollments.ForEach(enrollment => {
                    EnrolledStudentDto? endrolledStudentDto = _dtoGeneratorService.GetEnrolledStudentDtoForCourseEnrollmentEntity(enrollment);
                    if (endrolledStudentDto != null) enrolledStudents.Add(endrolledStudentDto);
                });
                return Ok(enrolledStudents);
            }
            return Ok(null);
        }

        /// <summary>
        /// Retrieve a list of all Course Enrollments for a student from the database.
        /// </summary>
        [HttpGet("Student/{studentId}")]
        [ProducesResponseType(typeof(List<StudentCourseEnrollmentDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetCourseEnrollmentsForStudent(int studentId) {
            List<CourseEnrollment> enrollments = await _dbContext.CourseEnrollments.Where(e => e.StudentId == studentId).Include(e => e.Course).Include(e => e.Student).ToListAsync();
            if (enrollments != null && enrollments.Count > 0) {
                List<StudentCourseEnrollmentDto> enrolledCourses = new List<StudentCourseEnrollmentDto>();
                enrollments.ForEach(enrollment => {
                    StudentCourseEnrollmentDto? studentCourseEnrollmentDto = _dtoGeneratorService.GetStudentCourseEnrollmentDtoForCourseEnrollmentEntity(enrollment);
                    if (studentCourseEnrollmentDto != null) enrolledCourses.Add(studentCourseEnrollmentDto);
                });
                return Ok(enrolledCourses);
            }
            return Ok(null);
        }


        /// <summary>
        /// Retrieve a specified Course Enrollment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CourseEnrollmentDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCourseEnrollment(int id) {
            CourseEnrollment? courseEnrollment = await _dbContext.CourseEnrollments.FindAsync(id);
            if (courseEnrollment != null) {
                //Load related entities.
                _dbContext.Entry(courseEnrollment).Reference(ce => ce.Course).Load();
                _dbContext.Entry(courseEnrollment).Reference(ce => ce.Student).Load();

                return Ok(_dtoGeneratorService.GetCourseEnrollmentDtoForCourseEnrollmentEntity(courseEnrollment));
            }
            return NotFound("No matching course was found for the specified id.");
        }

        /// <summary>
        /// Create a new course Enrollment
        /// </summary>
        /// <param name="courseEnrollment"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CourseEnrollmentDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> CreateCourseEnrollment([FromBody] BasicCourseEnrollmentDetailDto basicCourseEnrollmentDetailDto) {
            if (ModelState.IsValid) {
                //Check that we do not have a conflicting enrollment
                if (_dbContext.CourseEnrollments.Where(ce => ce.StudentId == basicCourseEnrollmentDetailDto.StudentId && ce.CourseId == basicCourseEnrollmentDetailDto.CourseId).Count() == 0) {
                    //Create a new course enrollment using DTO properties and current DateTime
                    CourseEnrollment courseEnrollment = new CourseEnrollment();
                    _dtoMappingService.MapBasicCourseEnrollmentDetailDtoToCourseEnrollmentEntity(basicCourseEnrollmentDetailDto, courseEnrollment);
                    courseEnrollment.EnrollmentDate = DateTime.Now;

                    _dbContext.CourseEnrollments.Add(courseEnrollment);
                    await _dbContext.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetCourseEnrollment), new { id = courseEnrollment.CourseEnrollmentId }, courseEnrollment);
                }
                else {
                    return Conflict("The specified student is already enrolled on the specified course.");
                }
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Delete a specific Course Enrollment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCourseEnrollment(int id) {
            //Ensure we have a valid course for the specified id
            CourseEnrollment? courseEnrollment = await _dbContext.CourseEnrollments.FindAsync(id);
            if (courseEnrollment == null) return NotFound("The specified course enrollment could not be found.");

            _dbContext.Remove(courseEnrollment);
            return await _dbContext.SaveChangesAsync() > 0 ? Ok("Course Enrollment was deleted.") : this.Problem(GENERIC_SERVER_ERROR_MESSAGE, statusCode: 500);
        }
    }
}
