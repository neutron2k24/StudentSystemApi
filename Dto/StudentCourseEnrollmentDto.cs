using StudentSystem.Models;

namespace StudentSystem.Dto {
    /// <summary>
    /// Defines a Data Transfer Object for a Students Course enrollments
    /// Only includes the course entity for a CourseEnrollment.
    /// </summary>
    public record StudentCourseEnrollmentDto {

        public int CourseEnrollmentId { get; set; }

        public DateTime EnrollmentDate { get; set; }
        public CourseDto? Course { get; set; }
    }
}
