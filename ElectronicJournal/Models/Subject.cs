using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Models
{
    public class Subject
    {
        public int ID { get; set; }
        public string SubjectName { get; set; }
        public string Teacher { get; set; }
        //public string Type { get; set; }    //Тип заняття (практична, лекція і т.д.)
    }
}
