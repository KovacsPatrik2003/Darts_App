using Darts_App.Logic;
using Darts_App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Darts_App.Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerGameConnectionController : ControllerBase
    {
        IPlayerGameConnectionLogic logic;

        public PlayerGameConnectionController(IPlayerGameConnectionLogic logic)
        {
            this.logic = logic;
        }

        // GET: api/<PlayerGameConnectionController>
        [HttpGet]
        public IEnumerable<PlayerGameConnection> ReadAll()
        {
            return this.logic.ReadAll();
        }

        // GET api/<PlayerGameConnectionController>/5
        [HttpGet("{id}")]
        public PlayerGameConnection Read(int id)
        {
            return this.logic.Read(id);
        }

        // POST api/<PlayerGameConnectionController>
        [HttpPost]
        public void Create([FromBody] PlayerGameConnection value)
        {
            this.logic.Create(value);
        }

        // PUT api/<PlayerGameConnectionController>/5
        [HttpPut]
        public void Update([FromBody] PlayerGameConnection value)
        {
            this.logic.Update(value);
        }

        // DELETE api/<PlayerGameConnectionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.logic.Delete(id);
        }
    }
}
