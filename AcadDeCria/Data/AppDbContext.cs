using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AcadDeCria.Models;

namespace AcadDeCria.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Adicione DbSets para suas entidades, por exemplo:
        public DbSet<User> users { get; set; }
        public DbSet<Calculator> calculators { get; set; }
    }
}
