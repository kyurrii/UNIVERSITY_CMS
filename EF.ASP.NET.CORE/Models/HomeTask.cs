namespace EF.ASP.NET.CORE.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class HomeTask
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Number { get; set; }

        public Course Course { get; set; }

        public List<HomeTaskAssessment> HomeTaskAssessments { get; set; }
    }
}