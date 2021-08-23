using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Models;
using UniversitySystem.Models.ViewModels;

namespace UniversitySystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _db;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public StudentController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        // GET: /Student/
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                var students = _db.Students.Include(s => s.Department).Where(student => student.Status == true).Select(student => new StudentVM()
                {
                    Id = student.Id,
                    Name = student.Name,
                    StudentRegNo = student.StudentRegNo
                });
                return View(students.ToList());
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: /Student/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return BadRequest();
                }
                Student student = _db.Students.Find(id);
                student.Department = _db.Departments.Find(student.DepartmentId);
                if (student == null)
                {
                    return NotFound();
                }
                return View(student);
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: /Student/Create
        public async Task<ActionResult> Create()
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                ViewBag.DepartmentId = new SelectList(_db.Departments, "Id", "DeptCode");
                return View();
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: /Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Student student)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (ModelState.IsValid)
                {
                    student.StudentRegNo = await GetStudentRegNo(student);
                    student.Status = true;
                    _db.Students.Add(student);
                    await _db.SaveChangesAsync();
                    ViewBag.Message = "Student Registered Successfully";
                }
                ViewBag.DepartmentId = new SelectList(_db.Departments, "Id", "DeptCode", student.DepartmentId);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Portal");
        }
        public async Task<string> GetStudentRegNo(Student aStudent)
        {
            var count = _db.Students.Count(m => (m.DepartmentId == aStudent.DepartmentId) && (m.Date.Year == aStudent.Date.Year)) + 1;

            var aDepartment = _db.Departments.FirstOrDefault(m => m.Id == aStudent.DepartmentId);

            /* if regNumber = 10 will be converted to 4 digits = 0010 */
            string leadingZero = "";
            int length = 4 - count.ToString().Length;
            for (int i = 0; i < length; i++)
            {
                leadingZero += "0";
            }
            string studentRegNo = aDepartment.DeptCode + "-" + aStudent.Date.Year + "-" + leadingZero + count;
            return studentRegNo;
        }


        public JsonResult IsEmailExists(string Email)
        {
            return Json(!_db.Students.Any(m => m.Email == Email), new Newtonsoft.Json.JsonSerializerSettings());
        }

        // GET: /Student/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return BadRequest();
                }
                Student student = _db.Students.Find(id);
                if (student == null)
                {
                    return NotFound();
                }
                ViewBag.DepartmentId = new SelectList(_db.Departments, "Id", "DeptCode", student.DepartmentId);
                return View(student);
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: /Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Student student)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (ModelState.IsValid)
                {
                    _db.Entry(student).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                ViewBag.DepartmentId = new SelectList(_db.Departments, "Id", "DeptCode", student.DepartmentId);
                return View(student);
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: /Student/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return BadRequest();
                }
                Student student = _db.Students.Find(id);
                if (student != null)
                {
                    StudentVM studentVM = new StudentVM()
                    {
                        Id = student.Id,
                        Name = student.Name,
                        StudentRegNo = student.StudentRegNo
                    };
                    return View(studentVM);
                }
                return NotFound();
            }
            return RedirectToAction("Index", "Portal");
        }


        // POST: /Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                Student student = _db.Students.Find(id);
                student.Status = false;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Portal");
        }

        //Finalizer
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}
