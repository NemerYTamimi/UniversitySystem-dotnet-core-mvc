using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Utility;

namespace UniversitySystem.Controllers
{
    public class PortalController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole(Helper.Admin))
            {
                ViewData["Title"] = "Admin Portal";
            }
            else if (User.IsInRole(Helper.Teacher))
            {
                ViewData["Title"] = "Teacher Portal";
            }
            else if (User.IsInRole(Helper.Student))
            {
                ViewData["Title"] = "Student Portal";
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            ViewData["CompanyName"] = "Demo University";
            return View();
        }
    }
}
