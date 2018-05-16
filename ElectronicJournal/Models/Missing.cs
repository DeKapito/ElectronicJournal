using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Models
{
    public enum IsMissing
    {
        [Display(Name = "Присутній")]
        present,
        [Display(Name = "нп - неповажний пропуск")]
        withoutReason,
        [Display(Name = "нп - хворий")]
        illness
    }

    public class Missing
    {
        public int ID { get; set; }

        public int StudentID { get; set; }
        public Student Student { get; set; }

        public IsMissing IsMissing { get; set; }

        public int LessonID { get; set; }
        public Lesson Lesson { get; set; }
    }
}
