using UniversitySystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniversitySystem.Models.ViewModels;

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
        public ActionResult Index()
        {
            var designations = _db.Designations;
            return View(designations);
        }

        // GET: DesignationController/Details/5
        public ActionResult Details(int id)
        {
            var designations = _db.Designations.Find(id);
            return View(designations);
        }

        // GET: DesignationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DesignationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Designation designation)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (ModelState.IsValid)
                {
                    _db.Designations.Add(designation);
                    _db.SaveChanges();
                    ViewBag.Message = "Student Registered Successfully";
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: DesignationController/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: DesignationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Designation designation)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (ModelState.IsValid)
                {
                    _db.Entry(designation).State = EntityState.Modified;
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(designation);
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: DesignationController/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: DesignationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                Designation designation = _db.Designations.Find(id);
                _db.Designations.Remove(designation);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Portal");
        }
    }
}
