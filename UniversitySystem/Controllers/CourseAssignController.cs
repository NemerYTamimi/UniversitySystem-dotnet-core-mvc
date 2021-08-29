using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UniversitySystem.Models;
using UniversitySystem.Models.ViewModels;

namespace UniversitySystem.Controllers
{
    public class CourseAssignController : Controller
    {
        private ApplicationDbContext _db;

        public CourseAssignController(ApplicationDbContext db)
        {
            _db = db;
        }

        public ActionResult ViewCourseStatistics()
        {
            ViewBag.Departments = _db.Departments.ToList();
            return View();
        }

        public ActionResult ShowCourseStatistics(int deptId)
        {
            var courses = _db.Courses.Where(m => m.DepartmentId == deptId).ToList();
            return Json(courses);
        }

        // GET: /CourseAssign/Create
        public IActionResult Save()
        {
            ViewBag.Departments = _db.Departments.ToList();
            return View();
        }

        [HttpPost]
        [Produces("application/json")]
        public IActionResult GetTeacherByDeptId(string jsonInput ="")
        {
            IdVM department = JsonConvert.DeserializeObject<IdVM>(jsonInput);
            var teachers = _db.Teachers.Where(m => m.DepartmentId == department.Id).ToList();
            return Ok(teachers);
        }

        [HttpPost]
        [Produces("application/json")]
        public IActionResult GetCourseByDeptId(string jsonInput = "")
        {
            IdVM department = JsonConvert.DeserializeObject<IdVM>(jsonInput);
            var courses = _db.Courses.Where(m => m.DepartmentId == department.Id).ToList();
            return Ok(courses);
        }


        // POST: /CourseAssign/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CourseAssign courseassign)
        {
            if (ModelState.IsValid)
            {
                _db.CourseAssigns.Add(courseassign);
                _db.SaveChanges();
                ViewBag.Message = "Course Assigned Successful";
            }
            ViewBag.Departments = _db.Departments.ToList();
            ViewBag.CourseID = new SelectList(_db.Courses, "Id", "CourseCode", courseassign.CourseId);
            ViewBag.DepartmentId = new SelectList(_db.Departments, "Id", "DeptCode", courseassign.DepartmentId);
            ViewBag.TeacherId = new SelectList(_db.Teachers, "Id", "TeacherName", courseassign.TeacherId);
            return View(courseassign);
        }


        public ActionResult SaveCourseAssign(String jsonInput = "")
        {
            CourseAssign courseAssign = JsonConvert.DeserializeObject<CourseAssign>(jsonInput);
            var checkAssignedCourses =
                _db.CourseAssigns.Where(m => m.CourseId == courseAssign.CourseId && m.Course.CourseStatus == true)
                    .ToList();

            if (checkAssignedCourses.Count > 0)
                return Ok(false);

            else
            {
                _db.CourseAssigns.Add(courseAssign);
                _db.SaveChanges();

                var teacher = _db.Teachers.FirstOrDefault(m => m.Id == courseAssign.TeacherId);

                if (teacher != null)
                {
                    _db.Teachers.Update(teacher);
                    _db.SaveChanges();

                    var course = _db.Courses.FirstOrDefault(m => m.Id == courseAssign.CourseId);

                    if (course != null)
                    {
                        course.CourseStatus = true;
                        course.CourseAssignTo = teacher.Name;
                        _db.Courses.Update(course);
                        _db.SaveChanges();

                        return Ok(true);
                    }
                    else
                    {
                        return Ok(false);
                    }
                }
                else
                {
                    return Ok(false);
                }
            }
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
