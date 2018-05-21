using ElectronicJournal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.ViewModels
{
    public class CreateUserViewModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int? GroupID { get; set; }
        public Group Group { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
