using UniversitySystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversitySystem.Models.ViewModels;
using System.Threading.Tasks;

namespace UniversitySystem.Controllers
{
    public class DesignationController : Controller
    {
        private readonly ApplicationDbContext _db;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public DesignationController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        // GET: DesignationController
        public async Task<ActionResult> Index()
        {
            var designations = _db.Designations;
            return View(designations);
        }

        // GET: Designation/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var designations = _db.Designations.Find(id);
            return View(designations);
        }

        // GET: Designation/Create
        public async Task<ActionResult> Create()
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
                Designation designation = _db.Designations.Find(id);
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
                Designation designation = _db.Designations.Find(id);
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
