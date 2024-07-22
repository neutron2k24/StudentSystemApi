using System.ComponentModel.DataAnnotations;
using StudentSystem.Enums;
using StudentSystem.Interfaces;

namespace StudentSystem.Dto {
    /// <summary>
    /// Defines a Data Transfer Object for an existing student.
    /// Inherits properties from StudentDetailDto
    /// </summary>
    public record StudentDto : BasicStudentDetailDto, IRestResponseDto {

        public int StudentId { get; set; }

        public List<LinkDto>? Links { get; set; }

        public virtual List<StudentCourseEnrollmentDto>? CourseEnrollments { get; set; }
    }
}
