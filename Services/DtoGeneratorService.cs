using StudentSystem.Dto;
using StudentSystem.Interfaces;
using StudentSystem.Models;
using Microsoft.AspNetCore.Mvc;
using StudentSystem.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http.Extensions;

namespace StudentSystem.Services {
    public class DtoGeneratorService : IDtoGeneratorService {

        /// <summary>
        /// Access the current  HttpContext via D.I.
        /// </summary>
        private readonly HttpContext? _httpContext;

        public DtoGeneratorService(IHttpContextAccessor httpContextAccessor) {
            if (httpContextAccessor != null && httpContextAccessor.HttpContext != null) {
                _httpContext = httpContextAccessor.HttpContext;
            }
        }

        #region StudentDto Generation

        /// <summary>
        /// Map Student Entity properties to a new StudentDto object.
        /// </summary>
        /// <param name="student"></param>
        /// <param name="student"></param>
        public StudentDto? GetStudentDtoForStudentEntity(Student student, IUrlHelper? urlHelper, bool includeEnrollments = false) {
            if (student == null) return null;
            StudentDto studentDto = new StudentDto() {
                StudentId = student.StudentId,
                Forename = student.Forename,
                Surname = student.Surname,
                DateOfBirth = student.DateOfBirth,
                EmailAddress = student.EmailAddress,
                Gender = student.Gender
            };

            //Add HATEOAS links.
            if (urlHelper != null) {
                studentDto.Links = new List<LinkDto>();
                string scheme = _httpContext != null ? _httpContext.Request.Scheme : "";
                studentDto.Links.Add(new LinkDto() {
                    Href = urlHelper.Action(nameof(StudentsController.GetStudents), "Students", new { id = student.StudentId, includeEnrolledCourses = includeEnrollments }, scheme),
                    Rel = "item",
                    Type = "GET"
                });
                studentDto.Links.Add(new LinkDto() {
                    Href = urlHelper.Action(nameof(StudentsController.CreateStudent), "Students", null, scheme),
                    Rel = "create",
                    Type = "POST"
                });
                studentDto.Links.Add(new LinkDto() {
                    Href = urlHelper.Action(nameof(StudentsController.UpdateStudent), "Students", new { id = student.StudentId }, scheme),
                    Rel = "update",
                    Type = "PUT"
                });
                studentDto.Links.Add(new LinkDto() {
                    Href = urlHelper.Action(nameof(StudentsController.DeleteStudent), "Students", new { id = student.StudentId }, scheme),
                    Rel = "delete",
                    Type = "DELETE"
                });
            }

            //Populate enrollments
            if (includeEnrollments && student.Enrollments != null && student.Enrollments.Count > 0) {
                studentDto.CourseEnrollments = new List<StudentCourseEnrollmentDto>();
                student.Enrollments.ForEach(enrollment => {
                    StudentCourseEnrollmentDto? courseEnrollmentDto = GetStudentCourseEnrollmentDtoForCourseEnrollmentEntity(enrollment, urlHelper);
                    if (courseEnrollmentDto != null) studentDto.CourseEnrollments.Add(courseEnrollmentDto);
                });
            }
            else {
                studentDto.CourseEnrollments = null;
            }
            return studentDto;
        }

        #endregion

        #region CourseDto Generation

        /// <summary>
        /// Get a CourseDto for a specified course entity.
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public CourseDto? GetCourseDtoForCourseEntity(Course course, IUrlHelper? urlHelper, bool includeEnrollments = false) {
            if (course == null) return null;
            var courseDto = new CourseDto() {
                CourseId = course.CourseId,
                Title = course.Title,
                Qualification = course.Qualification,
            };

            //Add HATEOAS links.
            if (urlHelper != null) {
                courseDto.Links = new List<LinkDto>();
                string scheme = _httpContext != null ? _httpContext.Request.Scheme : "";
                courseDto.Links.Add(new LinkDto() {
                    Href = urlHelper.Action(nameof(CoursesController.GetCourse), "Courses", new { id = course.CourseId, includeStudents = includeEnrollments }, scheme),
                    Rel = "item",
                    Type = "GET"
                });
                courseDto.Links.Add(new LinkDto() {
                    Href = urlHelper.Action(nameof(CoursesController.CreateCourse), "Courses", null, scheme),
                    Rel = "create",
                    Type = "POST"
                });
                courseDto.Links.Add(new LinkDto() {
                    Href = urlHelper.Action(nameof(CoursesController.UpdateCourse), "Courses", new { id = course.CourseId }, scheme),
                    Rel = "update",
                    Type = "PUT"
                });
                courseDto.Links.Add(new LinkDto() {
                    Href = urlHelper.Action(nameof(CoursesController.DeleteCourse), "Courses", new { id = course.CourseId }, scheme),
                    Rel = "delete",
                    Type = "DELETE"
                });
            }

            //Include student enrollments?
            if (includeEnrollments && course.Enrollments != null && course.Enrollments.Count > 0) {
                courseDto.EnrolledStudents = new List<EnrolledStudentDto>();
                course.Enrollments.ForEach(enrollment => {
                    EnrolledStudentDto? enrolledStudentDto = GetEnrolledStudentDtoForCourseEnrollmentEntity(enrollment, urlHelper);
                    if (enrolledStudentDto != null) courseDto.EnrolledStudents.Add(enrolledStudentDto);
                });
            }

            return courseDto;
        }

