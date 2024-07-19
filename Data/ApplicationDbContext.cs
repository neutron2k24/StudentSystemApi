using Microsoft.EntityFrameworkCore;
using StudentSystem.Models;

namespace StudentSystem.Data {
    /// <summary>
    /// Application DB Context. Derived from EntityFrameworkdCore DbContext
    /// </summary>
    public class ApplicationDbContext : DbContext {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
           
        }

        //Students Data
        public virtual DbSet<Student> Students { get; set; }

        //Course Data
        public virtual DbSet<Course> Courses { get; set; }

        //Enrollment Data
        public virtual DbSet<CourseEnrollment> CourseEnrollments { get; set;}
    }
}
