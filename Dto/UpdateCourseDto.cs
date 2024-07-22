using StudentSystem.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StudentSystem.Dto {
    /// <summary>
    /// Defines a Data Transfer Object for updating a Courses details.
    /// </summary>
    public record UpdateCourseDto : RestBaseDto {
        [Required]
        public string? Title { get; set; }
    }
}
