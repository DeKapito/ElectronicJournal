using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectronicJournal.Models;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace ElectronicJournal.Controllers
{
    public class LessonsController : Controller, IDataController
    {
        private readonly ElectronicJournalContext _context;

        public LessonsController(ElectronicJournalContext context)
        {
            _context = context;
        }

        // GET: Lessons
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var groupId = _context.Users.First(m => m.UserName == User.Identity.Name).GroupID;
            var electronicJournalContext = _context.Lesson.Where(m => m.GroupID == groupId).Include(l => l.Subject)
                                                            .OrderBy(l => l.Date)
                                                            .GroupBy(l => l.Date);                                                    

            return View(await electronicJournalContext.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> IndexPagging(int? id = 1)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            int numberOfWeek = (int)id;

            var groupId = _context.Users.First(m => m.UserName == User.Identity.Name).GroupID;
            var weeks = _context.Lesson.Where(m => m.GroupID == groupId).Include(l => l.Subject)
                                                            .OrderBy(l => l.Date)
                                                            .GroupBy(l => l.Date.StartOfWeek(DayOfWeek.Monday)).ToList();

            if(numberOfWeek > weeks.Count)
            {
                return View("DataNotFound");
            }

            var week = weeks[numberOfWeek - 1];
            ViewBag.numberOfWeek = numberOfWeek;
            ViewBag.numWeeks = weeks.Count;

            return View(week.ToList());
        }

        // GET: Lessons/Details/5
        [Authorize]
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
            var groupId = _context.Users.First(m => m.UserName == User.Identity.Name).GroupID;
            ViewBag.students = await _context.Student.Where(s => s.GroupID == groupId).OrderBy(m => m.LastName).ToListAsync();
            ViewBag.missings = await _context.Missing.Where(l => l.LessonID == id).Where(l => l.Student.GroupID == groupId).OrderBy(l => l.Student.LastName).ToListAsync();
            var missings = await _context.Missing.Where(l => l.LessonID == id).OrderBy(l => l.Student.LastName).ToListAsync();

            //Якщо н-ки не виставлені
            if (missings.Count == 0)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // GET: Lessons/Create
        [Authorize(Roles = "GroupLeader")]
        public IActionResult Create()
        {
            var groupId = _context.Users
                                .First(m => m.UserName == User.Identity.Name)
                                .GroupID;

            ViewData["SubjectID"] = new SelectList(_context.Subject
                                                            .Where(m => m.GroupID == groupId), "ID", "SubjectName");
            ViewBag.Students = _context.Student
                                        .Where(m => m.GroupID == groupId)
                                        .OrderBy(m => m.LastName)
                                        .ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GroupLeader")]
        public async Task<IActionResult> Create([Bind("ID,Date,Classroom,Type,SubjectID,NumberLesson,Missings,GroupID")] Lesson lesson)
        {
            if (ModelState.IsValid)
            { 
                lesson.GroupID = _context.Users
                                        .First(m => m.UserName == User.Identity.Name)
                                        .GroupID;
                _context.Add(lesson);

                var students = _context.Student.OrderBy(m => m.LastName).ToList();

                List<Missing> missings = new List<Missing>(students.Count);
                for (int i = 0; i < students.Count; i++)
                {
                    missings.Add(new Missing());
                    missings[i].LessonID = lesson.ID;
                    missings[i].StudentID = students[i].ID;
                    missings[i].IsMissing = 0;
                    _context.Add(missings[i]);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SubjectID"] = new SelectList(_context.Subject, "ID", "ID", lesson.SubjectID);
            return View(lesson);
        }

        // GET: Lessons/Edit/5
        [Authorize(Roles = "GroupLeader")]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GroupLeader")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Date,Classroom,Type,SubjectID, NumberLesson, GroupID")] Lesson lesson)
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
        [Authorize(Roles = "GroupLeader")]
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

            var groupId = _context.Users.First(m => m.UserName == User.Identity.Name).GroupID;
            ViewBag.lesson = lesson;
            ViewBag.students = await _context.Student.Where(m => m.GroupID == groupId).OrderBy(m => m.LastName).ToListAsync();

            List<Missing> missings = new List<Missing>();
            foreach (Student item in ViewBag.students)
            {
                Missing temp = new Missing();
                missings.Add(temp);
            }

            return View(missings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GroupLeader")]
        public async Task<IActionResult> AddMissings(List<Missing> missings, Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                var groupId = _context.Users.First(m => m.UserName == User.Identity.Name).GroupID;
                var students = await _context.Student.Where(m => m.GroupID == groupId).OrderBy(m => m.LastName).ToListAsync();

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
        [Authorize(Roles = "GroupLeader")]
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
        [Authorize(Roles = "GroupLeader")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lesson = await _context.Lesson.SingleOrDefaultAsync(m => m.ID == id);
            _context.Lesson.Remove(lesson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexPagging));
        }

        private bool LessonExists(int id)
        {
            return _context.Lesson.Any(e => e.ID == id);
        }
    }
}
