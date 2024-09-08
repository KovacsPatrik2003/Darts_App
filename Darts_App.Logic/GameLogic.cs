using Darts_App.Endpoint.Services;
using Darts_App.Models;
using Darts_App.Repository;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Darts_App.Logic
{
    public delegate int GameLogicDelegate();
    public delegate int OnGoingDelegate(Player p, List<Player> L, Game g);
    public delegate string GameLogicDelegateStirng();
    public delegate void GameLogicDelegateWinner(int winnerId);
    public delegate Task<int> Dobas(int p);
    public class GameLogic : IGameLogic
    {
        IRepository<Game> repo;
        IRepository<PlayerGameConnection> connectionRepo;
        public event GameLogicDelegate GetSets;
        public event GameLogicDelegate GetLegs;
        public event GameLogicDelegate GetStartPoint;
        public event GameLogicDelegateStirng GetChek_out;
        public event OnGoingDelegate OngoingGamePoints;
        public event GameLogicDelegateWinner Winner;


        private readonly IHubContext<SignalRHub> _hubContext;
        private string _receivedData;


        public GameLogic(IRepository<Game> repo, IRepository<PlayerGameConnection> connectionRepo, IHubContext<SignalRHub> hubContext)
        {
            this.repo = repo;
            this.connectionRepo = connectionRepo;
            _hubContext = hubContext;
        }

        public void Create(Game item)
        {
            this.repo.Create(item);
        }

        public void Delete(int id)
        {
            this.repo.Delete(id);
        }

        public Game Read(int id)
        {
            return this.repo.Read(id);
        }

        public IQueryable<Game> ReadAll()
        {
            return this.repo.ReadAll();
        }
        public async Task<int> pointsScored(int points=0)
        {
            
            return points;
        }
        
        public async Task GameSession(List<Player> players, int setCount, int legCount, int startPoints, string checkOutMethod)
        {
            Game game = new Game();

            //create connections between the tables
            for (int k = 0; k < players.Count; k++)
            {
                //connectionRepo.Create(new PlayerGameConnection()
                //{
                //    GameId = game.Id,
                //    PlayerId = players[k].Id
                //});
                game.Sets.Add(0);
                game.Legs.Add(0);
            }
            //game session

            //get sets from clientúawait SendMessageAsync(webSocket, "RequestSets");
            game.SetCount = setCount;

            //get legs from client
            game.LegCount = legCount;


            //get point from client
            game.StartPoints = startPoints;
            int? fixpoints = game.StartPoints;


            //get cheout mode from client
            game.Check_Out = checkOutMethod;

            //game session start
            bool finish = false;
            int max = 0;
            //set counter
            while (game.Sets.All(x => x != game.SetCount))
            {
                for (int i = 0; i < game.Legs.Count; i++)
                {
                    game.Legs[i] = 0;
                }
                //leg counter
                while (game.Legs.All(x => x < game.LegCount))
                {
                    //new leg start
                    for (int k = 0; k < players.Count; k++)
                    {
                        players[k].CurrentPoints = (int)fixpoints;
                    }
                    while (players.All(x => x.CurrentPoints > 0))
                    {
                        for (int k = 0; k < players.Count; k++)
                        {
                            int currentpoints = players[k].CurrentPoints;
                            int debugValue = 0;
                            for (int l = 0; l < 3; l++)
                            {

                                // Felhasználói pontszám lekérése a SignalR-en keresztül
                                var data = await WaitForDataFromClient();
                                int throwedPoint = int.Parse(data);
                                debugValue += throwedPoint;
                                int feedback = Pointdecrementation(throwedPoint, game.Check_Out, players[k].CurrentPoints);
                                if (feedback == -1)
                                {
                                    players[k].CurrentPoints = currentpoints;
                                    break;
                                }
                                else
                                {
                                    players[k].CurrentPoints -= feedback;
                                }
                                if (players[k].CurrentPoints == 0)
                                {
                                    finish = true;
                                    game.Legs[k]++;
                                    break;
                                }
                            }
                            ;
                            if (finish)
                            {
                                break;
                            }
                        }
                    }
                    finish = false;
                }
                max = 0;
                for (int i = 0; i < game.Legs.Count; i++)
                {
                    if (game.Legs[max] < game.Legs[i])
                    {
                        max = i;
                    }
                }
                game.Sets[max]++;
            }

            // game session end
            max = 0;
            for (int i = 0; i < game.Sets.Count; i++)
            {
                if (game.Sets[max] < game.Sets[i])
                {
                    max = i;
                }
            }
            game.WinnerId = players[max].Id;
            Winner?.Invoke(game.WinnerId);
        }

        private int Pointdecrementation(int point, string chekoutmethod, int actualpoint)
        {
            if (point < 0)
            {
                throw new Exception();
            }
            if (chekoutmethod == "straight")
            {
                if (actualpoint < point)
                {
                    //if overthrow
                    return -1;
                }
                else
                {
                    return point;
                }
            }
            else if (chekoutmethod == "double")
            {
                if (actualpoint < point)
                {
                    //if overthrow
                    return -1;
                }
                else
                {
                    int sub = actualpoint - point;
                    if (sub == 0 && point % 2 == 0)
                    {
                        return point;
                    }
                    else
                    {
                        if (sub > 1)
                        {
                            return point;
                        }
                        else
                        {
                            //if overthrow
                            return -1;
                        }
                    }
                }
            }
            //something else
            return -404;
        }
        private Task<string> WaitForDataFromClient()
        {
            // Új TaskCompletionSource létrehozása

            SignalRHub.tcs = new TaskCompletionSource<string>();

            // Esemény a kliens adatának kérésére
            _hubContext.Clients.All.SendAsync("RequestData", "Kérlek küldj adatot!");

            // Várakozás amíg az adat megérkezik
            return SignalRHub.tcs.Task;
        }
    } 
}
