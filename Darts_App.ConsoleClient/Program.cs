using Darts_App.Logic;
using Darts_App.Models;
using Darts_App.Repository;
using System;

namespace Darts_App.ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DartsDbContext ctx = new DartsDbContext();
            IRepository<Player> playerRepo = new PlayerRepository(ctx);
            IRepository<Game> gameRepo = new GameRepository(ctx);
            IPlayerLogic playerLogic = new PlayerLogic(playerRepo);
            IGameLogic gameLogic = new GameLogic(gameRepo);

            //Player p1 = new Player();
            //p1.Name = "alma";
            //p1.Password = "alma";
            //p1.GamesWinCount = 1;
            //Game g1 = new Game();
            //playerLogic.Create(p1);
            //Player output = playerLogic.SignIn("alma", "alma");
            //Console.WriteLine(output.GamesWinCount);


            foreach (var item in ctx.Players)
            {
                Console.WriteLine($"{item.Id} {item.Name}, {item.Games.Count}");
            }
            Console.WriteLine();
            foreach (var item in ctx.Games)
            {
                Console.WriteLine($"{item.Id} {item.WinnerId}");
            }
            Console.WriteLine();
            foreach (var item in ctx.PlayerGameConnection)
            {
                Console.WriteLine($"{item.Id} {item.Game.Id} {item.Player.Name}");
            }


            playerLogic.Create(new Player()
            {
                Name = "Aladar",
                Password = "Aladar",

            });
            ctx.PlayerGameConnection.Add(new PlayerGameConnection
            {
                GameId = 2,
                PlayerId = 3,
            });
            ctx.PlayerGameConnection.Add(new PlayerGameConnection
            {
                GameId = 2,
                PlayerId = 4,
            });
            ctx.SaveChanges();
            Console.WriteLine();
            foreach (var item in ctx.PlayerGameConnection)
            {
                Console.WriteLine($"{item.Id} {item.Game.Id} {item.Player.Id} {item.Player.Name}");
            }
            Console.WriteLine();
            foreach (var item in playerLogic.ReadAll())
            {
                Console.WriteLine($"{item.Name}");
                foreach (var item2 in item.Games)
                {
                    Console.WriteLine($" {item2.Id}");
                }
            }

        }
    }
}
