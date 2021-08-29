using Microsoft.AspNetCore.Identity;
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
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(Department department)
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

        public ActionResult Index()
        {
            IEnumerable<Department> departments = _departmentService.GetAllDepartment();
            ViewBag.DepartmentList = departments;
            return View();
        }

        public ActionResult IsDeptCodeExists(string code)
        {
            bool isDeptCodeExists = _departmentService.IsDepartmentCodeExist(code);

            if (isDeptCodeExists)
                return Json(false);
            else
            {
                return Json(true);
            }
        }
        public ActionResult IsDeptNameExists(string name)
        {
            bool isDeptNameExists = _departmentService.IsDepartmentNameExist(name);
            if (isDeptNameExists)
                return Json(false);
            else
            {
                return Json(true);
            }
        }
        // GET: Department/Edit/5
        public ActionResult Edit(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return BadRequest();
                }
                Department department = _departmentService.GetDepartment(id);
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
        public ActionResult Edit(Department department)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (ModelState.IsValid)
                {
                    _departmentService.EditDepartment(department);
                    return RedirectToAction("Index");
                }
                return View(department);
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: Department/Delete/5
        public ActionResult Delete(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id != null)
                {
                    Department department = _departmentService.GetDepartment(id);
                    if(department != null)
                    {
                        return View(department);
                    }
                }
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: Department/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Department department)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (ModelState.IsValid)
                {
                    _departmentService.DeleteDepartment(department.Id);
                    return RedirectToAction("Index");
                }
                return View(department);
            }
            return RedirectToAction("Index", "Portal");
        }

        ////Finalizer
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _departmentService.disposeDb();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
