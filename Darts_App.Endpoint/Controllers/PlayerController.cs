using Darts_App.Logic;
using Darts_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using TQPGSS_HFT_2023241.Endpoint.Services;

namespace Darts_App.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PlayerController : Controller
    {
        IPlayerLogic logic;
        IHubContext<SignalRHub> hub;
        public PlayerController(IPlayerLogic logic, IHubContext<SignalRHub> hub)
        {
            this.logic = logic;
            this.hub = hub;
        }


        [HttpGet]
        public IEnumerable<Player> ReadAll()
        {
            return this.logic.ReadAll();
        }

        [HttpGet("{id}")]
        public Player Read(int id)
        {
            return this.logic.Read(id);
        }

        [HttpPost]
        public void Create([FromBody] Player value)
        {
            this.logic.Create(value);
            this.hub.Clients.All.SendAsync("DriverCreated", value);
        }

        [HttpPut]
        public void Update([FromBody] Player value)
        {
            this.logic.Update(value);
            this.hub.Clients.All.SendAsync("DriverUpdated", value);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var toDelete = this.logic.Read(id);
            this.logic.Delete(id);
            this.hub.Clients.All.SendAsync("DriverDeleted", toDelete);
        }

        [HttpGet("{NameAndPassword}")]
        public Player SignIn(string name, string password)
        {
            return this.logic.SignIn(name, password);
        }

    }
}
