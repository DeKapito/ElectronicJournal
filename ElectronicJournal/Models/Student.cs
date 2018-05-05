using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Models
{
    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public ICollection<Missing> Missings { get; set; }
    }
}
