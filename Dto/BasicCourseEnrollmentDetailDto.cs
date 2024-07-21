using StudentSystem.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StudentSystem.Dto
{
    /// <summary>
    /// Defines a Data Transfer Object for creating or updating a Course Enrollment Entity.
    /// </summary>
    public record BasicCourseEnrollmentDetailDto : ICourseEnrollment
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        public int StudentId { get; set; }
    }
}
