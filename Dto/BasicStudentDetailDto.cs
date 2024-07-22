using StudentSystem.Enums;
using StudentSystem.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StudentSystem.Dto
{
    /// <summary>
    /// Defines a Data Transfer Object for Creating or Updating a Student Entity
    /// </summary>
    public record BasicStudentDetailDto : RestBaseDto, IStudent {
        [Required]
        public string? Forename { get; set; }

        [Required]
        public string? Surname { get; set; }

        [Required]
        
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string? EmailAddress { get; set; }

        [Required]
        public BirthGender? Gender { get; set; }
    }
}
