namespace StudentSystem.Interfaces {
    /// <summary>
    /// Define interface for a course.
    /// </summary>
    public interface ICourse {
        /// <summary>
        /// Course Title
        /// </summary>
        string? Title { get; set; }

        /// <summary>
        /// Qualification Awarded
        /// </summary>
        public string? Qualification { get; set; }
    }
}
