using StudentSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace StudentSystem.Dto {
    public class CourseEnrollmentDto : BasicCourseEnrollmentDetailDto {
        public int CourseEnrollmentId { get; set; }

        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        public CourseDto? Course { get; set; }
        public StudentDto? Student { get; set; }
    }
}
