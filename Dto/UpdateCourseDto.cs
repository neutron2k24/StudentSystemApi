using System.ComponentModel.DataAnnotations;

namespace StudentSystem.Dto {
    /// <summary>
    /// Defines a Data Transfer Object for updating a Courses details.
    /// </summary>
    public class UpdateCourseDto {
        [Required]
        public string? Title { get; set; }
    }
}
