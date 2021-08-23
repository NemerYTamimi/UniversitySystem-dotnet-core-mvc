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
    public class ClassRoomAllocationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassRoomAllocationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClassRoomAllocations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ClassRoomAllocations.Include(c => c.Course).Include(c => c.Day).Include(c => c.Department).Include(c => c.Room);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ClassRoomAllocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classRoomAllocation = await _context.ClassRoomAllocations
                .Include(c => c.Course)
                .Include(c => c.Day)
                .Include(c => c.Department)
                .Include(c => c.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classRoomAllocation == null)
            {
                return NotFound();
            }

            return View(classRoomAllocation);
        }

        // GET: ClassRoomAllocations/Create
        public IActionResult Create()
        {
            ViewBag.courses = _context.Courses.Where(s => s.CourseStatus == true).ToList();
            ViewBag.departments = _context.Departments.ToList();
            ViewBag.Rooms = _context.Rooms.ToList();
            ViewBag.Days = _context.Days.ToList();
            return View();
        }

        // POST: ClassRoomAllocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DepartmentId,CourseId,RoomId,DayId,StartTime,EndTime,RoomStatus")] ClassRoomAllocation classRoomAllocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classRoomAllocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseCode", classRoomAllocation.CourseId);
            ViewData["DayId"] = new SelectList(_context.Days, "Id", "Name", classRoomAllocation.DayId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DeptCode", classRoomAllocation.DepartmentId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", classRoomAllocation.RoomId);
            return View(classRoomAllocation);
        }

        // GET: ClassRoomAllocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classRoomAllocation = await _context.ClassRoomAllocations.FindAsync(id);
            if (classRoomAllocation == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseCode", classRoomAllocation.CourseId);
            ViewData["DayId"] = new SelectList(_context.Days, "Id", "Name", classRoomAllocation.DayId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DeptCode", classRoomAllocation.DepartmentId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", classRoomAllocation.RoomId);
            return View(classRoomAllocation);
        }

        // POST: ClassRoomAllocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DepartmentId,CourseId,RoomId,DayId,StartTime,EndTime,RoomStatus")] ClassRoomAllocation classRoomAllocation)
        {
            if (id != classRoomAllocation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classRoomAllocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassRoomAllocationExists(classRoomAllocation.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseCode", classRoomAllocation.CourseId);
            ViewData["DayId"] = new SelectList(_context.Days, "Id", "Name", classRoomAllocation.DayId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DeptCode", classRoomAllocation.DepartmentId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", classRoomAllocation.RoomId);
            return View(classRoomAllocation);
        }

        // GET: ClassRoomAllocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classRoomAllocation = await _context.ClassRoomAllocations
                .Include(c => c.Course)
                .Include(c => c.Day)
                .Include(c => c.Department)
                .Include(c => c.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classRoomAllocation == null)
            {
                return NotFound();
            }

            return View(classRoomAllocation);
        }

        // POST: ClassRoomAllocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classRoomAllocation = await _context.ClassRoomAllocations.FindAsync(id);
            _context.ClassRoomAllocations.Remove(classRoomAllocation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassRoomAllocationExists(int id)
        {
            return _context.ClassRoomAllocations.Any(e => e.Id == id);
        }

        [HttpPost]
        [Produces("application/json")]

        public IActionResult GetClassScheduleInfo(string jsonInput = "")
        {
            int deptId= JsonConvert.DeserializeObject<int>(jsonInput);
            var courses = _context.Courses.Where(m => m.DepartmentId == deptId).ToList();
            return Json(courses);
        }
        [HttpPost]
        [Produces("application/json")]

        public IActionResult SaveRoomSchedule(string jsonInput = "")
        {
            if(jsonInput != null)
            {
                ClassRoomAllocation classRoomAllocation = JsonConvert.DeserializeObject<ClassRoomAllocation>(jsonInput);

                var scheduleList = _context.ClassRoomAllocations.Where(m => m.RoomId == classRoomAllocation.RoomId && m.DayId == classRoomAllocation.DayId && m.RoomStatus == "Allocated").ToList();
                if (scheduleList.Count == 0)
                {
                    classRoomAllocation.RoomStatus = "Allocated";
                    _context.ClassRoomAllocations.Add(classRoomAllocation);
                    _context.SaveChanges();
                    return Json(true);
                }
                else
                {
                    bool status = false;
                    foreach (var allocation in scheduleList)
                    {
                        if ((classRoomAllocation.StartTime >= allocation.StartTime && classRoomAllocation.StartTime < allocation.EndTime)
                             || (classRoomAllocation.EndTime > allocation.StartTime && classRoomAllocation.EndTime <= allocation.EndTime) && classRoomAllocation.RoomStatus == "Allocated")
                        {
                            status = true;
                        }
                    }
                    if (status == false)
                    {
                        classRoomAllocation.RoomStatus = "Allocated";
                        _context.ClassRoomAllocations.Add(classRoomAllocation);
                        _context.SaveChanges();
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
            }
            return BadRequest();

        }

        [HttpPost]
        [Produces("application/json")]
        public IActionResult GetCoursesByDeptId(string jsonInput = "")
        {
            if (jsonInput != null)
            {
                try
                {
                    DepartmentVM department = JsonConvert.DeserializeObject<DepartmentVM>(jsonInput);
                    var courses = _context.Courses.Where(m => m.DepartmentId == department.DepartmentId).ToList();
                    return /*Ok();*/ Ok(courses);
                }
                catch(Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest();
        }
        public IActionResult UnAllocateAllRooms(bool decision)
        {
            var roomStatus = _context.ClassRoomAllocations.Where(m => m.RoomStatus == "Allocated").ToList();
            if (roomStatus.Count == 0)
            {
                return Json(false);
            }
            else
            {
                foreach (var room in roomStatus)
                {
                    room.RoomStatus = "";
                    _context.ClassRoomAllocations.Update(room);
                    _context.SaveChanges();

                }
                return Json(true);
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
