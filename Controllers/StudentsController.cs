using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentSystem.Configuration;
using StudentSystem.Data;
using StudentSystem.Dto;
using StudentSystem.Interfaces;
using StudentSystem.Models;
using StudentSystem.Services;

namespace StudentSystem.Controllers
{

    /// <summary>
    /// API Controller for Students
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase {

        //Common error messages
        private const string GENERIC_SERVER_ERROR_MESSAGE = "An internal server error occured whilst attempting to process the Students details";

        //Application db context
        private readonly ApplicationDbContext _dbContext;
        private readonly IDtoMappingService _dtoMappingService;

        public StudentsController(ApplicationDbContext dbContext, IDtoMappingService dtoMappingService) {
            if (dbContext == null || dtoMappingService == null) throw new Exception("ApplicationContext and DtoMapping Service must be configured.");
            _dbContext = dbContext;
            _dtoMappingService = dtoMappingService;
        }

        /// <summary>
        /// Retrieve a list of all students in the database.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<StudentDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetStudents(int pageIndex = 0, int pageSize = AppSettings.DEFAULT_PAGE_SIZE, bool includeEnrolledCourses = false) {
            IQueryable query = _dbContext.Students.Skip(pageIndex * pageSize).Take(pageSize);
            List<Student>? students;

            if (includeEnrolledCourses)
                students = await _dbContext.Students.Include(s => s.Enrollments).ThenInclude(en => en.Course).ToListAsync();
            else
                students = await _dbContext.Students.ToListAsync();

            if (students != null) {
                List<StudentDto> studentDtos = new List<StudentDto>();
                students.ForEach(student => {
                    StudentDto? studentDto = _dtoMappingService.GetStudentDtoForStudentEntity(student, includeEnrolledCourses);
                    if (studentDto != null) studentDtos.Add(studentDto);
                });

                return Ok(studentDtos);
            }
            return Ok(null);
        }


        /// <summary>
        /// Retrieve a list of all students with surname search
        /// </summary>
        [HttpGet("search/{surname}")]
        [ProducesResponseType(typeof(List<StudentDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetStudentsBySurnameSearch(string surname, int pageIndex = 0, int pageSize = AppSettings.DEFAULT_PAGE_SIZE, bool includeEnrolledCourses = false) {
            IQueryable query = _dbContext.Students.Where(s => s.Surname != null && s.Surname.ToLower().StartsWith(surname.ToLower())).Skip(pageIndex * pageSize).Take(pageSize);
            List<Student>? students;

            if (includeEnrolledCourses)
                students = await _dbContext.Students.Include(s => s.Enrollments).ThenInclude(en => en.Course).ToListAsync();
            else
                students = await _dbContext.Students.ToListAsync();
            if (students != null) {
                List<StudentDto> studentDtos = new List<StudentDto>();
                students.ForEach(student => {
                    StudentDto? studentDto = _dtoMappingService.GetStudentDtoForStudentEntity(student, includeEnrolledCourses);
                    if (studentDto != null) studentDtos.Add(studentDto);
                });

                return Ok(studentDtos);
            }
            return NotFound("No students found for the surname specified.");
        }

        /// <summary>
        /// Retrieve a specified Student by id.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudentDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStudent(int id, bool includeEnrolledCourses = false) {
            Student? student = includeEnrolledCourses ? await _dbContext.Students.Where(s => s.StudentId == id).Include(s => s.Enrollments).ThenInclude(en => en.Course).SingleOrDefaultAsync() : await _dbContext.Students.FindAsync(id);
            if (student != null) {
                return Ok(_dtoMappingService.GetStudentDtoForStudentEntity(student, includeEnrolledCourses));
            }
            return NotFound("No matching student was found for the specified id.");
        }

        /// <summary>
        /// Create a new student.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Student), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateStudent([FromBody] BasicStudentDetailDto basicStudentDetailDto) {
            if (ModelState.IsValid) {
                Student student = new Student();

                //Map DTO to Entity.
                _dtoMappingService.MapBasicStudentDetailDtoToStudentEntity(basicStudentDetailDto, student);

                _dbContext.Students.Add(student);
                return await _dbContext.SaveChangesAsync() > 0 ?
                    CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student) : 
                    Problem(GENERIC_SERVER_ERROR_MESSAGE, statusCode: 500);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Create a new student.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(StudentDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] BasicStudentDetailDto basicStudentDetailDto) {
            if (ModelState.IsValid) {
                //Ensure we have a valid student for the specified id
                Student? student = await _dbContext.Students.FindAsync(id);
                if (student == null) return NotFound("The specified student could not be found. Update failed.");

                //Map DTO to Entity
                _dtoMappingService.MapBasicStudentDetailDtoToStudentEntity(basicStudentDetailDto, student);

                //Detatch and update
                _dbContext.Entry(student).State = EntityState.Modified;

                return await _dbContext.SaveChangesAsync() > 0 ? 
                    Ok(_dtoMappingService.GetStudentDtoForStudentEntity(student, true)) : 
                    Problem(GENERIC_SERVER_ERROR_MESSAGE, statusCode: 500);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Delete the specified student.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> DeleteStudent(int id) {
            //Ensure we have a valid student for the specified id
            Student? student = await _dbContext.Students.FindAsync(id);
            if (student == null) return NotFound("The specified student could not be found. Update failed.");
            _dbContext.Remove(student);
            return await _dbContext.SaveChangesAsync() > 0 ? 
                Ok("Student was deleted.") : 
                Problem(GENERIC_SERVER_ERROR_MESSAGE, statusCode: 500);
        }

       
    }

}
