using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Models
{
    public enum TypeLesson
    {
        [Display(Name = "Лекція")]
        lecture,
        [Display(Name = "Практична")]
        practical,
        [Display(Name = "Лабораторна")]
        laboratory
    }

    public class Lesson
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public string Classroom { get; set; }
        public TypeLesson Type { get; set; }

        public int SubjectID { get; set; }
        public Subject Subject { get; set; }

        public ICollection<Missing> Missings { get; set; }

        //ще додати предмет, н-ки, студенти

    }
}
