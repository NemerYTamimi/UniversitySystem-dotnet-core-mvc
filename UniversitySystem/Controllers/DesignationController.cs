using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UniversitySystem.Models;

namespace UniversitySystem.Controllers
{
    public class DesignationController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DesignationController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: DesignationController
        public async Task<ActionResult> Index()
        {
            var designations = await _db.Designations.ToListAsync();
            return View(designations);
        }

        // GET: Designation/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var designations = await _db.Designations.FindAsync(id);
            return View(designations);
        }

        // GET: Designation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Designation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Designation designation)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (ModelState.IsValid)
                {
                    _db.Designations.Add(designation);
                    await _db.SaveChangesAsync();
                    ViewBag.Message = "Student Registered Successfully";
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: Designation/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return BadRequest();
                }
                Designation designation = await _db.Designations.FindAsync(id);
                if (designation == null)
                {
                    return NotFound();
                }
                return View(designation);
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: Designation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Designation designation)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (ModelState.IsValid)
                {
                    _db.Entry(designation).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(designation);
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: Designation/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return BadRequest();
                }
                Designation designation = await _db.Designations.FindAsync(id);
                if (designation != null)
                {
                    return View(designation);
                }
                return NotFound();
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: Designation/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                Designation designation = _db.Designations.Find(id);
                _db.Designations.Remove(designation);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Portal");
        }
    }
}
