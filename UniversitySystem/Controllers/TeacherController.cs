using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using UniversitySystem.Models;
using UniversitySystem.Models.ViewModels;
using UniversitySystem.Utility;

namespace UniversitySystem.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public TeacherController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: /Teacher/
        public ActionResult Index()
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                var teachers = _db.Teachers.Include(s => s.Department).Include(t => t.Designation).Select(teacher => new TeacherVM()
                {
                    Id = teacher.Id,
                    Email = teacher.Email,
                    DesignationName = teacher.Designation.Name,
                    Name = teacher.Name
                });
                return View(teachers.ToList());
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: /Teacher/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return BadRequest();
                }
                Teacher teacher = await _db.Teachers.FindAsync(id);
                if (teacher == null)
                {
                    return NotFound();
                }
                Department department = await _db.Departments.FindAsync(teacher.DepartmentId);
                ViewBag.Department = department;
                return View(teacher);
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: /Teacher/Create
        public async Task<ActionResult> Create()
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                ViewBag.DepartmentId = new SelectList(await _db.Departments.ToListAsync(), "Id", "DeptCode");
                ViewBag.DesignationId = new SelectList(await _db.Designations.ToListAsync(), "Id", "Name");
                return View();
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: /Teacher/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Teacher teacher)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (ModelState.IsValid)
                {
                    _db.Teachers.Add(teacher);
                    if (await CreateAccount(teacher.Email, teacher.Name, Helper.Teacher))
                    {
                        await _db.SaveChangesAsync();
                        ViewBag.Message = "Teacher Saved Successfully";
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.DepartmentId = new SelectList(_db.Departments, "Id", "DeptCode", teacher.DepartmentId);
                ViewBag.DesignationId = new SelectList(_db.Designations, "Id", "Name", teacher.DesignationId);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Portal");
        }
        public ActionResult IsEmailExists(string email, int Id)
        {
            if (email != null)
            {
                return Json(!_db.Teachers.Any(m => m.Email == email && m.Id != Id) && !_db.Students.Any(m => m.Email == email && m.Id != Id));
            }
            return BadRequest();
        }

        // GET: /Teacher/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return BadRequest();
                }
                Teacher teacher = await _db.Teachers.FindAsync(id);
                if (teacher == null)
                {
                    return NotFound();
                }
                ViewBag.DepartmentId = new SelectList(_db.Departments, "Id", "DeptCode", teacher.DepartmentId);
                ViewBag.DesignationId = new SelectList(_db.Designations, "Id", "Name", teacher.DesignationId);
                return View(teacher);
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: /Teacher/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Teacher teacher)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (ModelState.IsValid)
                {
                    _db.Entry(teacher).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                ViewBag.DepartmentId = new SelectList(_db.Departments, "Id", "DeptCode", teacher.DepartmentId);
                ViewBag.DesignationId = new SelectList(_db.Designations, "Id", "Name", teacher.DesignationId);
                return View(teacher);
            }
            return RedirectToAction("Index", "Portal");

        }

        // GET: /Teacher/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return BadRequest();
                }
                Teacher teacher = await _db.Teachers.FindAsync(id);
                if (teacher == null)
                {
                    return NotFound();
                }
                return View(teacher);
            }
            return RedirectToAction("Index", "Portal");
        }


        // POST: /Teacher/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                Teacher teacher = await _db.Teachers.FindAsync(id);
                _db.Teachers.Remove(teacher);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Portal");
        }

        private async Task<bool> CreateAccount(string email, string name, string role)
        {
            var user = new ApplicationUser()
            {
                Name = name,
                Email = email,
                UserName = email
            };
            var result = await _userManager.CreateAsync(user, "ChangeMe123$");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return true;
            }
            return false;
        }

        public IActionResult MyCourses()
        {
            if (User.IsInRole(Helper.Teacher))
            {
                ViewBag.Semesters = _db.Semesters;
                //var teacher = _db.Teachers.Where(s => s.Email == User.Identity.Name).FirstOrDefault();
                //ViewBag.TeacherId = teacher.Id;
                return View();
            }
            ViewData["msg"] = "You are not a teacehr!";
            return RedirectToAction("Index", "Portal");
        }

        [HttpPost]
        [Produces("application/json")]
        public IActionResult TeacherCourses(string jsonInput = "")
        {
            if (jsonInput != null)
            {
                try
                {
                    var teacher = _db.Teachers.FirstOrDefault(m => m.Email == User.Identity.Name);
                    IdVM semester = JsonConvert.DeserializeObject<IdVM>(jsonInput);
                    var teacherCourses = _db.CourseAssigns
                        .Include(m => m.Course)
                        .Where(m => m.TeacherId == teacher.Id && m.Course.SemesterId == semester.Id);
                    return /*Ok();*/ Ok(teacherCourses);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest();
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
