using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Models
{
    public class Group
    {
        public int ID { get; set; }

        [Required]
        [StringLength(9)]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
