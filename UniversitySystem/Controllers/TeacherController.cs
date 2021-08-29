using UniversitySystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversitySystem.Models.ViewModels;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UniversitySystem.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TeacherController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Teacher/
        public ActionResult Index()
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                var teachers =   _db.Teachers.Include(s => s.Department).Include(t => t.Designation).Select(teacher => new TeacherVM()
                {
                    Id = teacher.Id,
                    Email = teacher.TeacherEmail,
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
                    await _db.SaveChangesAsync();
                    ViewBag.Message = "Teacher Saved Successfully";
                    // return RedirectToAction("Index");
                }
                ViewBag.DepartmentId = new SelectList(_db.Departments, "Id", "DeptCode", teacher.DepartmentId);
                ViewBag.DesignationId = new SelectList(_db.Designations, "Id", "Name", teacher.DesignationId);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Portal");
        }
        public ActionResult IsEmailExists(string teacherEmail)
        {
            if (teacherEmail != null)
            {
                return Json(!_db.Teachers.Any(m => m.TeacherEmail == teacherEmail));
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
