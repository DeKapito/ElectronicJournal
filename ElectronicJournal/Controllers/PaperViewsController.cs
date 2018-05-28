using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Models;
using ElectronicJournal.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

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
                return NotFound();
            }

            PaperViewModel paperViewModel = new PaperViewModel
            {
                Students = _context.Student.Where(s => s.GroupID == groupId).OrderBy(s => s.LastName).ToList(),
                Lessons = weeks[numberOfWeek - 1].ToList(),
                Missings = _context.Missing.ToList()
            };

            ViewBag.id = id;
            ViewBag.numberOfWeek = numberOfWeek;

            return View(paperViewModel);
        }

        //public async Task<IActionResult> Export(int? id = 1)
        //{
        //    if (id == null || id <= 0)
        //    {
        //        return NotFound();
        //    }

        //    int numberOfWeek = (int)id;

        //    string rootFolder = _hostingEnvironment.WebRootPath;
        //    string fileName = "export.xlsx";
        //    string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, fileName);
        //    FileInfo file = new FileInfo(Path.Combine(rootFolder, fileName));

        //    MemoryStream memory = new MemoryStream();

        //    var groupId = _context.Users.First(m => m.UserName == User.Identity.Name).GroupID;
        //    var weeks = _context.Lesson.Where(m => m.GroupID == groupId).Include(l => l.Subject)
        //                                                    .OrderBy(l => l.Date)
        //                                                    .GroupBy(l => l.Date.StartOfWeek(DayOfWeek.Monday)).ToList();

        //    PaperViewModel paperViewModel = new PaperViewModel
        //    {
        //        Students = _context.Student.Where(s => s.GroupID == groupId).OrderBy(s => s.LastName).ToList(),
        //        Lessons = weeks[numberOfWeek - 1].ToList(),
        //        Missings = _context.Missing.ToList()
        //    };

        //    using (var fileStream = new FileStream(Path.Combine(rootFolder, fileName), FileMode.Create, FileAccess.Write))
        //    {
        //        IWorkbook workbook = new XSSFWorkbook();
        //        ISheet excelSheet = workbook.CreateSheet("One");
        //        IRow row = excelSheet.CreateRow(1);
        //        row.CreateCell(0).SetCellValue(1);

        //        workbook.Write(fileStream);
        //    }

        //    using (var stream = new FileStream(Path.Combine(rootFolder, fileName), FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;

        //    return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        //}
    }
}