using Darts_App.Logic;
using Darts_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using TQPGSS_HFT_2023241.Endpoint.Services;

namespace Darts_App.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : Controller
    {
        IGameLogic logic;
        IHubContext<SignalRHub> hub;
        public GameController(IGameLogic logic, IHubContext<SignalRHub> hub)
        {
            this.logic = logic;
            this.hub = hub;
        }

        [HttpGet]
        public IEnumerable<Game> ReadAll()
        {
            return this.logic.ReadAll();
        }

        [HttpGet("{id}")]
        public Game Read(int id)
        {
            return this.logic.Read(id);
        }

        [HttpPost]
        public void Create([FromBody] Game value)
        {
            this.logic.Create(value);
            this.hub.Clients.All.SendAsync("DriverCreated", value);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var toDelete = this.logic.Read(id);
            this.logic.Delete(id);
            this.hub.Clients.All.SendAsync("DriverDeleted", toDelete);
        }




        [HttpGet]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await WebSocketHandler.HandleWebSocketAsync(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }





        [HttpPost("start-game-session")]
        public async Task<IActionResult> StartGameSession([FromBody] GameSessionRequest request)
        {
            var players = request.Players;

            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await this.logic.GameSession(players, webSocket);
                return new EmptyResult();
            }
            else
            {
                return BadRequest("WebSocket connection expected.");
            }
        }
    }
    public class GameSessionRequest
    {
        public List<Player> Players { get; set; }
    }
}
