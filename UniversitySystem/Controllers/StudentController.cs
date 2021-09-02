using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UniversitySystem.Models;
using UniversitySystem.Models.ViewModels;
using UniversitySystem.Services;
using UniversitySystem.Utility;

namespace UniversitySystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IStudentService _studentService;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;

        public StudentController(ApplicationDbContext db, IStudentService studentService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _studentService = studentService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        // GET: /Student/
        public ActionResult Index()
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
                Finanial finanial = await _db.Finanials.FirstOrDefaultAsync(f => f.StudentId == student.Id);
                if (finanial == null)
                {
                    return NotFound();
                }
                ViewBag.credit = finanial.Credit;
                ViewBag.used = finanial.CreditUsed;
                return View(student);
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: /Student/Create
        public async Task<ActionResult> Create()
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                ViewBag.ParentId = new SelectList(await _db.Parents.ToListAsync(), "Id", "Name");
                ViewBag.DepartmentId = new SelectList(await _db.Departments.ToListAsync(), "Id", "DeptCode");
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

                    Finanial finanial = new Finanial()
                    {
                        StudentId = student.Id
                    };
                    if (await CreateAccount(student.Email, student.Name, Helper.Student))
                    {
                        await _db.Finanials.AddAsync(finanial);
                        await _db.SaveChangesAsync();
                        ViewBag.Message = "Student Registered Successfully";
                    }
                }
                ViewBag.ParentId = new SelectList(_db.Parents, "Id", "Name", student.ParentId);
                ViewBag.DepartmentId = new SelectList(_db.Departments, "Id", "DeptCode", student.DepartmentId);
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

        public async Task<string> GetStudentRegNo(Student aStudent)
        {
            var count = await _db.Students.CountAsync(m => (m.DepartmentId == aStudent.DepartmentId) && (m.Date.Year == aStudent.Date.Year)) + 1;

            var aDepartment = await _db.Departments.FirstOrDefaultAsync(m => m.Id == aStudent.DepartmentId);

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


        public ActionResult IsEmailExists(string Email, int Id)
        {
            if (Email != null)
            {
                return Json(!_db.Students.Any(m => m.Email == Email && m.Id != Id));
            }
            return BadRequest();
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
                Student student = await _db.Students.FindAsync(id);
                if (student == null)
                {
                    return NotFound();
                }
                ViewBag.DepartmentId = new SelectList(await _db.Departments.ToListAsync(), "Id", "DeptCode", student.DepartmentId);
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
                ViewBag.DepartmentId = new SelectList(await _db.Departments.ToListAsync(), "Id", "DeptCode", student.DepartmentId);
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
                Student student = await _db.Students.FindAsync(id);
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
                Student student = await _db.Students.FindAsync(id);
                student.Status = false;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Portal");
        }


        // GET: /Student/AddCredit/5
        public async Task<ActionResult> AddCredit(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return BadRequest();
                }
                Student student = await _db.Students.FindAsync(id);
                if (student != null)
                {
                    StudentCreditVM studentCreditVM = new StudentCreditVM()
                    {
                        StudentRegNo = student.StudentRegNo
                    };
                    return View(studentCreditVM);
                }
                return NotFound();
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: /Student/AddCredit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCredit(StudentCreditVM studentCreditVM)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (_studentService.AddCredit(studentCreditVM.StudentRegNo, studentCreditVM.Credit))
                {
                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            return RedirectToAction("Index", "Portal");
        }

        [HttpPost]
        [Produces("application/json")]
        public IActionResult StudentCourses(string jsonInput = "")
        {
            if (jsonInput != null)
            {
                try
                {
                    StudentSemesterVM ssvm = JsonConvert.DeserializeObject<StudentSemesterVM>(jsonInput);
                    var studentCourses = _db.EnrollCourses
                        .Include(m => m.Course)
                        .Where(m => m.RegistrationNo == ssvm.StudentRegNo && m.Course.SemesterId == ssvm.SemesterId);
                    return /*Ok();*/ Ok(studentCourses);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest();
        }

        public IActionResult MyCourses()
        {
            if (User.IsInRole(Helper.Student))
            {
                ViewBag.Semesters = _db.Semesters;
                var student = _db.Students.Where(s => s.Email == User.Identity.Name).FirstOrDefault();
                ViewBag.StudentRegNo = student.StudentRegNo;
                return View();
            }
            ViewData["msg"] = "You are not a student!";
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
