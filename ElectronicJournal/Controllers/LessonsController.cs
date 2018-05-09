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
    public class LessonsController : Controller
    {
        private readonly ElectronicJournalContext _context;

        public LessonsController(ElectronicJournalContext context)
        {
            _context = context;
        }

        // GET: Lessons
        public async Task<IActionResult> Index()
        {
            var electronicJournalContext = _context.Lesson.Include(l => l.Subject).OrderBy(l => l.Date);

            return View(await electronicJournalContext.ToListAsync());
        }

        // GET: Lessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lesson
                .Include(l => l.Subject)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // GET: Lessons/Create
        public IActionResult Create()
        {
            ViewData["SubjectID"] = new SelectList(_context.Subject, "ID", "SubjectName");
            ViewBag.Students = _context.Student.OrderBy(m => m.LastName).ToList();

            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Date,Classroom,Type,SubjectID,NumberLesson,Missings")] Lesson lesson)
        {
            if (ModelState.IsValid)
            { 
                _context.Add(lesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SubjectID"] = new SelectList(_context.Subject, "ID", "ID", lesson.SubjectID);
            return View(lesson);
        }

        // GET: Lessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lesson.SingleOrDefaultAsync(m => m.ID == id);
            if (lesson == null)
            {
                return NotFound();
            }
            ViewData["SubjectID"] = new SelectList(_context.Subject, "ID", "SubjectName", lesson.SubjectID);
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Date,Classroom,Type,SubjectID, NumberLesson")] Lesson lesson)
        {
            if (id != lesson.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lesson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonExists(lesson.ID))
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
            ViewData["SubjectID"] = new SelectList(_context.Subject, "ID", "ID", lesson.SubjectID);
            return View(lesson);
        }

        // GET: Missings1/Create
        public async Task<IActionResult> AddMissings(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lesson.SingleOrDefaultAsync(m => m.ID == id);

            if (lesson == null)
            {
                return NotFound();
            }

            ViewBag.lesson = lesson;
            ViewBag.students = await _context.Student.OrderBy(m => m.LastName).ToListAsync();

            List<Missing> missings = new List<Missing>();
            foreach (Student item in ViewBag.students)
            {
                Missing temp = new Missing();
                missings.Add(temp);
            }

            //ViewData["SubjectID"] = new SelectList(_context.Subject, "ID", "ID", lesson.SubjectID);
            //ViewData["StudentID"] = new SelectList(_context.Student, "ID", "ID");
            return View(missings);
        }

        //POST: Missings1/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMissings(List<Missing> missings, Lesson lesson)   //public async Task<IActionResult> AddMissings([Bind("ID,StudentID,IsMissing,LessonID")] Missing missing)
        {
            if (ModelState.IsValid)
            {
                var students = await _context.Student.OrderBy(m => m.LastName).ToListAsync();

                var missingsToDelete = _context.Missing.Where(m => m.LessonID == lesson.ID);
                foreach(var item in missingsToDelete)
                {
                    _context.Remove(item);
                }

                for (int i = 0; i < missings.Count; i++)
                {
                    missings[i].LessonID = lesson.ID;
                    missings[i].StudentID = students[i].ID;
                    _context.Add(missings[i]);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: Lessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lesson
                .Include(l => l.Subject)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lesson = await _context.Lesson.SingleOrDefaultAsync(m => m.ID == id);
            _context.Lesson.Remove(lesson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LessonExists(int id)
        {
            return _context.Lesson.Any(e => e.ID == id);
        }
    }
}
