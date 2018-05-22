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

    public enum NumberLesson
    {
        [Display(Name = "1")]
        first = 1,
        [Display(Name = "2")]
        second,
        [Display(Name = "3")]
        third,
        [Display(Name = "4")]
        fourth,
        [Display(Name = "5")]
        fifth,
        [Display(Name = "6")]
        sixth,
        [Display(Name = "7")]
        seventh,
        [Display(Name = "8")]
        eighth
    }

    public class Lesson
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [StringLength(10)]
        public string Classroom { get; set; }

        public TypeLesson Type { get; set; }
        public NumberLesson NumberLesson { get; set; }

        public int SubjectID { get; set; }
        public Subject Subject { get; set; }

        public int? GroupID { get; set; }
        public virtual Group Group { get; set; }

        public ICollection<Missing> Missings { get; set; }
    }
}
