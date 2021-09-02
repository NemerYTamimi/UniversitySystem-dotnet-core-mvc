using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UniversitySystem.Models;

namespace UniversitySystem.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CourseController(ApplicationDbContext context)
        {
            _db = context;
        }

        // GET: Course
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                var applicationDbContext = _db.Courses.Include(c => c.Department).Include(c => c.Semester);
                return View(await applicationDbContext.ToListAsync());
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var course = await _db.Courses
                    .Include(c => c.Department)
                    .Include(c => c.Semester)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (course == null)
                {
                    return NotFound();
                }

                return View(course);
            }
            return RedirectToAction("Index", "Portal");

        }

        // GET: Course/Create
        public IActionResult Create()
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                ViewData["DepartmentId"] = new SelectList(_db.Departments, "Id", "DeptCode");
                ViewData["SemesterId"] = new SelectList(_db.Semesters, "Id", "Name");
                return View();
            }
            return RedirectToAction("Index", "Portal");

        }

        // POST: Course/Create


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseCode,CourseName,CourseCredit,CourseDescription,CourseAssignTo,CourseStatus,DepartmentId,SemesterId,Capacity")] Course course)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (ModelState.IsValid)
                {
                    _db.Add(course);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["DepartmentId"] = new SelectList(_db.Departments, "Id", "DeptCode", course.DepartmentId);
                ViewData["SemesterId"] = new SelectList(_db.Semesters, "Id", "Name", course.SemesterId);
                return View(course);
            }
            return RedirectToAction("Index", "Portal");

        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var course = await _db.Courses.FindAsync(id);
                if (course == null)
                {
                    return NotFound();
                }
                ViewData["DepartmentId"] = new SelectList(_db.Departments, "Id", "DeptCode", course.DepartmentId);
                ViewData["SemesterId"] = new SelectList(_db.Semesters, "Id", "Name", course.SemesterId);
                return View(course);
            }
            return RedirectToAction("Index", "Portal");

        }

        // POST: Course/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseName,CourseCode,CourseCredit,CourseDescription,CourseStatus,DepartmentId,SemesterId,Capacity")] Course course)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id != course.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _db.Entry<Course>(course).Property(x => x.CourseName).IsModified = false;
                        _db.Entry<Course>(course).Property(x => x.CourseCode).IsModified = false;
                        _db.Entry<Course>(course).Property(x => x.DepartmentId).IsModified = false;
                        _db.Update(course);
                        await _db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CourseExists(course.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                ViewData["DepartmentId"] = new SelectList(_db.Departments, "Id", "DeptCode", course.DepartmentId);
                ViewData["SemesterId"] = new SelectList(_db.Semesters, "Id", "Name", course.SemesterId);
                return View(course);
            }
            return RedirectToAction("Index", "Portal");

        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var course = await _db.Courses
                    .Include(c => c.Department)
                    .Include(c => c.Semester)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (course == null)
                {
                    return NotFound();
                }

                return View(course);
            }
            return RedirectToAction("Index", "Portal");

        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                var course = await _db.Courses.FindAsync(id);
                _db.Courses.Remove(course);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Portal");

        }

        public ActionResult IsUniqueCode(string CourseCode)
        {
            if (CourseCode != null)
            {
                return Json(!_db.Courses.Any(m => m.CourseCode == CourseCode));
            }
            return BadRequest();
        }
        public ActionResult IsUniqueName(string CourseName)
        {
            if (CourseName != null)
            {
                return Json(!_db.Courses.Any(m => m.CourseName == CourseName));
            }
            return BadRequest();
        }

        private bool CourseExists(int id)
        {
            return _db.Courses.Any(e => e.Id == id);
        }

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
