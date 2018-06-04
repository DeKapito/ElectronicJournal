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
    public class SubjectsController : Controller, IDataController
    {
        private readonly ElectronicJournalContext _context;

        public SubjectsController(ElectronicJournalContext context)
        {
            _context = context;
        }

        // GET: Subjects
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var groupId = _context.Users.First(m => m.UserName == User.Identity.Name).GroupID;
            var subjects = _context.Subject.Where(m => m.GroupID == groupId).OrderBy(s => s.SubjectName);

            return View(await subjects.ToListAsync());
        }

        // GET: Subjects/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subject
                .SingleOrDefaultAsync(m => m.ID == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        [Authorize(Roles = "GroupLeader")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GroupLeader")]
        public async Task<IActionResult> Create([Bind("ID,SubjectName,Teacher")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                subject.GroupID = _context.Users.First(m => m.UserName == User.Identity.Name).GroupID;
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subjects/Edit/5
        [Authorize(Roles = "GroupLeader")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subject.SingleOrDefaultAsync(m => m.ID == id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GroupLeader")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SubjectName,Teacher,GroupID")] Subject subject)
        {
            if (id != subject.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.ID))
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
            return View(subject);
        }

        // GET: Subjects/Delete/5
        [Authorize(Roles = "GroupLeader")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subject
                .SingleOrDefaultAsync(m => m.ID == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GroupLeader")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subject.SingleOrDefaultAsync(m => m.ID == id);
            _context.Subject.Remove(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
            return _context.Subject.Any(e => e.ID == id);
        }
    }
}
