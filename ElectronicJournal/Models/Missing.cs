using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Models
{
    public class Missing
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        //public ICollection<Student> Students { get; set; }
        public bool IsMissing { get; set; }
        public int LessonID { get; set; }
    }
}
