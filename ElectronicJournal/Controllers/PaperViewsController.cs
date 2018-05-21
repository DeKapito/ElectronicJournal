using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Models;
using ElectronicJournal.ViewModels;
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

            PaperViewModel paperViewModel = new PaperViewModel
            {
                Students = _context.Student.OrderBy(s => s.LastName).ToList(),
                Lessons = weeks[numberOfWeek - 1].ToList(),
                Missings = _context.Missing.ToList()
            };

            //var week = weeks[numberOfWeek - 1];
            ViewBag.numberOfWeek = numberOfWeek;
            //ViewBag.numWeeks = weeks.Count;
            //ViewBag.students = _context.Student.OrderBy(s => s.LastName).ToList();

            return View(paperViewModel);
        }

        //public IActionResult Index(int? id = 1)
        //{
        //    if (id == null || id <= 0)
        //    {
        //        return NotFound();
        //    }

        //    int numberOfWeek = (int)id;

        //    var weeks = _context.Lesson.Include(l => l.Subject)
        //                                                    .OrderBy(l => l.Date)
        //                                                    .GroupBy(l => l.Date.StartOfWeek(DayOfWeek.Monday)).ToList();

        //    if (numberOfWeek > weeks.Count)
        //    {
        //        return NotFound();
        //    }

        //    var week = weeks[numberOfWeek - 1];
        //    ViewBag.numberOfWeek = numberOfWeek;
        //    ViewBag.numWeeks = weeks.Count;
        //    ViewBag.students = _context.Student.OrderBy(s => s.LastName).ToList();

        //    return View(week.ToList());
        //}
    }
}