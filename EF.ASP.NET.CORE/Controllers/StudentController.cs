using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EF.ASP.NET.CORE.Models;
using EF.ASP.NET.CORE.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Services;

namespace EF.ASP.NET.CORE.Controllers
{
    public class StudentController : Controller
    {

        private readonly UniContext context;

    
        private readonly StudentService studentService;

        public StudentController(StudentService studentService, UniContext context)
        {
            this.studentService = studentService;
            this.context = context;
        }

        // GET
        public IActionResult Students()
        {
            return View(studentService.GetAllStudents());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var student = studentService.GetStudentById(id);
            ViewData["Action"] = "Edit";
            return this.View(student);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Student model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Action"] = "Edit";
                return this.View("Edit", model);
            }
            this.studentService.UpdateStudent(model);

            return RedirectToAction("Students");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            this.studentService.DeleteStudent(id);
            return RedirectToAction("Students");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Action"] = "Create";
            var student = new Student();
            return this.View("Edit", student);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Student model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Action"] = "Create";
                return this.View("Edit", model);
            }

            this.studentService.CreateStudent(model);
            return RedirectToAction("Students");

        }


        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var student = await context.Student.Include(s=>s.CourseStud).FirstOrDefaultAsync(s => s.Id == id);
            var coursesStd = this.context.StudentCourse.Where(s => s.StudentId == id).ToList();


            StudentDetails model = new StudentDetails();

            model.Id = student.Id;
            model.Name = student.Name;
            model.BirthDate = student.BirthDate;
            model.Email = student.Email;
            model.Notes = student.Notes;
            model.PhoneNumber = student.PhoneNumber;
            model.GitHubLink = student.GitHubLink;
            model.CourseView = new List<CourseViewModel>();



            var coursesIDs = student.CourseStud.Select(s => s.CourseId).ToList();

            ViewBag.CoursesNomber = context.StudentCourse.Where(s => s.StudentId == id).Count();
            model.CourseView.Clear();

            foreach (var courseId in coursesIDs)
            {
                var course = this.context.Course.Include(c=>c.HomeTasks).SingleOrDefault(s => s.Id == courseId);

                var htass = this.context.HomeTaskAssessment.Include(t=>t.HomeTask).ThenInclude(k=>k.Course).Include(t=>t.Student).Where(h => h.HomeTask.Course.Id == courseId && h.Student.Id == id).ToList();
                // student.CourseStud.Add(new StudentCourse() { Course = course, Student = student, CourseId = course.Id, StudentId = student.Id });

                int htqty = htass.Count();
                int htasOkqty = htass.Where(h => h.IsComplete == true).Count();
                double compRate = 0;
                if (htqty != 0)
                {
                    compRate = htasOkqty  *100 / htqty;
                }
                    

                model.CourseView.Add(new CourseViewModel() {CourseId=courseId,Name=course.Name,PassCredits=course.PassCredits,HomeTaskNomber=htqty, CompletionRate=compRate });


            }

            if (student == null)
            {
                return NotFound();
            }


            return View("Details", model);
        }




    }
}