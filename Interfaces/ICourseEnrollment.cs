namespace StudentSystem.Interfaces {
    /// <summary>
    /// Define Interface for a Student Course Enrollment
    /// </summary>
    public interface ICourseEnrollment {
        int CourseId { get; set; }
        int StudentId { get; set; }
    }
}
