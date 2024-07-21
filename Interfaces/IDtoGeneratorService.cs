using Microsoft.AspNetCore.Mvc;
using StudentSystem.Dto;
using StudentSystem.Models;

namespace StudentSystem.Interfaces {
    /// <summary>
    /// Defines an Interface for a Data Transfer Object Generator service.
    /// Generates DTOs from specified Entities.
    /// </summary>
    public interface IDtoGeneratorService {
        /// <summary>
        /// Get a CourseDto for a specified course entity.
        /// </summary>
        CourseDto? GetCourseDtoForCourseEntity(Course course, IUrlHelper? urlHelper, bool includeEnrollments = false);

        /// <summary>
        /// Map a Course Enrollment Entity to a new CourseEnrollmentDto.
        /// </summary>
        CourseEnrollmentDto? GetCourseEnrollmentDtoForCourseEnrollmentEntity(CourseEnrollment courseEnrollment, IUrlHelper? urlHelper);

        /// <summary>
        /// Map a Course Enrollment Entity to a new EnrolledStudentDto.
        /// </summary>
        EnrolledStudentDto? GetEnrolledStudentDtoForCourseEnrollmentEntity(CourseEnrollment courseEnrollment, IUrlHelper? urlHelper);

        /// <summary>
        /// Map a Course Enrollment Entity to a new StudentCourseEnrollmentDto.
        /// </summary>
        StudentCourseEnrollmentDto? GetStudentCourseEnrollmentDtoForCourseEnrollmentEntity(CourseEnrollment courseEnrollment, IUrlHelper? urlHelper);

        /// <summary>
        /// Map Student Entity properties to a new StudentDto object.
        /// </summary>
        StudentDto? GetStudentDtoForStudentEntity(Student student, IUrlHelper? urlHelper, bool includeEnrollments = false);

        /// <summary>
        /// Generate a PagedCollectionResultDto object
        /// </summary>
        PagedCollectionResultDto<T> GetPagedCollectionResultDto<T>(string controllerRoute, int pageIndex, int pageSize, int totalCount, List<T> results, IUrlHelper? urlHelper);
    }
}
