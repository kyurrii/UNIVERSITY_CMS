using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF.ASP.NET.CORE.ViewModels;
using Microsoft.AspNetCore.Mvc;
using EF.ASP.NET.CORE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EF.ASP.NET.CORE.Controllers
{
    public class CourseController : Controller
    {
        private readonly UniContext context;

        public CourseController(UniContext _context)
        {
            context = _context;
        }

        public IActionResult Courses()
        {
            var courses = context.Course.ToList();
            return View(courses);
        }

        [Authorize(Roles = "Admin,Lecturer")]
        [HttpGet]
        public IActionResult AssignStudents(int id)
        {
            var allStudents = this.context.Student.ToList();
            var course = this.context.Course.SingleOrDefault(c=>c.Id==id);
            CourseStudentAssignment model = new CourseStudentAssignment();

            model.Id = id;
            model.EndDate = course.EndDate;
            model.Name = course.Name;
            model.StartDate = course.StartDate;
            model.PassCredits = course.PassCredits;
            model.Students = new List<StudentViewModel>();
            var assignments = new List<StudentCourse>();

            assignments = context.StudentCourse.Where( c => c.CourseId==id).ToList();

            foreach (var student in allStudents)
            {
                bool isAssigned = assignments.Where(p => p.StudentId == student.Id).Any(s=>s.Student!=null);

                model.Students.Add(new StudentViewModel() { StudentId = student.Id, StudentFullName = student.Name, IsAssigned = isAssigned });

            }



                return this.View(model);
        }

        [Authorize(Roles = "Admin,Lecturer")]
        [HttpPost]
        public IActionResult AssignStudents(CourseStudentAssignment assignmentViewModel)
        {

            this.SetStudentsToCourse(assignmentViewModel.Id, assignmentViewModel.Students.Where(p => p.IsAssigned).Select(student => student.StudentId));

            context.StudentCourse.RemoveRange();
            context.SaveChanges();

            return RedirectToAction("Courses");
        }



        public virtual void SetStudentsToCourse(int courseId, IEnumerable<int> studentIds)
        {
            var course = this.context.Course.SingleOrDefault(c => c.Id == courseId);
            var assignments = context.StudentCourse.Where(c => c.CourseId == courseId).ToList();
            context.StudentCourse.Where(c => c.CourseId == courseId).ToList().Clear();
          
            course.StudentCour.Clear();

            this.context.Update(course);
            context.SaveChanges();

            foreach (var studentId in studentIds)
            {
                var student = this.context.Student.SingleOrDefault(s => s.Id == studentId);
                course.StudentCour.Add(new StudentCourse() { Course = course, Student = student });
            }

            this.context.Course.Update(course);
 
         
            context.SaveChanges();
         
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
           Course courseTodelete = this.context.Course.Include(s=>s.StudentCour).SingleOrDefault(c=>c.Id==id);
            this.context.Remove(courseTodelete);
            context.SaveChanges();

            return RedirectToAction("Courses");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["action"] = nameof(this.Create);
            return this.View("Edit", new Course());
        }

        [Authorize(Roles = "Admin,Lecturer")]
        [HttpGet]
        public IActionResult Details(int id)
        {
            ViewData["action"] = nameof(this.Details);

            var allStudents = this.context.Student.AsNoTracking().ToList();
            var course = this.context.Course.SingleOrDefault(c => c.Id == id);
            CourseStudentAssignment model = new CourseStudentAssignment();

            model.Id = id;
            model.EndDate = course.EndDate;
            model.Name = course.Name;
            model.StartDate = course.StartDate;
            model.PassCredits = course.PassCredits;
            model.Students = new List<StudentViewModel>();
            var assignments = new List<StudentCourse>();

            assignments = context.StudentCourse.Where(c => c.CourseId == id).ToList();

            foreach (var student in allStudents) 
            {
      
                bool isAssigned = assignments.Any(p => p.StudentId == student.Id);

                if (isAssigned)
                {
                    model.Students.Add(new StudentViewModel() { StudentId = student.Id, StudentFullName = student.Name, IsAssigned = isAssigned });

                }
            }
            return this.View( model);
       
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
     
        public IActionResult Edit(int id)
        {
            Course course = this.context.Course.SingleOrDefault(c => c.Id == id);
            if (course == null)
            {
                return this.NotFound();
            }

            ViewData["action"] = nameof(this.Edit);

            return this.View(course);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
    
        public IActionResult Edit(Course courseParameter)
        {
            if (courseParameter == null)
            {
                return this.BadRequest();
            }
           this.context.Course.Update(courseParameter);
            context.SaveChanges();
            return this.RedirectToAction(nameof(Courses));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Course courseParameter)
        {
            if (courseParameter == null)
            {
                return this.BadRequest();
            }

            if (!ModelState.IsValid)
            {
                ViewData["action"] = nameof(this.Create);

                return this.View("Edit", courseParameter);
            }
            this.context.Course.Add(courseParameter);
            context.SaveChanges();
            return this.RedirectToAction(nameof(Courses));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        
        public IActionResult AssignLecturers(int id)
        {
            var allLecturers = context.Lecturer.ToList();
            Course course = this.context.Course.Include(cr=>cr.Lecturers).SingleOrDefault(c => c.Id == id);
            CourseLecturerAssignmentViewModel model = new CourseLecturerAssignmentViewModel();

            model.Id = id;
            model.EndDate = course.EndDate;
            model.Name = course.Name;
            model.StartDate = course.StartDate;
            model.PassCredits = course.PassCredits;
            model.Lecturers = new List<LecturersViewModel>();

            foreach (var lecturer in allLecturers)
            {
                 bool isAssigned = course.Lecturers.Any(p => p.Id == lecturer.Id);
             //   bool isAssigned = this.context.Lecturer.Any(p => p.CourseId == id);

                model.Lecturers.Add(new LecturersViewModel() { LecturerId = lecturer.Id, Name = lecturer.Name, IsAssigned = isAssigned });
            }

            return this.View(model);
        }

         [Authorize(Roles = "Admin")]
        [HttpPost]
      
        public IActionResult AssignLecturers(CourseLecturerAssignmentViewModel model)
        {


            this.SetLecturersToCourse(model.Id, model.Lecturers.Where(p => p.IsAssigned).Select(lecturer => lecturer.LecturerId));

            return RedirectToAction("Courses");
        }

        public virtual void SetLecturersToCourse(int courseId, IEnumerable<int> lecturerIds)
        {
            var course = this.context.Course.Include(cr=>cr.Lecturers).SingleOrDefault(c => c.Id == courseId);

            var allLecturerIds = context.Lecturer.Select(s => s.Id).ToList();

         

            course.Lecturers.ToList().Clear();


            this.context.Update(course);
        
            context.SaveChanges();

            foreach (var lectId in lecturerIds)
            {
                var lecturer = context.Lecturer.SingleOrDefault(s => s.Id == lectId);
                course.Lecturers.Add(lecturer);
                
            }


            this.context.Update(course);
            context.SaveChanges();



        }
    }
}