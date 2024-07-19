using StudentSystem.Dto;
using StudentSystem.Models;

namespace StudentSystem.Interfaces {
    /// <summary>
    /// Interface for DTO Mapping Service.
    /// </summary>
    public interface IDtoMappingService  {

        #region CourseDto Mapping
        /// <summary>
        /// Function to map NewCourseDto properties to a Course Entity.
        /// </summary>
        void MapBasicCourseDetailDtoToCourseEntity(BasicCourseDetailDto courseDetailDto, Course course);

        /// <summary>
        /// Function to map CourseDto properties to a Course Entity.
        /// </summary>
        void MapCourseDtoToCourseEntity(CourseDto courseDto, Course course);

        /// <summary>
        /// Get a CourseDto for a specified course entity.
        /// </summary>
        CourseDto? GetCourseDtoForCourseEntity(Course course, bool includeEnrollments = false);

        #endregion

        #region CourseEnrollmentDto Mapping

        /// <summary>
        /// Map a Course Enrollment Entity to a new CourseEnrollmentDto.
        /// </summary>
        CourseEnrollmentDto? GetCourseEnrollmentDtoForCourseEnrollmentEntity(CourseEnrollment courseEnrollment);

        #endregion

        #region EnrolledStudentDto Mapping

        /// <summary>
        /// Map BasicCourseEnrollmentDetailDto properties to CourseEnrollment Entity.
        /// </summary>
        void MapBasicCourseEnrollmentDetailDtoToCourseEnrollmentEntity(BasicCourseEnrollmentDetailDto basicCourseEnrollmentDetailDto, CourseEnrollment enrollment);

        /// <summary>
        /// Map a Course Enrollment Entity to a new EnrolledStudentDto.
        /// </summary>
        EnrolledStudentDto? GetEnrolledStudentDtoForCourseEnrollmentEntity(CourseEnrollment courseEnrollment);

        #endregion

        #region StudentCourseEnrollmentDto Mapping
        
        /// <summary>
        /// Map a Course Enrollment Entity to a new StudentCourseEnrollmentDto.
        /// </summary>
        StudentCourseEnrollmentDto? GetStudentCourseEnrollmentDtoForCourseEnrollmentEntity(CourseEnrollment courseEnrollment);

        #endregion

        #region StudentDto Mapping

        /// <summary>
        /// Map StudentDetailDto properties to a Student Entity object.
        /// </summary>
        void MapBasicStudentDetailDtoToStudentEntity(BasicStudentDetailDto studentDetailDto, Student student);

        /// <summary>
        /// Map StudentDto properties to Student Entity.
        /// </summary>
        void MapStudentDtoToStudentEntity(StudentDto studentDto, Student student);

        /// <summary>
        /// Map Student Entity properties to a new StudentDto object.
        /// </summary>
        StudentDto? GetStudentDtoForStudentEntity(Student student, bool includeEnrollments = false);

        #endregion
    }
}
