namespace EF.ASP.NET.CORE.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CourseStudentAssignment
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int PassCredits { get; set; }

        public virtual List<StudentViewModel> Students { get; set; }
    }

    public class StudentViewModel
    { 
        [Key]
        public int StudentId { get; set; }

        public string StudentFullName { get; set; }

        public bool IsAssigned { get; set; }
    }
}
