using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Models
{
    public class Week
    {
        public int ID { get; set; }
        public DateTime FirstDayWeek { get; set; }
        public DateTime LastDayWeek { get; set; }

        //public int LessonID { get; set; }
        //public Lesson Lesson { get; set; }

        //public int MissingID { get; set; }
        //public Missing Missing { get; set; }

        public ICollection<Student> Students { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
    }
}
