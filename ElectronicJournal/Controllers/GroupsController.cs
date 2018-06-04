using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectronicJournal.Models;
using Microsoft.AspNetCore.Authorization;

namespace ElectronicJournal.Controllers
{
    public class GroupsController : Controller, IDataController
    {
        private readonly ElectronicJournalContext _context;

        public GroupsController(ElectronicJournalContext context)
        {
            _context = context;
        }

        // GET: Groups
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Group.ToListAsync());
        }

        // GET: Groups/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ID,Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(group);
        }

        // GET: Groups/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Group.SingleOrDefaultAsync(m => m.ID == id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Group group)
        {
            if (id != group.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(group.ID))
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
            return View(group);
        }

        // GET: Groups/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Group
                .SingleOrDefaultAsync(m => m.ID == id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var group = await _context.Group.SingleOrDefaultAsync(m => m.ID == id);
            var user = await _context.Users.FirstOrDefaultAsync(m => m.GroupID == id);

            if(user != null)
            {
                return View("NotDelete");
            }
            var lessons = _context.Lesson.Where(m => m.GroupID == id);
            var students = _context.Student.Where(m => m.GroupID == id);
            var subjects = _context.Subject.Where(m => m.GroupID == id);

            foreach (var lesson in lessons)
            {
                _context.Remove(lesson);
            }

            foreach (var student in students)
            {
                _context.Remove(student);
            }

            foreach (var subject in subjects)
            {
                _context.Remove(subject);
            }

            _context.Group.Remove(group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            return _context.Group.Any(e => e.ID == id);
        }
    }
}
