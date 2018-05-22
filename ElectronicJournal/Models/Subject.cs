using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Models
{
    public class Subject
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string SubjectName { get; set; }

        [Required]
        [StringLength(50)]
        public string Teacher { get; set; }

        public int? GroupID { get; set; }
        public virtual Group Group { get; set; }

        public ICollection<Lesson> Lessons { get; set; }
    }
}
