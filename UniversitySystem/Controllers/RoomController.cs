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
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RoomController(ApplicationDbContext db)
        {
            _db = db;

        }

        // GET: Room
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                return View(await _db.Rooms.ToListAsync());
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: Room/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return NotFound();
                }
                var room = await _db.Rooms
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (room == null)
                {
                    return NotFound();
                }

                return View(room);
            }
            return RedirectToAction("Index", "Portal");

        }

        // GET: Room/Create
        public IActionResult Create()
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                return View();
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: Room/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Room room)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (ModelState.IsValid)
                {
                    _db.Add(room);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(room);
            }
            return RedirectToAction("Index", "Portal");

        }

        // GET: Room/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var room = await _db.Rooms.FindAsync(id);
                if (room == null)
                {
                    return NotFound();
                }
                return View(room);
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: Room/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Room room)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id != room.Id)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        _db.Update(room);
                        await _db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RoomExists(room.Id))
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
                return View(room);
            }
            return RedirectToAction("Index", "Portal");
        }

        // GET: Room/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                if (id == null)
                {
                    return NotFound();
                }
                var room = await _db.Rooms
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (room == null)
                {
                    return NotFound();
                }
                return View(room);
            }
            return RedirectToAction("Index", "Portal");
        }

        // POST: Room/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.IsInRole(Utility.Helper.Admin))
            {
                var room = await _db.Rooms.FindAsync(id);
                _db.Rooms.Remove(room);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Portal");
        }

        private bool RoomExists(int id)
        {
            return _db.Rooms.Any(e => e.Id == id);
        }
    }
}
