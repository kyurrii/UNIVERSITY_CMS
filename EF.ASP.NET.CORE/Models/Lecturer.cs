namespace EF.ASP.NET.CORE.Models
{
    using EF.ASP.NET.CORE.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using System.Collections.Generic;

    public class Lecturer
    {
       public int Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }
        [ForeignKey("Course")]
        public int? CourseId { get; set; }
       // 
       public virtual Course Course { get; set; }
        
       // public List<Course> Courses { get; set; }
    }
}
