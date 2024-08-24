using Darts_App.Endpoint.Services;
using Darts_App.Logic;
using Darts_App.Models;
using Darts_App.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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

        IGameLogic logic;
        IHubContext<SignalRHub> hub;

        public GameController(IGameLogic logic, IHubContext<SignalRHub> hub)
        {
            this.logic = logic;
            this.hub = hub;
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
        public void StartGameSession([FromBody] GameSessionRequest request, int setCount, int legCount, int startPoints, string checkOutMethod)
        {
            var players = request.Players;
            this.logic.GameSession(players, setCount, legCount, startPoints, checkOutMethod);
            this.hub.Clients.All.SendAsync("GameSessionStarted");
        }

        [HttpGet("points/{points}")]
        public async Task ThrowedPoints(int points)
        {
            ;
            
            this.logic.ScoredPoints = points;
            await this.hub.Clients.All.SendAsync("ThrowHappend");
            ;
            return;
        }

    }
    public class GameSessionRequest
    {
        public List<Player> Players { get; set; }
    }
}
