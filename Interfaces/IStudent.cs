using StudentSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace StudentSystem.Interfaces {
    /// <summary>
    /// Define interface for a student
    /// </summary>
    public interface IStudent {

        /// <summary>
        /// Students First Name
        /// </summary>
        string? Forename { get; set; }

        /// <summary>
        /// Students Last Name
        /// </summary>
        string? Surname { get; set; }

        
        /// <summary>
        /// Students Date of Birth
        /// </summary>
        DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Students Email Address
        /// </summary>
        string? EmailAddress { get; set; }

        /// <summary>
        /// Students Birth Gender
        /// </summary>
        BirthGender? Gender { get; set; }
    }
}
