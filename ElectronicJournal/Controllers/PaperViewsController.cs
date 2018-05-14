using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.Controllers
{
    public class PaperViewsController : Controller
    {
        private readonly ElectronicJournalContext _context;

        public PaperViewsController(ElectronicJournalContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? id = 1)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }

            int numberOfWeek = (int)id;

            var weeks = _context.Lesson.Include(l => l.Subject)
                                                            .OrderBy(l => l.Date)
                                                            .GroupBy(l => l.Date.StartOfWeek(DayOfWeek.Monday)).ToList();

            if (numberOfWeek > weeks.Count)
            {
                return NotFound();
            }

            var week = weeks[numberOfWeek - 1];
            ViewBag.numberOfWeek = numberOfWeek;
            ViewBag.numWeeks = weeks.Count;

            return View(week.ToList());
        }
    }
}