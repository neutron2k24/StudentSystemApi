using System.ComponentModel.DataAnnotations;
using StudentSystem.Enums;

namespace StudentSystem.Dto {
    /// <summary>
    /// Defines a Data Transfer Object for an existing student.
    /// Inherits properties from StudentDetailDto
    /// </summary>
    public class StudentDto : BasicStudentDetailDto {

        public int StudentId { get; set; }

        public virtual List<StudentCourseEnrollmentDto>? CourseEnrollments { get; set; }
    }
}
