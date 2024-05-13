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

            Player p1 = new Player();
            p1.Name = "alma";
            p1.Password = "alma";
            p1.GamesWinCount = 1;
            Game g1 = new Game();
            playerLogic.Create(p1);
            Player output = playerLogic.SignIn("alma", "alma");
            Console.WriteLine(output.GamesWinCount);
        }
    }
}
