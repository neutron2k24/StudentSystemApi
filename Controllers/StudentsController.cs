using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentSystem.Configuration;
using StudentSystem.Data;
using StudentSystem.Dto;
using StudentSystem.Enums;
using StudentSystem.Interfaces;
using StudentSystem.Models;
using StudentSystem.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        private readonly IDtoGeneratorService _dtoGeneratorService;

        public StudentsController(ApplicationDbContext dbContext, IDtoMappingService dtoMappingService, IDtoGeneratorService dtoGeneratorService) {
            if (dbContext == null || dtoMappingService == null) throw new Exception("ApplicationContext and DtoMapping Service must be configured.");
            _dbContext = dbContext;
            _dtoMappingService = dtoMappingService;
            _dtoGeneratorService = dtoGeneratorService;
        }

        /// <summary>
        /// Apply specified Sort Direct and Field to query
        /// </summary>
        private void ApplySortOrder(ref IQueryable<Student> query, StudentSortField sortBy, SortDirection sortDir) {
            switch (sortBy) {
                default:
                case StudentSortField.Surname:
                    query = sortDir == SortDirection.Ascending ? query.OrderBy(c => c.Surname) : query.OrderByDescending(c => c.Surname);
                    break;
                case StudentSortField.Forename:
                    query = sortDir == SortDirection.Ascending ? query.OrderBy(c => c.Forename) : query.OrderByDescending(c => c.Forename);
                    break;
                case StudentSortField.Email:
                    query = sortDir == SortDirection.Ascending ? query.OrderBy(c => c.EmailAddress) : query.OrderByDescending(c => c.EmailAddress);
                    break;
                case StudentSortField.DateOfBirth:
                    query = sortDir == SortDirection.Ascending ? query.OrderBy(c => c.DateOfBirth) : query.OrderByDescending(c => c.DateOfBirth);
                    break;
                case StudentSortField.Gender:
                    query = sortDir == SortDirection.Ascending ? query.OrderBy(c => c.Gender) : query.OrderByDescending(c => c.Gender);
                    break;
            }
        }


        /// <summary>
        /// Function to retrieve students and return a PageCollectionResultsDto.
        /// </summary>
        private async Task<PagedCollectionResultDto<StudentDto>?> GetStudents(int pageIndex, int pageSize, StudentSortField sortBy, SortDirection sortDir, string? surnameSearch, bool includeEnrolledCourses = false) {
            IQueryable<Student> query = _dbContext.Students;
            ApplySortOrder(ref query, sortBy, sortDir);

            if (!string.IsNullOrEmpty(surnameSearch)) query = query.Where(s => s.Surname != null && s.Surname.ToLower().StartsWith(surnameSearch.ToLower()));

            //Count all matching records before paging.
            int totalCount = await query.CountAsync();
            if (pageSize > 0) query = query.Skip(pageIndex * pageSize).Take(pageSize).AsQueryable();

            List<Student>? students;
            if (includeEnrolledCourses)
                students = await query.Include(s => s.Enrollments).ThenInclude(en => en.Course).ToListAsync();
            else
                students = await query.ToListAsync();
            if (students != null) {
                List<StudentDto> studentDtos = new List<StudentDto>();
                students.ForEach(student => {
                    StudentDto? studentDto = _dtoGeneratorService.GetStudentDtoForStudentEntity(student, this.Url, includeEnrolledCourses);
                    if (studentDto != null) studentDtos.Add(studentDto);
                });

                return _dtoGeneratorService.GetPagedCollectionResultDto("Students", pageIndex, pageSize, totalCount, studentDtos, this.Url);
            }
            return null;
        }


        /// <summary>
        /// Retrieve a list of all students in the database.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedCollectionResultDto<StudentDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetStudents(int pageIndex = 0, int pageSize = AppSettings.DEFAULT_PAGE_SIZE, StudentSortField sortBy = StudentSortField.Surname, SortDirection sortDir = SortDirection.Ascending, bool includeEnrolledCourses = false) {
            return Ok(await GetStudents(pageIndex, pageSize, sortBy, sortDir, null, includeEnrolledCourses));
        }

        /// <summary>
        /// Retrieve a list of all students with surname search
        /// </summary>
        [HttpGet("search/{surname}")]
        [ProducesResponseType(typeof(PagedCollectionResultDto<StudentDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetStudentsBySurnameSearch(string surname, int pageIndex = 0, int pageSize = AppSettings.DEFAULT_PAGE_SIZE, StudentSortField sortBy = StudentSortField.Surname, SortDirection sortDir = SortDirection.Ascending, bool includeEnrolledCourses = false) {
            PagedCollectionResultDto<StudentDto>? result = await GetStudents(pageIndex, pageSize, sortBy, sortDir, surname, includeEnrolledCourses);
            return result != null ? Ok(result) : NotFound("No students found for the surname specified.");
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
                return Ok(_dtoGeneratorService.GetStudentDtoForStudentEntity(student, this.Url, includeEnrolledCourses));
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
        /// Update a Student
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
                    Ok(_dtoGeneratorService.GetStudentDtoForStudentEntity(student, this.Url, true)) : 
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
