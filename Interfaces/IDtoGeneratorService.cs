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
        CourseDto? GetCourseDtoForCourseEntity(Course course, bool includeEnrollments = false);

        /// <summary>
        /// Map a Course Enrollment Entity to a new CourseEnrollmentDto.
        /// </summary>
        CourseEnrollmentDto? GetCourseEnrollmentDtoForCourseEnrollmentEntity(CourseEnrollment courseEnrollment);

        /// <summary>
        /// Map a Course Enrollment Entity to a new EnrolledStudentDto.
        /// </summary>
        EnrolledStudentDto? GetEnrolledStudentDtoForCourseEnrollmentEntity(CourseEnrollment courseEnrollment);

        /// <summary>
        /// Map a Course Enrollment Entity to a new StudentCourseEnrollmentDto.
        /// </summary>
        StudentCourseEnrollmentDto? GetStudentCourseEnrollmentDtoForCourseEnrollmentEntity(CourseEnrollment courseEnrollment);

        /// <summary>
        /// Map Student Entity properties to a new StudentDto object.
        /// </summary>
        StudentDto? GetStudentDtoForStudentEntity(Student student, bool includeEnrollments = false);

        /// <summary>
        /// Generate a PagedCollectionResultDto object
        /// </summary>
        PagedCollectionResultDto<T> GetPagedCollectionResultDto<T>(int pageIndex, int pageSize, int totalCount, List<T> results);
    }
}
