using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectronicJournal.Models;

namespace ElectronicJournal.Controllers
{
    public class Missings1Controller : Controller
    {
        private readonly ElectronicJournalContext _context;

        public Missings1Controller(ElectronicJournalContext context)
        {
            _context = context;
        }

        // GET: Missings1
        public async Task<IActionResult> Index()
        {
            var electronicJournalContext = _context.Missing.Include(m => m.Lesson).Include(m => m.Student);
            return View(await electronicJournalContext.ToListAsync());
        }

        // GET: Missings1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var missing = await _context.Missing
                .Include(m => m.Lesson)
                .Include(m => m.Student)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (missing == null)
            {
                return NotFound();
            }

            return View(missing);
        }

        // GET: Missings1/Create
        public IActionResult Create()
        {
            ViewData["LessonID"] = new SelectList(_context.Lesson, "ID", "ID");
            ViewData["StudentID"] = new SelectList(_context.Student.OrderBy(l => l.LastName), "ID", "LastName");
            return View();
        }

        // POST: Missings1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,StudentID,IsMissing,LessonID")] Missing missing) //Missing missing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(missing);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LessonID"] = new SelectList(_context.Lesson, "ID", "ID", missing.LessonID);
            ViewData["StudentID"] = new SelectList(_context.Student, "ID", "ID", missing.StudentID);
            return View(missing);
        }

        // GET: Missings1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var missing = await _context.Missing.SingleOrDefaultAsync(m => m.ID == id);
            if (missing == null)
            {
                return NotFound();
            }
            ViewData["LessonID"] = new SelectList(_context.Lesson, "ID", "ID", missing.LessonID);
            ViewData["StudentID"] = new SelectList(_context.Student, "ID", "ID", missing.StudentID);
            return View(missing);
        }

        // POST: Missings1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,StudentID,IsMissing,LessonID")] Missing missing)
        {
            if (id != missing.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(missing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MissingExists(missing.ID))
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
            ViewData["LessonID"] = new SelectList(_context.Lesson, "ID", "ID", missing.LessonID);
            ViewData["StudentID"] = new SelectList(_context.Student, "ID", "ID", missing.StudentID);
            return View(missing);
        }

        // GET: Missings1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var missing = await _context.Missing
                .Include(m => m.Lesson)
                .Include(m => m.Student)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (missing == null)
            {
                return NotFound();
            }

            return View(missing);
        }

        // POST: Missings1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var missing = await _context.Missing.SingleOrDefaultAsync(m => m.ID == id);
            _context.Missing.Remove(missing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MissingExists(int id)
        {
            return _context.Missing.Any(e => e.ID == id);
        }
    }
}
