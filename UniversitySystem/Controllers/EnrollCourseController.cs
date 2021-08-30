using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UniversitySystem.Models;
using UniversitySystem.Models.ViewModels;
using UniversitySystem.Services;

namespace UniversitySystem.Controllers
{
    public class EnrollCourseController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IStudentService _studentService;

        public EnrollCourseController(ApplicationDbContext context, IStudentService studentService)
        {
            _db = context;
            _studentService = studentService;
        }

        // GET: EnrollCourse
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _db.EnrollCourses.Include(e => e.Course);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EnrollCourse/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollCourse = await _db.EnrollCourses
                .Include(e => e.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollCourse == null)
            {
                return NotFound();
            }

            return View(enrollCourse);
        }

        // GET: EnrollCourse/Create
        public IActionResult Create()
        {
            ViewData["StudentRegNo"] = new SelectList(_db.Students, "StudentRegNo", "StudentRegNo");
            ViewData["CourseId"] = new SelectList(_db.Courses, "Id", "CourseCode");
            return View();
        }

        // POST: EnrollCourse/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RegistrationNo,CourseId")] EnrollCourse enrollCourse)
        {
            if (ModelState.IsValid)
            {
                enrollCourse.EnrollDate = DateTime.Now;
                Course course = _db.Courses.Find(enrollCourse.CourseId);

                if (course != null)
                {
                    if (_studentService.UseCredit(enrollCourse.RegistrationNo, course.CourseCredit))
                    {
                        if (_studentService.NumOfEnrolledCourses(enrollCourse.RegistrationNo, course.SemesterId) < 7)
                        {

                            if (!_studentService.IsEnrolled(enrollCourse.RegistrationNo, course.SemesterId, enrollCourse.CourseId))
                            {
                                if (!IsFullCapacity(course.Id))
                                {
                                    _db.Add(enrollCourse);
                                    await _db.SaveChangesAsync();
                                    return RedirectToAction(nameof(Index));
                                }
                                else
                                {
                                    ViewData["msg"] = "The course capacity is full";
                                }
                            }
                            else
                            {
                                ViewData["msg"] = "The student is already enrolled in the course";
                            }
                        }
                        else
                        {
                            ViewData["msg"] = "The student exceeds number of enrolled courses in the same semester";
                        }
                    }
                    else
                    {
                        ViewData["msg"] = "The student credit is not enough to enroll in the course";
                    }
                }
            }
            ViewData["StudentRegNo"] = new SelectList(_db.Students, "StudentRegNo", "StudentRegNo");
            ViewData["CourseId"] = new SelectList(_db.Courses, "Id", "CourseCode", enrollCourse.CourseId);

            return View(enrollCourse);
        }


        // GET: EnrollCourse/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollCourse = await _db.EnrollCourses
                .Include(e => e.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollCourse == null)
            {
                return NotFound();
            }

            return View(enrollCourse);
        }

        // POST: EnrollCourse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollCourse = await _db.EnrollCourses.FindAsync(id);
            _db.EnrollCourses.Remove(enrollCourse);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollCourseExists(int id)
        {
            return _db.EnrollCourses.Any(e => e.Id == id);
        }

        private bool IsFullCapacity(int id)
        {
            int capacity = _db.Courses.Find(id).Capacity;
            return _db.EnrollCourses.Count(s => s.CourseId == id) == capacity;
        }
        //[HttpPost]
        //[Produces("application/json")]
        //public IActionResult GetStudentByDeptId(string jsonInput = "")
        //{
        //    IdVM department = JsonConvert.DeserializeObject<IdVM>(jsonInput);
        //    var students = _db.Students.Where(m => m.DepartmentId == department.Id).ToList();
        //    return Ok(students);
        //}
    }
}
