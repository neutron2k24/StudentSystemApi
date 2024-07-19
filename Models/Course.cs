using StudentSystem.Interfaces;
using System.ComponentModel.DataAnnotations;
namespace StudentSystem.Models {

    /// <summary>
    /// Defines a Course Entity
    /// </summary>
    public class Course : ICourse {
        public int CourseId { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Qualification { get; set; }

        public List<CourseEnrollment> Enrollments { get; set; } = new();
    }
}
