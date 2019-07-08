using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF.ASP.NET.CORE.ViewModels;
using Services;
using EF.ASP.NET.CORE.Models;
using Microsoft.EntityFrameworkCore;

namespace EF.ASP.NET.CORE.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EF.ASP.NET.CORE.ViewModels;

   
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Routing;

    public class HomeTaskController : Controller
    {

        private readonly HomeTaskService homeTaskService;
        private readonly StudentService studentService;
      
        private readonly UniContext context;

        public HomeTaskController(HomeTaskService homeTaskService, StudentService studentService, UniContext context)
        {
            this.homeTaskService = homeTaskService;
            this.studentService = studentService;
            this.context = context;
        }

         public IActionResult HomeTasks(int Id)
        {
            var homeTasks = context.HomeTask.Where(c => c.Course.Id == Id).ToList();
            ViewData["CourseId"] = Id;

            return View(homeTasks);


        }

     

        [HttpGet]
        [Authorize(Roles = "Admin,Lecturer")]
        public IActionResult Edit(int id)
        {
            HomeTask homeTask = this.homeTaskService.GetHomeTaskById(id);
            if (homeTask == null)
                return this.NotFound();
            ViewData["Action"] = "Edit";

            return View(homeTask);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Lecturer")]
        public IActionResult Edit(HomeTask homeTaskParameter)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Action"] = "Edit";

                return View(homeTaskParameter);
            }

            var homeTask = this.context.HomeTask.Include(c => c.Course).SingleOrDefault(h => h.Id == homeTaskParameter.Id);

            homeTask.Title = homeTaskParameter.Title;
            homeTask.Number = homeTaskParameter.Number;
            homeTask.Description = homeTaskParameter.Description;
            homeTask.Date = homeTaskParameter.Date;

            var routeValueDictionary = new RouteValueDictionary();

           
            this.context.Update(homeTask);
            context.SaveChanges();

            routeValueDictionary.Add("id", homeTask.Course.Id);
         
            return RedirectToAction("HomeTasks", "HomeTask", routeValueDictionary);
        }


        [HttpGet]
        [Authorize(Roles = "Admin,Lecturer")]
        public IActionResult Create(int Id)
        {
            ViewData["Action"] = "Create";
            ViewData["CourseId"] = Id;

            var homeTask = new HomeTask();
            return View("Edit", homeTask);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Lecturer")]
        public IActionResult Create([Bind("Title,Description,Number,Date")] HomeTask model, int courseId)
        {


            if (!ModelState.IsValid)
            {
                ViewData["Action"] = "Create";
                ViewData["CourseId"] = courseId;
                return View("Edit", model);
            }
            var routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("id", courseId);

            var course = this.context.Course.SingleOrDefault(c => c.Id == courseId);
            model.Course = course;
            context.HomeTask.Add(model);

            context.SaveChanges();

            return RedirectToAction("HomeTasks", "HomeTask", routeValueDictionary);

       
        }


       


        [HttpGet]
        [Authorize(Roles = "Admin,Lecturer")]
        public IActionResult Delete(int Id)    //int courseId)
        {
            //   this.homeTaskService.DeleteHomeTask(homeTaskId);
            HomeTask homeTask = this.context.HomeTask.Include(c => c.Course).SingleOrDefault(h => h.Id == Id);
            this.context.HomeTask.Remove(homeTask);
            context.SaveChanges();

            var routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("id", homeTask.Course.Id);
            return RedirectToAction("HomeTasks", "HomeTask", routeValueDictionary);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Lecturer")]
        public IActionResult Evaluate(int id)
        {
            var homeTask = this.context.HomeTask.Include(c => c.Course).Include(hs => hs.HomeTaskAssessments).ThenInclude(s => s.Student).Single(h => h.Id == id);  //homeTaskService.GetHomeTaskById(id);

            if (homeTask == null)
            {
                return this.NotFound();
            }

            HomeTaskAssessmentViewModel assessmentViewModel =
                new HomeTaskAssessmentViewModel
                {
                    Date = homeTask.Date,
                    Description = homeTask.Description,
                    Title = homeTask.Title,
                    HomeTaskStudents = new List<HomeTaskStudentViewModel>(),
                    HomeTaskId = homeTask.Id
                };



            if (homeTask.HomeTaskAssessments?.Any() == true)
            {
                foreach (var homeTaskHomeTaskAssessment in homeTask.HomeTaskAssessments)
                {
                    assessmentViewModel.HomeTaskStudents.Add(new HomeTaskStudentViewModel()
                    {

                        StudentFullName = homeTaskHomeTaskAssessment.Student.Name,
                        StudentId = homeTaskHomeTaskAssessment.Student.Id,
                        IsComplete = homeTaskHomeTaskAssessment.IsComplete,
                        HomeTaskAssessmentId = homeTaskHomeTaskAssessment.Id
                    });
                }

            }
            else
            {
                var studList = context.StudentCourse.Where(c => c.CourseId == homeTask.Course.Id).Select(s => s.Student).ToList();

                foreach (var student in studList)
                {
                    assessmentViewModel.HomeTaskStudents.Add(new HomeTaskStudentViewModel() { StudentFullName = student.Name, StudentId = student.Id });
                }
            }

            return this.View(assessmentViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Lecturer")]
        public IActionResult Evaluate(HomeTaskAssessmentViewModel model)
        {
            var homeTask = this.context.HomeTask.Include(h => h.HomeTaskAssessments).Single(h => h.Id == model.HomeTaskId);    //homeTaskService.GetHomeTaskById(model.HomeTaskId);

            if (homeTask == null)
            {
                return this.NotFound();
            }

            foreach (var homeTaskStudent in model.HomeTaskStudents)
            {
                var target = homeTask.HomeTaskAssessments.Find(p => p.Id == homeTaskStudent.HomeTaskAssessmentId);
                if (target != null)
                {
                    target.Date = DateTime.Now;
                    target.IsComplete = homeTaskStudent.IsComplete;
                }
                else
                {
                    var student = this.context.Student.Single(s => s.Id == homeTaskStudent.StudentId);    //studentService.GetStudentById(homeTaskStudent.StudentId);
                    homeTask.HomeTaskAssessments.Add(new HomeTaskAssessment
                    {
                        HomeTask = homeTask,
                        IsComplete = homeTaskStudent.IsComplete,
                        Student = student,
                        Date = DateTime.Now

                    });
                }
                this.context.HomeTask.Update(homeTask);
                context.SaveChanges();
            }
            return RedirectToAction("Courses", "Course");
        }
    }
}