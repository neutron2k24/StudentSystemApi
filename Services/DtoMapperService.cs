using StudentSystem.Dto;
using StudentSystem.Interfaces;
using StudentSystem.Models;

namespace StudentSystem.Services {

    /// <summary>
    /// Data Transfer Object to Data Entity Mapping Service.
    /// </summary>
    public class DtoMapperService : IDtoMappingService {

        #region IStudentDtoMapperService

        /// <summary>
        /// Map StudentDetailDto properties to a Student Entity object.
        /// </summary>
        /// <param name="basicStudentDetailDto"></param>
        /// <param name="student"></param>
        public void MapStudentDetailDtoToStudentEntity(BasicStudentDetailDto basicStudentDetailDto, Student student) {
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

        /// <summary>
        /// Map Student Entity properties to a new StudentDto object.
        /// </summary>
        /// <param name="student"></param>
        /// <param name="student"></param>
        public StudentDto? GetStudentDtoForStudentEntity(Student student, bool includeEnrollments = false) {
            if (student == null) return null;
            StudentDto studentDto = new StudentDto() {
                StudentId = student.StudentId,
                Forename = student.Forename,
                Surname = student.Surname,
                DateOfBirth = student.DateOfBirth,
                EmailAddress = student.EmailAddress,
                Gender = student.Gender
            };

            //Populate enrollments
            if (includeEnrollments && student.Enrollments != null && student.Enrollments.Count > 0) {
                studentDto.CourseEnrollments = new List<StudentCourseEnrollmentDto>();
                student.Enrollments.ForEach(enrollment => {
                    StudentCourseEnrollmentDto? courseEnrollmentDto = GetStudentCourseEnrollmentDtoForCourseEnrollmentEntity(enrollment);
                    if (courseEnrollmentDto != null) studentDto.CourseEnrollments.Add(courseEnrollmentDto);
                });
            }
            else {
                studentDto.CourseEnrollments = null;
            }
            return studentDto;
        }

        #endregion

        #region ICourseDtoMapperService

        /// <summary>
        /// Utility function to map NewCourseDto properties to a Course Entity.
        /// </summary>
        /// <param name="basicCourseDetailDto"></param>
        /// <param name="course"></param>
        public void MapCourseDetailDtoToCourseEntity(BasicCourseDetailDto basicCourseDetailDto, Course course) {
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

        /// <summary>
        /// Get a CourseDto for a specified course entity.
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public CourseDto? GetCourseDtoForCourseEntity(Course course, bool includeEnrollments = false) {
            if (course == null) return null;
            var courseDto = new CourseDto() {
                CourseId = course.CourseId,
                Title = course.Title,
                Qualification = course.Qualification
            };

            //Include student enrollments?
            if (includeEnrollments && course.Enrollments != null && course.Enrollments.Count > 0) {
                courseDto.EnrolledStudents = new List<EnrolledStudentDto>();
                course.Enrollments.ForEach(enrollment => {
                    EnrolledStudentDto? enrolledStudentDto = GetEnrolledStudentDtoForCourseEnrollmentEntity(enrollment);
                    if (enrolledStudentDto != null) courseDto.EnrolledStudents.Add(enrolledStudentDto);
                });
            }

            return courseDto;
        }

        #endregion

        #region ICourseEnrollmentDtoMapperService

        /// <summary>
        /// Map a Course Enrollment Entity to a new CourseEnrollmentDto.
        /// </summary>
        /// <param name="courseEnrollment"></param>
        public CourseEnrollmentDto? GetCourseEnrollmentDtoForCourseEnrollmentEntity(CourseEnrollment courseEnrollment) {
            if (courseEnrollment == null || courseEnrollment.Student == null || courseEnrollment.Course == null) return null;
            CourseEnrollmentDto courseEnrollmentDto = new CourseEnrollmentDto() {
                CourseEnrollmentId = courseEnrollment.CourseEnrollmentId,
                EnrollmentDate = courseEnrollment.EnrollmentDate,
                StudentId = courseEnrollment.StudentId,
                CourseId = courseEnrollment.CourseId,
                Student = GetStudentDtoForStudentEntity(courseEnrollment.Student, false),
                Course = GetCourseDtoForCourseEntity(courseEnrollment.Course, false)
            };
            return courseEnrollmentDto;
        }

        #endregion

        #region IEnrolledStudentDtoMapperService

        /// <summary>
        /// Map a Course Enrollment Entity to a new EnrolledStudentDto.
        /// </summary>
        /// <param name="courseEnrollment"></param>
        public EnrolledStudentDto? GetEnrolledStudentDtoForCourseEnrollmentEntity(CourseEnrollment courseEnrollment) {
            if (courseEnrollment == null || courseEnrollment.Student == null) return null;
            EnrolledStudentDto enrolledStudentDto = new EnrolledStudentDto() {
                CourseEnrollmentId = courseEnrollment.CourseEnrollmentId,
                EnrollmentDate = courseEnrollment.EnrollmentDate,
                Student = GetStudentDtoForStudentEntity(courseEnrollment.Student)
            };

            return enrolledStudentDto;
        }

        #endregion

        #region IStudentCourseEnrollmentDtoMapperService

        /// <summary>
        /// Map a Course Enrollment Entity to a new StudentCourseEnrollmentDto.
        /// </summary>
        /// <param name="courseEnrollment"></param>
        public StudentCourseEnrollmentDto? GetStudentCourseEnrollmentDtoForCourseEnrollmentEntity(CourseEnrollment courseEnrollment) {
            if (courseEnrollment == null || courseEnrollment.Course == null) return null;
            StudentCourseEnrollmentDto studentCourseEnrollmentDto = new StudentCourseEnrollmentDto() {
                CourseEnrollmentId = courseEnrollment.CourseEnrollmentId,
                EnrollmentDate = courseEnrollment.EnrollmentDate,
                Course = GetCourseDtoForCourseEntity(courseEnrollment.Course, false)
            };

            return studentCourseEnrollmentDto;
        }

        #endregion
    }
}
