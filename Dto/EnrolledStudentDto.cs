using StudentSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace StudentSystem.Dto {
    /// <summary>
    /// Defines a Data Transfer Object for a Student Enrolled on a Course.
    /// Only includes the student entity for a CourseEnrollment.
    /// </summary>
    public class EnrolledStudentDto {

        public int CourseEnrollmentId { get; set; }

        public DateTime EnrollmentDate { get; set; }
        public StudentDto? Student { get; set; }
    }
}
