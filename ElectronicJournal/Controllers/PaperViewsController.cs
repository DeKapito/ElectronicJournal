using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Models;
using ElectronicJournal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.Controllers
{
    public class PaperViewsController : Controller
    {
        private readonly ElectronicJournalContext _context;
        private IHostingEnvironment _hostingEnvironment;

        public PaperViewsController(ElectronicJournalContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize]
        public IActionResult Index(int? id = 1)
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

            if (numberOfWeek > weeks.Count)
            {
                return View("~/Views/Lessons/DataNotFound.cshtml");
            }

            PaperViewModel paperViewModel = new PaperViewModel
            {
                Students = _context.Student.Where(s => s.GroupID == groupId).OrderBy(s => s.LastName).ToList(),
                Lessons = weeks[numberOfWeek - 1].ToList(),
                Missings = _context.Missing.ToList()
            };

            ViewBag.id = id;
            ViewBag.numberOfWeek = numberOfWeek;
            ViewBag.numWeeks = weeks.Count;

            return View(paperViewModel);
        }
    }
}