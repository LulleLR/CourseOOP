using CourseOOP.Databases.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseOOP.Databases
{
    internal class AddRequestsDbContext : DbContext
    {
        public DbSet<AddRequest> Requests { get; set; }
        public string DbPath { get; set; }

        public AddRequestsDbContext()
        {
            string folder = Environment.CurrentDirectory;
            DbPath = Path.Combine(folder, "AddRequests.db");
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }
    }
}
