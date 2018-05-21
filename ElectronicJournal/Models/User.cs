using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
      //  public string Group { get; set; }

        public int? GroupID { get; set; }
        public virtual Group Group { get; set; }
    }
}
