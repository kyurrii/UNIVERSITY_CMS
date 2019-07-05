using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EF.ASP.NET.CORE.Models
{
    public class StudentCourse
    {
        public virtual Course Course{ get; set; }

        public virtual Student Student { get; set; }

        public int StudentId { get; set; }

        public int CourseId { get; set; }

    }
}
