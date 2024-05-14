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
        public DbSet<PlayerGameConnection> PlayerGameConnection { get; set; }

        public DartsDbContext()
        {
            this.Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //string conn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DartsDb.mdf;Integrated Security=True;MultipleActiveResultSets=True";
                //optionsBuilder.UseSqlServer(conn).UseLazyLoadingProxies();
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseInMemoryDatabase("Darts");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasMany(x => x.Games)
                .WithMany(x => x.Players)
                .UsingEntity<PlayerGameConnection>(
                    x => x.HasOne(x => x.Game)
                          .WithMany().HasForeignKey(x => x.GameId).OnDelete(DeleteBehavior.Cascade),

                    x => x.HasOne(x => x.Player)
                          .WithMany().HasForeignKey(x => x.PlayerId).OnDelete(DeleteBehavior.Cascade)
                );


            List<Player> playerlist = new List<Player>();
            List<Game> gamelist = new List<Game>();
            List<PlayerGameConnection> playerGameConnections = new List<PlayerGameConnection>();

            playerlist.Add(new Player()
            {
                Id = 1,
                Name = "Patrik",
                Password = "Patrik",
                
            }
            );
            playerlist.Add(new Player()
            {
                Id = 2,
                Name = "Adam",
                Password = "Adam"
            }
            );
            playerlist.Add(new Player()
            {
                Id = 3,
                Name = "Balint",
                Password = "Balint"
            }
            );
            gamelist.Add(new Game()
            {
                Id = 1,
                WinnerId = 1,
            });
            gamelist.Add(new Game()
            {
                Id = 2,
                WinnerId = 2,
            });
            gamelist.Add(new Game()
            {
                Id = 3,
                WinnerId = 3,
            });
            playerGameConnections.Add(new PlayerGameConnection
            {
                Id = 1,
                GameId = 1,
                PlayerId = 1,
            });
            playerGameConnections.Add(new PlayerGameConnection
            {
                Id = 2,
                GameId = 1,
                PlayerId = 2,
            });
            modelBuilder.Entity<Player>().HasData(playerlist);
            modelBuilder.Entity<Game>().HasData(gamelist);
            modelBuilder.Entity<PlayerGameConnection>().HasData(playerGameConnections);
        }
    }
}