        #endregion

        #region CourseEnrollmentDto Generation

        /// <summary>
        /// Map a Course Enrollment Entity to a new CourseEnrollmentDto.
        /// </summary>
        /// <param name="courseEnrollment"></param>
        public CourseEnrollmentDto? GetCourseEnrollmentDtoForCourseEnrollmentEntity(CourseEnrollment courseEnrollment, IUrlHelper? urlHelper) {
            if (courseEnrollment == null || courseEnrollment.Student == null || courseEnrollment.Course == null) return null;
            CourseEnrollmentDto courseEnrollmentDto = new CourseEnrollmentDto() {
                CourseEnrollmentId = courseEnrollment.CourseEnrollmentId,
                EnrollmentDate = courseEnrollment.EnrollmentDate,
                StudentId = courseEnrollment.StudentId,
                CourseId = courseEnrollment.CourseId,
                Student = GetStudentDtoForStudentEntity(courseEnrollment.Student, urlHelper, false),
                Course = GetCourseDtoForCourseEntity(courseEnrollment.Course, urlHelper, false)
            };

            //Add HATEOAS links.
            if (urlHelper != null) {
                courseEnrollmentDto.Links = new List<LinkDto>();
                string scheme = _httpContext != null ? _httpContext.Request.Scheme : "";
                courseEnrollmentDto.Links.Add(new LinkDto() {
                    Href = urlHelper.Action(nameof(CourseEnrollmentsController.GetCourseEnrollment), "CourseEnrollments", new { id = courseEnrollment.CourseEnrollmentId }, scheme),
                    Rel = "item",
                    Type = "GET"
                });
                courseEnrollmentDto.Links.Add(new LinkDto() {
                    Href = urlHelper.Action(nameof(CourseEnrollmentsController.CreateCourseEnrollment), "CourseEnrollments", null, scheme),
                    Rel = "create",
                    Type = "POST"
                });
                courseEnrollmentDto.Links.Add(new LinkDto() {
                    Href = urlHelper.Action(nameof(CourseEnrollmentsController.DeleteCourseEnrollment), "CourseEnrollments", new { id = courseEnrollment.CourseEnrollmentId }, scheme),
                    Rel = "delete",
                    Type = "DELETE"
                });
            }

            return courseEnrollmentDto;
        }

        #endregion

        #region EnrolledStudentDto Generation

        /// <summary>
        /// Map a Course Enrollment Entity to a new EnrolledStudentDto.
        /// </summary>
        /// <param name="courseEnrollment"></param>
        public EnrolledStudentDto? GetEnrolledStudentDtoForCourseEnrollmentEntity(CourseEnrollment courseEnrollment, IUrlHelper? urlHelper) {
            if (courseEnrollment == null || courseEnrollment.Student == null) return null;
            EnrolledStudentDto enrolledStudentDto = new EnrolledStudentDto() {
                CourseEnrollmentId = courseEnrollment.CourseEnrollmentId,
                EnrollmentDate = courseEnrollment.EnrollmentDate,
                Student = GetStudentDtoForStudentEntity(courseEnrollment.Student, urlHelper)
            };

            return enrolledStudentDto;
        }

        #endregion

        #region StudentCourseEnrollmentDto Generation

        /// <summary>
        /// Map a Course Enrollment Entity to a new StudentCourseEnrollmentDto.
        /// </summary>
        /// <param name="courseEnrollment"></param>
        public StudentCourseEnrollmentDto? GetStudentCourseEnrollmentDtoForCourseEnrollmentEntity(CourseEnrollment courseEnrollment, IUrlHelper? urlHelper) {
            if (courseEnrollment == null || courseEnrollment.Course == null) return null;
            StudentCourseEnrollmentDto studentCourseEnrollmentDto = new StudentCourseEnrollmentDto() {
                CourseEnrollmentId = courseEnrollment.CourseEnrollmentId,
                EnrollmentDate = courseEnrollment.EnrollmentDate,
                Course = GetCourseDtoForCourseEntity(courseEnrollment.Course, urlHelper, false)
            };

            return studentCourseEnrollmentDto;
        }

        #endregion

        #region PagedResultDto Generation

        /// <summary>
        /// Generate a PagedCollectionResultDto object
        /// </summary>
        public PagedCollectionResultDto<T> GetPagedCollectionResultDto<T>(string controllerRoute, int pageIndex, int pageSize, int totalCount, List<T> results, IUrlHelper? urlHelper) {
            PagedCollectionResultDto<T> pagedResultDto = new PagedCollectionResultDto<T>() {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                ResultCount = results.Count,
                PageCount = pageSize > 0 ? Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalCount) / pageSize)) : 1,
                Results = results
            };

            if (urlHelper != null) {
                string scheme = _httpContext != null ? _httpContext.Request.Scheme : "";
                pagedResultDto.Links = new List<LinkDto>() { 
                    new LinkDto {
                        Href = urlHelper.Action(null, controllerRoute, null, scheme),
                        Rel = "self",
                        Type = "GET"
                    }
                };
            }

            return pagedResultDto;
        }

        #endregion


    }
}
