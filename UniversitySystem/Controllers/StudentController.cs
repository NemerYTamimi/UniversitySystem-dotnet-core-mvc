using UniversitySystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public ActionResult Index()
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                var students = _db.Students.Include(s => s.Department).Select(student => new StudentVM()
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
        public ActionResult Details(int? id)
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
        public ActionResult Create()
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
        public ActionResult Create(Student student)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (ModelState.IsValid)
                {
                    student.StudentRegNo = GetStudentRegNo(student);
                    _db.Students.Add(student);
                    _db.SaveChanges();
                    ViewBag.Message = "Student Registered Successfully";
                }

                ViewBag.DepartmentId = new SelectList(_db.Departments, "Id", "DeptCode", student.DepartmentId);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Portal");

        }
        public string GetStudentRegNo(Student aStudent)
        {
            var count = _db.Students.Count(m => (m.DepartmentId == aStudent.DepartmentId) && (m.Date.Year == aStudent.Date.Year)) + 1;

            var aDepartment = _db.Departments.FirstOrDefault(m => m.Id == aStudent.DepartmentId);

            /* if regNumber = 10 will be converted to 4 digits = 0010 */
            string leadingZero = "";
            int length = 3 - count.ToString().Length;
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
        public ActionResult Edit(int? id)
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
        public ActionResult Edit(Student student)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (ModelState.IsValid)
                {
                    _db.Entry(student).State = EntityState.Modified;
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.DepartmentId = new SelectList(_db.Departments, "Id", "DeptCode", student.DepartmentId);
                return View(student);
            }
            return RedirectToAction("Index", "Portal");

        }

        // GET: /Student/Delete/5
        public ActionResult Delete(int? id)
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
        public ActionResult Delete(int id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                Student student = _db.Students.Find(id);
                _db.Students.Remove(student);
                _db.SaveChanges();
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
