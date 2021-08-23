using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Models;

namespace UniversitySystem.Controllers
{
    public class GradeController : Controller
    {
        private readonly ApplicationDbContext _db;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        public GradeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        // GET: Grade
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                return View(await _db.Grades.ToListAsync());
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: Grade/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return NotFound();
                }
                var grade = await _db.Grades
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (grade == null)
                {
                    return NotFound();
                }

                return View(grade);
            }
            return RedirectToAction("Index", "Portal");

        }

        // GET: Grade/Create
        public IActionResult Create()
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                return View();
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: Grade/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Grade grade)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (ModelState.IsValid)
                {
                    _db.Add(grade);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(grade);
            }
            return RedirectToAction("Index", "Portal");

        }

        // GET: Grade/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var grade = await _db.Grades.FindAsync(id);
                if (grade == null)
                {
                    return NotFound();
                }
                return View(grade);
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: Grade/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Grade grade)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id != grade.Id)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        _db.Update(grade);
                        await _db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!GradeExists(grade.Id))
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
                return View(grade);
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: Grade/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return NotFound();
                }
                var grade = await _db.Grades
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (grade == null)
                {
                    return NotFound();
                }
                return View(grade);
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: Grade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                var grade = await _db.Grades.FindAsync(id);
                _db.Grades.Remove(grade);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Portal");
        }

        private bool GradeExists(int id)
        {
            return _db.Grades.Any(e => e.Id == id);
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

