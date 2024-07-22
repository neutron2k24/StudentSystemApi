using StudentSystem.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace StudentSystem.Dto {
    public record CourseEnrollmentDto : BasicCourseEnrollmentDetailDto, IRestResponseDto {
        public int CourseEnrollmentId { get; set; }

        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        public CourseDto? Course { get; set; }
        public StudentDto? Student { get; set; }

        public List<LinkDto>? Links { get; set; }
    }
}
