using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ElectronicJournal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ElectronicJournal.Models
{
    public class ElectronicJournalContext : IdentityDbContext<User>
    {
        public ElectronicJournalContext (DbContextOptions<ElectronicJournalContext> options)
            : base(options)
        {
        }

        public DbSet<ElectronicJournal.Models.Student> Student { get; set; }

        public DbSet<ElectronicJournal.Models.Subject> Subject { get; set; }

        public DbSet<ElectronicJournal.Models.Missing> Missing { get; set; }

        public DbSet<ElectronicJournal.Models.Lesson> Lesson { get; set; }

        public DbSet<ElectronicJournal.Models.Group> Group { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
