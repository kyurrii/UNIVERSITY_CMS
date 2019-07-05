using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EF.ASP.NET.CORE.Models;
using Microsoft.EntityFrameworkCore;

namespace EF.ASP.NET.CORE.Controllers
{
  

    using Microsoft.AspNetCore.Authorization;
  

 //   using Models;
   

    public class LecturerController : Controller
    {
        private readonly UniContext context;

        public LecturerController(UniContext context)
        {
            this.context = context;
        }

        // GET
        public IActionResult Lecturers()
        {
            List<Lecturer> lecturers=new List<Lecturer>();

            if (context.Lecturer.Any())
            {
                 lecturers = context.Lecturer.ToList();
               
            }

            // return View("Lecturers",lecturers);
            return View(lecturers);

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            Lecturer lecturer = context.Lecturer.SingleOrDefault(s=>s.Id==id);
            ViewData["Action"] = "Edit";
            return this.View(lecturer);
        }

        [HttpPost]
       [Authorize(Roles = "Admin")]
        public IActionResult Edit(Lecturer model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Action"] = "Edit";
                return this.View("Edit", model);
            }
            this.context.Lecturer.Update(model);
            context.SaveChangesAsync();

            return RedirectToAction("Lecturers");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
          var lecturerTodelete = this.context.Lecturer.SingleOrDefault(s=>s.Id==id);
            context.Lecturer.Remove(lecturerTodelete);
            context.SaveChanges();
            return RedirectToAction("Lecturers");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["Action"] = "Create";
            var lecturer = new Lecturer();
            return this.View("Edit", lecturer);
        }

        [HttpPost]

        public IActionResult Create(Lecturer model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Action"] = "Create";
                return this.View("Edit", model);
            }

            this.context.Lecturer.Add(model);
            this.context.SaveChanges();
            
            return RedirectToAction("Lecturers");

        }

        
    }
}