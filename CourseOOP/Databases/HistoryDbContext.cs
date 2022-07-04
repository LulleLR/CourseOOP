using CourseOOP.Databases.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace CourseOOP.Databases
{
    internal class HistoryDbContext : DbContext
    {
        public DbSet<Request> Requests { get; set; }
        public string DbPath { get; set; }

        public HistoryDbContext()
        {
            string folder = Environment.CurrentDirectory;
            DbPath = Path.Combine(folder, "History.db");
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }
    }
}
