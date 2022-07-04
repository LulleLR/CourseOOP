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
    internal class FavouritesDbContext : DbContext
    {
        public DbSet<Favorite> Favorites { get; set; }
        public string DbPath { get; set; }

        public FavouritesDbContext()
        {
            string folder = Environment.CurrentDirectory;
            DbPath = Path.Combine(folder, "Favourites.db");
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }
    }
}
