using StudentSystem.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StudentSystem.Dto
{
    /// <summary>
    /// Defines a Data Transfer Object for creating or updating a course entity.
    /// </summary>
    public record BasicCourseDetailDto : RestBaseDto, ICourse { 
        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Qualification { get; set; }
    }
}
