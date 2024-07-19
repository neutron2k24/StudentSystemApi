using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using StudentSystem.Enums;
using StudentSystem.Interfaces;

namespace StudentSystem.Models {

    /// <summary>
    /// Defines a student
    /// </summary>
    public class Student : IStudent {
        public int StudentId { get; set; }

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

        public virtual List<CourseEnrollment> Enrollments { get; set; } = new();
    }
}
