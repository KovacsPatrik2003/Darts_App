using Darts_App.Models;
using Darts_App.Repository;
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
        public GameLogic(IRepository<Game> repo, IRepository<PlayerGameConnection> connectionRepo)
        {
            this.repo = repo;
            this.connectionRepo = connectionRepo;
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

        public async Task GameSession(List<Player> players, WebSocket webSocket)
        {
            Game game = new Game();
            this.Create(game);
            //create connections between the tables
            for (int k = 0; k < players.Count; k++)
            {
                connectionRepo.Create(new PlayerGameConnection()
                {
                    GameId = game.Id,
                    PlayerId = players[k].Id
                });
                game.Sets.Add(0);
                game.Legs.Add(0);
            }
            //game session

            //get sets from clientúawait SendMessageAsync(webSocket, "RequestSets");
            int set = int.Parse(await ReceiveMessageAsync(webSocket));
            //game.SetCount =(int) GetSets?.Invoke();
            game.SetCount = set;


            //get legs from client
            await SendMessageAsync(webSocket, "RequestLegs");
            int leg = int.Parse(await ReceiveMessageAsync(webSocket));
            //game.LegCount =(int) GetLegs?.Invoke();
            game.LegCount = leg;


            //get point from client
            await SendMessageAsync(webSocket, "RequestStartPoints");
            game.StartPoints = int.Parse(await ReceiveMessageAsync(webSocket));
            //game.StartPoints = GetStartPoint?.Invoke();
            int? fixpoints = game.StartPoints;


            //get cheout mode from client
            await SendMessageAsync(webSocket, "RequestCheckOutMode");
            game.Check_Out = await ReceiveMessageAsync(webSocket);
            //game.Check_Out = GetChek_out?.Invoke();

            //game session start
            bool finish = false;
            int max = 0;
            //set counter
            while (game.Sets.All(x=>x!=game.SetCount))
            {
                for (int i = 0; i < game.Legs.Count; i++)
                {
                    game.Legs[i] = 0;
                }
                //leg counter
                while (game.Legs.All(x=>x<game.LegCount))
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
                            for (int l = 0; l < 3; l++)
                            {
                                await SendMessageAsync(webSocket, $"OngoingGamePoints:{players[k].Id}");
                                int notZeroResultCheck = int.Parse(await ReceiveMessageAsync(webSocket));
                                //int notZeroResultChek = (int)OngoingGamePoints?.Invoke(players[k], players, game);
                                int feedback = Pointdecrementation(notZeroResultCheck, game.Check_Out, players[k].CurrentPoints);
                                if (feedback ==-1)
                                {
                                    players[k].CurrentPoints=currentpoints;
                                    break;
                                }
                                else
                                {
                                    players[k].CurrentPoints-=feedback;
                                }
                                if (players[k].CurrentPoints == 0)
                                {
                                    finish = true;
                                    game.Legs[k]++;
                                    break;
                                }
                            }
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

        private async Task SendMessageAsync(WebSocket webSocket, string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task<string> ReceiveMessageAsync(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            return Encoding.UTF8.GetString(buffer, 0, result.Count);
        }

        private int Pointdecrementation(int point, string chekoutmethod, int actualpoint)
        {
            if (chekoutmethod=="Straight Out")
            {
                if (actualpoint<point)
                {
                    //if overthrow
                    return -1;
                }
                else
                {
                    return point;
                }
            }
            else if(chekoutmethod == "Double Out")
            {
                if (actualpoint<point)
                {
                    //if overthrow
                    return -1;
                }
                else
                {
                    int sub=actualpoint-point;
                    if (sub==0 && point%2==0)
                    {
                        return point;
                    }
                    else
                    {
                        if (sub>1)
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
    } 
}
