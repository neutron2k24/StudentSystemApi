using System.ComponentModel.DataAnnotations;
using StudentSystem.Models;
using StudentSystem.Interfaces;

namespace StudentSystem.Dto {
    /// <summary>
    /// Defines a Data Transfer Object for a Course.
    /// </summary>
    public class CourseDto : BasicCourseDetailDto {
        public int CourseId { get; set; }

        public List<EnrolledStudentDto>? EnrolledStudents { get; set; }
    }
}
