using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UniversitySystem.Models;

namespace UniversitySystem.Controllers
{
    public class SemesterController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SemesterController(ApplicationDbContext db)
        {
            _db = db;

        }
        // GET: Semester
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                return View(await _db.Semesters.ToListAsync());
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: Semester/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return NotFound();
                }
                var semester = await _db.Semesters
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (semester == null)
                {
                    return NotFound();
                }

                return View(semester);
            }
            return RedirectToAction("Index", "Portal");

        }

        // GET: Semester/Create
        public IActionResult Create()
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                return View();
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: Semester/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Semester semester)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (ModelState.IsValid)
                {
                    _db.Add(semester);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(semester);
            }
            return RedirectToAction("Index", "Portal");

        }

        // GET: Semester/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var semester = await _db.Semesters.FindAsync(id);
                if (semester == null)
                {
                    return NotFound();
                }
                return View(semester);
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: Semester/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Semester semester)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id != semester.Id)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        _db.Update(semester);
                        await _db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SemesterExists(semester.Id))
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
                return View(semester);
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: Semester/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return NotFound();
                }
                var semester = await _db.Semesters
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (semester == null)
                {
                    return NotFound();
                }
                return View(semester);
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: Semester/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                var semester = await _db.Semesters.FindAsync(id);
                _db.Semesters.Remove(semester);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Portal");
        }

        private bool SemesterExists(int id)
        {
            return _db.Semesters.Any(e => e.Id == id);
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

