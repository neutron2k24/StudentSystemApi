
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentSystem.Configuration;
using StudentSystem.Data;
using StudentSystem.Dto;
using StudentSystem.Enums;
using StudentSystem.Interfaces;
using StudentSystem.Models;
using StudentSystem.Services;

namespace StudentSystem.Controllers
{
    /// <summary>
    /// API Controller for Course data.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase {

        //Common error messages
        private const string GENERIC_SERVER_ERROR_MESSAGE = "An internal server error occured whilst attempting to process the Course details";

        //Application db context
        private readonly ApplicationDbContext _dbContext;
        private readonly IDtoMappingService _dtoMappingService;
        private readonly IDtoGeneratorService _dtoGeneratorService;

        public CoursesController(ApplicationDbContext dbContext, IDtoMappingService dtoMappingService, IDtoGeneratorService dtoGeneratorService) {
            _dbContext = dbContext;
            _dtoMappingService = dtoMappingService;
            _dtoGeneratorService = dtoGeneratorService;
        }


        /// <summary>
        /// Apply specified Sort Direct and Field to query
        /// </summary>
        private void ApplySortOrder(ref IQueryable<Course> query, CourseSortField sortBy, SortDirection sortDir) {
            switch (sortBy) {
                default:
                case CourseSortField.Title:
                    query = sortDir == SortDirection.Ascending ? query.OrderBy(c => c.Title) : query.OrderByDescending(c => c.Title);
                    break;
                case CourseSortField.Qualification:
                    query = sortDir == SortDirection.Ascending ? query.OrderBy(c => c.Qualification) : query.OrderByDescending(c => c.Qualification);
                    break;
            }
        }


        /// <summary>
        /// Function to retrieve courses.
        /// </summary>
        /// <returns></returns>
        private async Task<PagedCollectionResultDto<CourseDto>?> GetCourses(int pageIndex, int pageSize, CourseSortField sortBy, SortDirection sortDir, string? courseTitleSearch, bool includeStudents = false) {
            IQueryable<Course> query = _dbContext.Courses;
            ApplySortOrder(ref query, sortBy, sortDir);

            if (!string.IsNullOrEmpty(courseTitleSearch)) query = query.Where(c => c.Title != null && c.Title.ToLower().StartsWith(courseTitleSearch.ToLower()));

            //Count all matching records before paging.
            int totalCount = await query.CountAsync();

            if (pageSize > 0) query = query.Skip(pageIndex * pageSize).Take(pageSize).AsQueryable();

            List<Course>? courses;

            if (includeStudents)
                courses = await query.Include(c => c.Enrollments).ThenInclude(en => en.Student).ToListAsync();
            else
                courses = await query.ToListAsync();

            if (courses != null) {
                List<CourseDto> courseDtos = new List<CourseDto>();
                courses.ForEach(course => {
                    CourseDto? courseDto = _dtoGeneratorService.GetCourseDtoForCourseEntity(course, this.Url, includeStudents);
                    if (courseDto != null) courseDtos.Add(courseDto);
                });

                return _dtoGeneratorService.GetPagedCollectionResultDto("Courses", pageIndex, pageSize, totalCount, courseDtos, this.Url);
            }
            return null;
        }


        [HttpGet]
        [ProducesResponseType(typeof(PagedCollectionResultDto<CourseDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetCourses(int pageIndex = 0, int pageSize = AppSettings.DEFAULT_PAGE_SIZE, CourseSortField sortBy = CourseSortField.Title, SortDirection sortDir = SortDirection.Ascending, bool includeStudents = false) {
            return Ok(await GetCourses(pageIndex, pageSize, sortBy, sortDir, null, includeStudents));
        }

        [HttpGet("search/{courseTitle}")]
        [ProducesResponseType(typeof(PagedCollectionResultDto<CourseDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetCoursesBySearch(string courseTitle, int pageIndex = 0, int pageSize = AppSettings.DEFAULT_PAGE_SIZE, CourseSortField sortBy = CourseSortField.Title, SortDirection sortDir = SortDirection.Ascending, bool includeStudents = false) {

            PagedCollectionResultDto<CourseDto>? result = await GetCourses(pageIndex, pageSize, sortBy, sortDir, courseTitle, includeStudents);
            return result != null ? Ok(result) : NotFound("No courses found for the specified search criteria.");
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CourseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCourse(int id, bool includeStudents = false) {
            Course? course = includeStudents ? await _dbContext.Courses.Where(c => c.CourseId == id).Include(c => c.Enrollments).ThenInclude(en => en.Student).SingleOrDefaultAsync() : await _dbContext.Courses.FindAsync(id);

            if (course != null) {
                return Ok(_dtoGeneratorService.GetCourseDtoForCourseEntity(course, this.Url, includeStudents));
            }
            return NotFound("No matching course was found for the specified id.");
        }


        [HttpPost]
        [ProducesResponseType(typeof(CourseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreateCourse([FromBody] BasicCourseDetailDto basicCourseDetailDto) {
            if (ModelState.IsValid) {
                Course course = new Course();

                //Map CourseDto propertiers to Course
                _dtoMappingService.MapBasicCourseDetailDtoToCourseEntity(basicCourseDetailDto, course);
                _dbContext.Courses.Add(course);
                
                return await _dbContext.SaveChangesAsync() > 0 ? 
                    CreatedAtAction(nameof(GetCourse), new { id = course.CourseId }, course) : 
                    Problem(GENERIC_SERVER_ERROR_MESSAGE, statusCode: 500);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CourseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] BasicCourseDetailDto basicCourseDetailDto) {
            if (ModelState.IsValid) {
                //Ensure we have a valid course for the specified id
                Course? course = await _dbContext.Courses.FindAsync(id);
                if (course == null) return NotFound("The specified course could not be found. Update failed.");

                //Map CourseDto propertiers to Course
                _dtoMappingService.MapBasicCourseDetailDtoToCourseEntity(basicCourseDetailDto, course);

                //Detatch and update
                _dbContext.Entry(course).State = EntityState.Modified;
                return await _dbContext.SaveChangesAsync() > 0 ? 
                    Ok(_dtoGeneratorService.GetCourseDtoForCourseEntity(course, this.Url, true))
                    : Problem(GENERIC_SERVER_ERROR_MESSAGE, statusCode: 500);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCourse(int id) {
            //Ensure we have a valid course for the specified id
            Course? course = await _dbContext.Courses.FindAsync(id);
            if (course == null) return NotFound("The specified course could not be found. Delete failed.");

            _dbContext.Remove(course);
            return await _dbContext.SaveChangesAsync() > 0 ? 
                Ok("Course was deleted.") : 
                this.Problem(GENERIC_SERVER_ERROR_MESSAGE, statusCode: 500);
        }
    }
}
