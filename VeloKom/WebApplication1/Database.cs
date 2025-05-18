using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Diagnostics.Metrics;
using Microsoft.Identity.Client;
using System.Collections.Generic;

namespace VeloKom
{
    public class Database : DbContext
    {
        string connect = @"Server=localhost;Database=Cycling;Trusted_Connection=True;TrustServerCertificate=True;";
        public DbSet<Users> Users { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<Ads> Ads { get; set; }
        public DbSet<Categories> Categories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connect);
        }
    }
}
