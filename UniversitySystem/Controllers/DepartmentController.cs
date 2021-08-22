using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

    }
}
