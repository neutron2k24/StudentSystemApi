using StudentSystem.Dto;
using StudentSystem.Models;

namespace StudentSystem.Interfaces {
    /// <summary>
    /// Interface for DTO to Entity Mapping Service.
    /// </summary>
    public interface IDtoMappingService  {

        /// <summary>
        /// Function to map NewCourseDto properties to a Course Entity.
        /// </summary>
        void MapBasicCourseDetailDtoToCourseEntity(BasicCourseDetailDto courseDetailDto, Course course);

        /// <summary>
        /// Function to map CourseDto properties to a Course Entity.
        /// </summary>
        void MapCourseDtoToCourseEntity(CourseDto courseDto, Course course);

        /// <summary>
        /// Map BasicCourseEnrollmentDetailDto properties to CourseEnrollment Entity.
        /// </summary>
        void MapBasicCourseEnrollmentDetailDtoToCourseEnrollmentEntity(BasicCourseEnrollmentDetailDto basicCourseEnrollmentDetailDto, CourseEnrollment enrollment);

        /// <summary>
        /// Map StudentDetailDto properties to a Student Entity object.
        /// </summary>
        void MapBasicStudentDetailDtoToStudentEntity(BasicStudentDetailDto studentDetailDto, Student student);

        /// <summary>
        /// Map StudentDto properties to Student Entity.
        /// </summary>
        void MapStudentDtoToStudentEntity(StudentDto studentDto, Student student);

        
    }
}
