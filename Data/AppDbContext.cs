using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LibraryManagementSystem.Data
{
    internal class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=LibraryBooks;Username=postgres;Password=********");
        }

        public DbSet<Book> Books { get; set; }
    }
}
