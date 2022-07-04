using CourseOOP.Databases.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace CourseOOP.Databases
{
    internal class MyDbContext : DbContext
    {
        public DbSet<Culture> Cultures { get; set; }
        public string DbPath { get; set; }

        public MyDbContext()
        {
            string folder = Environment.CurrentDirectory;
            DbPath = Path.Combine(folder, "Cultures.db");
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }
    }
}
