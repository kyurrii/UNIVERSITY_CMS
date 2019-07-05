using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EF.ASP.NET.CORE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Services;

namespace EF.ASP.NET.CORE.Controllers
{
    public class StudentController : Controller
    {

        //private readonly UniContext context;

        //public StudentController(UniContext _context)
        //{
        //    context = _context;

        //}


        private readonly StudentService studentService;

        public StudentController(StudentService studentService)
        {
            this.studentService = studentService;
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

        //public IActionResult Students()
        //{
        //    var students = context.Student.ToList();

        //    return View(students);
        //}


        //[Authorize(Roles = "Admin,Lecturer")]
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var student = await context.Student
        //        .Include(s => s.CourseStud)
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(m => m.Id == id);

        //    var coursesIDs = student.CourseStud.Select(s=>s.CourseId).ToList();

        //    ViewBag.CoursesNomber = context.StudentCourse.Where(s => s.StudentId == id).Count();

        //    foreach (var courseId in coursesIDs)
        //    {
        //        var course = this.context.Course.SingleOrDefault(s => s.Id == courseId);
        //        student.CourseStud.Add(new StudentCourse() { Course = course, Student = student });
        //    }

        //    if (student == null)
        //    {
        //        return NotFound();
        //    }

        //    return View("Details",student);
        //}




        //[HttpGet]
        //[Authorize(Roles = "Admin")]
        //public IActionResult Edit(int id)
        //{
        //    var student = context.Student.Single(s=>s.Id==id);
        //    ViewData["Action"] = "Edit";
        //    return this.View(student);
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //public IActionResult Edit(Student model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ViewData["Action"] = "Edit";
        //        return this.View("Edit", model);
        //    }
        //    this.context.Update(model);
        //    this.context.SaveChanges();

        //    return RedirectToAction("Students");
        //}

        //[HttpGet]
        //[Authorize(Roles = "Admin")]
        //public IActionResult Delete(int id)
        //{
        //    var student = context.Student.Single(s => s.Id == id);
        //    this.context.Student.Remove(student);
        //    this.context.SaveChanges();

        //    return RedirectToAction("Students");
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    ViewData["Action"] = "Create";
        //    var student = new Student();
        //    return this.View("Edit", student);
        //}

        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public IActionResult Create(Student model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ViewData["Action"] = "Create";
        //        return this.View("Edit", model);
        //    }

        //    this.context.Student.Add(model);
        //    this.context.SaveChanges();
        //    //this.studentService.CreateStudent(model);
        //    return RedirectToAction("Students");

        //}
    }
    }