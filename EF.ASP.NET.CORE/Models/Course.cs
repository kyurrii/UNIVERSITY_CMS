namespace EF.ASP.NET.CORE.Models
{
    using EF.ASP.NET.CORE.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Course
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int PassCredits { get; set; }

         public List<HomeTask> HomeTasks { get; set; }
        //  [NotMapped]
        public ICollection<Lecturer> Lecturers { get; set; } = new List<Lecturer>();

        public List<StudentCourse> StudentCour { get; set; } = new List<StudentCourse>();
    }
}