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


        public StudentController(ApplicationDbContext db)
        {
            _db = db;
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
                    await _db.Finanials.AddAsync(finanial);
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


        public ActionResult IsEmailExists(string Email)
        {
            if (Email != null)
            {
                return Json(!_db.Students.Any(m => m.Email == Email));
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
                Student student = await _db.Students.FirstOrDefaultAsync(s => s.StudentRegNo == studentCreditVM.StudentRegNo);
                if(student != null)
                {
                    Finanial finanial = await _db.Finanials.FirstOrDefaultAsync(f => f.StudentId == student.Id);
                    if (finanial != null)
                    {
                        finanial.Credit += studentCreditVM.Credit;
                        await _db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }
                return NotFound();
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
