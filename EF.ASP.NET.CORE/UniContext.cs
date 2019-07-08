using EF.ASP.NET.CORE.Models;
using EF.ASP.NET.CORE.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EF.ASP.NET.CORE
{
    public class UniContext : DbContext
    {

        public UniContext(DbContextOptions<UniContext> options) : base(options)
        {
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
           // optionsBuilder.UseSqlServer(@Server= (localdb)\MSSQLLocalDB; Initial Catalog = BaseCourse001; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
         //  optionsBuilder.UseSqlServer(@"Data Source=KYURRII07\SQLEXPRESS;Initial Catalog=BaseCourse01;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");

            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<StudentCourse>().ToTable("StudentCourse");

            modelBuilder.Entity<StudentCourse>().HasKey(k => new { k.CourseId, k.StudentId });

            modelBuilder.Entity<Lecturer>().ToTable("Lecturer");
            modelBuilder.Entity<HomeTask>().ToTable("HomeTask");
            modelBuilder.Entity<HomeTaskAssessment>().ToTable("HomeTaskAssessment");

            //modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment");
            //modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment");

            //modelBuilder.Entity<HomeTaskAssessment>()
            //        .HasKey(c => new { c.CourseID, c.InstructorID });
        }





             public DbSet<Student> Student { get; set; }
             public DbSet<Course> Course { get; set; }
             public DbSet<Lecturer> Lecturer { get; set; }
             public DbSet<HomeTask> HomeTask { get; set; }
             public DbSet<HomeTaskAssessment> HomeTaskAssessment { get; set; }

             public DbSet <StudentCourse> StudentCourse { get; set; }


    }
}
