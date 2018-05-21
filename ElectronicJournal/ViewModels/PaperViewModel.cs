using ElectronicJournal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.ViewModels
{
    public class PaperViewModel
    {
        public List<Student> Students { get; set; }
        public List<Lesson> Lessons { get; set; }
        public List<Missing> Missings { get; set; }
    }
}
