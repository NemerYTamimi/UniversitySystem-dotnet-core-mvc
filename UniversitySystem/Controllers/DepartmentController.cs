﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversitySystem.Models;
using UniversitySystem.Services;

namespace UniversitySystem.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _db;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        private readonly IDepartmentService _departmentService;


        public DepartmentController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IDepartmentService departmentService)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _departmentService = departmentService;
        }

        [HttpGet]
        public  async Task<ActionResult> Create()
        {
            return View();
        }

        // GET: Department/Details/5
        public  async Task<ActionResult> Details(int id)
        {
            var departments =  _db.Departments.FindAsync(id);
            return View(departments);
        }

        [HttpPost]
        public  async Task<ActionResult> Create(Department department)
        {
            try
            {
                string message = _departmentService.SaveDepartment(department);
                ViewBag.Message = message;
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                ViewBag.Message = exception.InnerException.Message;
                return View();
            }
        }

        public  async Task<ActionResult> Index()
        {
            IEnumerable<Department> departments = _departmentService.GetAllDepartment();
            ViewBag.DepartmentList = departments;
            return View();
        }

        public JsonResult IsDeptCodeExists(string code)
        {
            bool isDeptCodeExists = _departmentService.IsDepartmentCodeExist(code);

            if (isDeptCodeExists)
                return Json(false, new Newtonsoft.Json.JsonSerializerSettings());
            else
            {
                return Json(true, new Newtonsoft.Json.JsonSerializerSettings());
            }
        }
        public JsonResult IsDeptNameExists(string name)
        {
            bool isDeptNameExists = _departmentService.IsDepartmentNameExist(name);
            if (isDeptNameExists)
                return Json(false, new Newtonsoft.Json.JsonSerializerSettings());
            else
            {
                return Json(true, new Newtonsoft.Json.JsonSerializerSettings());

            }
        }
        // GET: Department/Edit/5
        public  async Task<ActionResult> Edit(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return BadRequest();
                }
                Department department = await _db.Departments.FindAsync(id);
                if (department == null)
                {
                    return NotFound();
                }
                return View(department);
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: Department/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<ActionResult> Edit(Department Department)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (ModelState.IsValid)
                {
                    _db.Entry(Department).State = EntityState.Modified;
                     await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(Department);
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: Department/Delete/5
        public  async Task<ActionResult> Delete(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return BadRequest();
                }
                Department Department = await _db.Departments.FindAsync(id);
                if (Department != null)
                {
                    return View(Department);
                }
                return NotFound();
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: Department/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<ActionResult> Delete(int id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                Department Department = await _db.Departments.FindAsync(id);
                _db.Departments.Remove(Department);
                 await _db.SaveChangesAsync();
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
