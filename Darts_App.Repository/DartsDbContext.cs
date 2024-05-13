using Darts_App.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darts_App.Repository
{
    public class DartsDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }

        public DartsDbContext()
        {
            this.Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //string conn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DartsDatabase.mdf;Integrated Security=True";
                //optionsBuilder.UseSqlServer(conn).UseLazyLoadingProxies();
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseInMemoryDatabase("Darts");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                        .HasMany(p => p.Games)
                        .WithOne(g => g.Player)
                        .HasForeignKey(g => g.PlayerId)
                        .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
