namespace EF.ASP.NET.CORE.Models
{
    using EF.ASP.NET.CORE.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Student
    {

        public string Name { get; set; }

        public int Id { get; set; }
        

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string GitHubLink { get; set; }

        public string Notes { get; set; }

        public List<StudentCourse> CourseStud { get; set; } = new List<StudentCourse>();
        // [NotMapped]
        //  public List<Course> Courses { get; set; } = new List<Course>();
        //  [NotMapped]
          public List<HomeTaskAssessment> HomeTaskAssessments { get; set; } = new List<HomeTaskAssessment>();
    }
}