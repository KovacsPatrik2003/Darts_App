using Darts_App.Logic;
using Darts_App.Models;
using Darts_App.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Darts_App.ConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DartsDbContext ctx = new DartsDbContext();
            IRepository<Player> playerRepo = new PlayerRepository(ctx);
            IRepository<Game> gameRepo = new GameRepository(ctx);
            IRepository<PlayerGameConnection> playergameRepo = new PlayerGameConnectionRepository(ctx);
            IPlayerLogic playerLogic = new PlayerLogic(playerRepo);
            IGameLogic gameLogic = new GameLogic(gameRepo,playergameRepo);
            List<Player> trygamePlayers = new List<Player>();
            trygamePlayers.Add(playerLogic.SignIn("Patrik", "Patrik"));
            trygamePlayers.Add(playerLogic.SignIn("Adam", "Adam"));
            Game game = new Game();
            gameLogic.GetSets += GameLogic_GetSets;
            gameLogic.GetLegs += GameLogic_GetLegs;
            gameLogic.GetStartPoint += GameLogic_GetStartPoint;
            gameLogic.GetChek_out += GameLogic_GetChek_out;
            gameLogic.OngoingGamePoints += GameLogic_OngoingGamePoints;
            gameLogic.GameSession(trygamePlayers, game);
            Console.WriteLine($"Winner: {playerLogic.Read(game.WinnerId).Name}");
        }

        private static int GameLogic_OngoingGamePoints(Player p, List<Player> L, Game g)
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < L.Count; i++)
            {
                Console.WriteLine($"Name: {L[i].Name}, Sets:{g.Sets[i]}, Legs:{g.Legs[i]}, Points:{L[i].CurrentPoints.ToString().PadRight(20, ' ')}");
            }
            Console.WriteLine($"Sets: {g.SetCount}\nLegs: {g.LegCount}");
            Console.WriteLine($"Enter the points what {p.Name} scrored: "+"       ");
            (int x, int y) = Console.GetCursorPosition();
            Console.SetCursorPosition(0, y);
            Console.WriteLine("                       ");
            Console.SetCursorPosition(0, y);
            return int.Parse(Console.ReadLine());
        }

        private static string GameLogic_GetChek_out()
        {
            Console.Clear();
            Console.WriteLine("Enter the cheout method: ");
            return Console.ReadLine();
        }

        private static int GameLogic_GetStartPoint()
        {
            Console.Clear();
            Console.WriteLine("Enter the startpoint: ");
            return int.Parse(Console.ReadLine());
        }

        private static int GameLogic_GetLegs()
        {
            Console.Clear();
            Console.WriteLine("Enter the number of legs: ");
            return int.Parse(Console.ReadLine());
        }

        private static int GameLogic_GetSets()
        {
            Console.Clear();
            Console.WriteLine("Enter the number of sets: ");
            return int.Parse(Console.ReadLine());;
        }
    }
}
