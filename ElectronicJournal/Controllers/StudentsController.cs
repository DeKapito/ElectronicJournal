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
    public class StudentsController : Controller, IDataController
    {
        private readonly ElectronicJournalContext _context;

        public StudentsController(ElectronicJournalContext context)
        {
            _context = context;
        }

        // GET: Students
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var groupId = _context.Users.First(m => m.UserName == User.Identity.Name).GroupID;
            var students = _context.Student.Where(m => m.GroupID == groupId).OrderBy(s => s.LastName);

            return View(await students.ToListAsync());
        }

        // GET: Students/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .SingleOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        [Authorize(Roles = "GroupLeader")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GroupLeader")]
        public async Task<IActionResult> Create([Bind("ID,Name,LastName, Father")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.GroupID = _context.Users.First(m => m.UserName == User.Identity.Name).GroupID;
                _context.Add(student);

                var someStudent = await _context.Student.FirstOrDefaultAsync(m => m.GroupID == student.GroupID);
                if (someStudent != null)
                {
                    var missings = _context.Missing.Where(m => m.StudentID == someStudent.ID).ToList();

                    for (int i = 0; i < missings.Count; i++)
                    {
                        _context.Add(new Missing
                        {
                            StudentID = student.ID,
                            LessonID = missings[i].LessonID,
                            IsMissing = IsMissing.withoutReason
                        });
                    }
                }     

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "GroupLeader")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.SingleOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GroupLeader")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,LastName,Father,GroupID")] Student student)
        {
            if (id != student.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
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
            return View(student);
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "GroupLeader")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .SingleOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GroupLeader")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.SingleOrDefaultAsync(m => m.ID == id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.ID == id);
        }
    }
}
