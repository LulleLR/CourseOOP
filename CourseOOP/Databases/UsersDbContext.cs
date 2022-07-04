using CourseOOP.Databases.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace CourseOOP.Databases
{
    internal class UsersDbContext : DbContext
    {
        public DbSet<Client> Users { get; set; }
        public string DbPath { get; set; }

        public UsersDbContext()
        {
            string folder = Environment.CurrentDirectory;
            DbPath = Path.Combine(folder, "Users.db");
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }
    }
}
