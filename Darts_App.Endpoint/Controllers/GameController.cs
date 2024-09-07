using Darts_App.Endpoint.Services;
using Darts_App.Logic;
using Darts_App.Models;
using Darts_App.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Darts_App.Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {

        private readonly IGameLogic logic;
        private readonly IHubContext<SignalRHub> hub;

        public GameController(IGameLogic logic, IHubContext<SignalRHub> hubContext)
        {
            this.logic = logic;
            this.hub = hubContext;
            
        }

        // GET: api/<GameController>
        [HttpGet]
        public IEnumerable<Game> ReadAll()
        {
            return this.logic.ReadAll();
        }

        // GET api/<GameController>/5
        [HttpGet("{id}")]
        public Game Read(int id)
        {
            return logic.Read(id);
        }

        // POST api/<GameController>
        [HttpPost]
        public void Create([FromBody] Game value)
        {
            this.logic.Create(value);
        }

        // DELETE api/<GameController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.logic.Delete(id);
        }

        [HttpPost("start-game-session/{setCount}/{legCount}/{startPoints}/{checkOutMethod}")]
        public async Task<IActionResult> StartGameSession([FromBody] GameSessionRequest request, int setCount, int legCount, int startPoints, string checkOutMethod)
        {
            var players = request.Players;
            Task gameSessionTask = new Task(async() =>
            {
                await this.logic.GameSession(players, setCount, legCount, startPoints, checkOutMethod);
            });

            gameSessionTask.Start();
           
            return Ok("Ciklus futtatása elkezdődött");
        }

        [HttpGet("points/{points}")]
        public async Task ThrowedPoints(int points)
        {
            await this.hub.Clients.All.SendAsync("ThrowHappend");
            ;
            return;
        }
        [HttpPost("IgnorGameSession")]
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
                            for (int l = 0; l < 3; l++)
                            {

                                // Felhasználói pontszám lekérése a SignalR-en keresztül
                                //var playerPointQueue = _gameStateService.GetPlayerPoints(players[k].Id);
                                int throwedPoint=0;
                               


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
           
        }
        [HttpPost("IgnorDecrement")]
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
    }
    public class GameSessionRequest
    {
        public List<Player> Players { get; set; }
    }
}
