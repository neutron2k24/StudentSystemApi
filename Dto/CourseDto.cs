using System.ComponentModel.DataAnnotations;
using StudentSystem.Models;
using StudentSystem.Interfaces;

namespace StudentSystem.Dto {
    /// <summary>
    /// Defines a Data Transfer Object for a Course.
    /// </summary>
    public record CourseDto : BasicCourseDetailDto, IRestDto {
        public int CourseId { get; set; }

        public List<LinkDto>? Links { get; set; }

        public List<EnrolledStudentDto>? EnrolledStudents { get; set; }
    }
}
