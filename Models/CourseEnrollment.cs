using StudentSystem.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StudentSystem.Models {
    /// <summary>
    /// Defines a Course Enrollement.
    /// </summary>
    public class CourseEnrollment : ICourseEnrollment {
        public int CourseEnrollmentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required] 
        public int StudentId { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public Course? Course { get; set; }
        public Student? Student { get; set; }
    }
}
