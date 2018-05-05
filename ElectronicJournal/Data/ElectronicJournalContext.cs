using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.Models
{
    public class ElectronicJournalContext : DbContext
    {
        public ElectronicJournalContext (DbContextOptions<ElectronicJournalContext> options)
            : base(options)
        {
        }

        public DbSet<ElectronicJournal.Models.Student> Student { get; set; }
    }
}
