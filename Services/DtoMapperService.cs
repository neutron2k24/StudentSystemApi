using StudentSystem.Dto;
using StudentSystem.Interfaces;
using StudentSystem.Models;

namespace StudentSystem.Services {

    /// <summary>
    /// Data Transfer Object to Data Entity Mapping Service.
    /// </summary>
    public class DtoMapperService : IDtoMappingService {

        #region StudentDto mapping

        /// <summary>
        /// Map StudentDetailDto properties to a Student Entity object.
        /// </summary>
        /// <param name="basicStudentDetailDto"></param>
        /// <param name="student"></param>
        public void MapBasicStudentDetailDtoToStudentEntity(BasicStudentDetailDto basicStudentDetailDto, Student student) {
            student.Forename = basicStudentDetailDto.Forename;
            student.Surname = basicStudentDetailDto.Surname;
            student.EmailAddress = basicStudentDetailDto.EmailAddress;
            student.DateOfBirth = basicStudentDetailDto.DateOfBirth;
            student.Gender = basicStudentDetailDto.Gender;
        }

        /// <summary>
        /// Map StudentDto properties to Student Entity.
        /// </summary>
        /// <param name="studentDto"></param>
        /// <param name="student"></param>
        public void MapStudentDtoToStudentEntity(StudentDto studentDto, Student student) {
            student.Forename = studentDto.Forename;
            student.Surname = studentDto.Surname;
            student.EmailAddress = studentDto.EmailAddress;
            student.DateOfBirth = studentDto.DateOfBirth;
            student.Gender = studentDto.Gender;
        }

        #endregion

        #region CourseDto mapping

        /// <summary>
        /// Utility function to map NewCourseDto properties to a Course Entity.
        /// </summary>
        /// <param name="basicCourseDetailDto"></param>
        /// <param name="course"></param>
        public void MapBasicCourseDetailDtoToCourseEntity(BasicCourseDetailDto basicCourseDetailDto, Course course) {
            course.Title = basicCourseDetailDto.Title;
            course.Qualification = basicCourseDetailDto.Qualification;
        }

        /// <summary>
        /// Utility function to map CourseDto properties to a Course Entity.
        /// </summary>
        /// <param name="courseDto"></param>
        /// <param name="course"></param>
        public void MapCourseDtoToCourseEntity(CourseDto courseDto, Course course) {
            course.CourseId = courseDto.CourseId;
            course.Title = courseDto.Title;
            course.Qualification = courseDto.Qualification;
        }

        #endregion

        #region CourseEnrollmentDto mapping

        /// <summary>
        /// Map BasicCourseEnrollmentDetailDto properties to CourseEnrollment Entity.
        /// </summary>
        /// <param name="basicCourseEnrollmentDetailDto"></param>
        /// <param name="enrollment"></param>
        public void MapBasicCourseEnrollmentDetailDtoToCourseEnrollmentEntity(BasicCourseEnrollmentDetailDto basicCourseEnrollmentDetailDto, CourseEnrollment enrollment) {
            enrollment.CourseId = basicCourseEnrollmentDetailDto.CourseId;
            enrollment.StudentId = basicCourseEnrollmentDetailDto.StudentId;
        }

        #endregion
    }
}
