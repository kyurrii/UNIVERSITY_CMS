using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF.ASP.NET.CORE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF.ASP.NET.CORE.ViewModels
{
    public class StudentDetails
    {

       

        public string Name { get; set; }

        public int Id { get; set; }


        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string GitHubLink { get; set; }

        public string Notes { get; set; }

        public List<CourseViewModel> CourseView { get; set; } = new List<CourseViewModel>();


   }

    public class CourseViewModel
    {
        public string Name { get; set; }

        public int CourseId { get; set; }


        public int PassCredits { get; set; }

        public int HomeTaskNomber { get; set; }
        public double CompletionRate { get; set; }


        public List<HomeTaskViewModel> HomeTasksView { get; set; } = new List<HomeTaskViewModel>();

    }

    public class HomeTaskViewModel
    {
        public int HomeTaskId { get; set; }

       
        public string Title { get; set; }

      

        public int Number { get; set; }

        public List<HomeTaskAssessmentModel> HomeTaskAssessmentsView { get; set; } = new List<HomeTaskAssessmentModel>();
    }

    public class HomeTaskAssessmentModel
    {

        public int Id { get; set; }

        public bool IsComplete { get; set; }

    }
}
