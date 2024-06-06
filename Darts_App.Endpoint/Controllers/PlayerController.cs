using Darts_App.Logic;
using Darts_App.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Darts_App.Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        IPlayerLogic logic;

        public PlayerController(IPlayerLogic logic)
        {
            this.logic = logic;
        }

        // GET: api/<PlayerController>
        [HttpGet]
        public IQueryable<Player> ReadAll()
        {
            return this.logic.ReadAll();
        }

        // GET api/<PlayerController>/5
        [HttpGet("{id}")]
        public Player Read(int id)
        {
            return this.logic.Read(id);
        }

        // POST api/<PlayerController>
        [HttpPost]
        public void Create([FromBody] Player value)
        {
            this.logic.Create(value);
        }

        // PUT api/<PlayerController>/5
        [HttpPut]
        public void Put([FromBody] Player value)
        {
            this.logic.Update(value);
        }

        // DELETE api/<PlayerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this.logic.Delete(id);
        }

        [HttpGet("{userName}/{password}")]
        public Player LogIn(string userName, string password)
        {
            try
            {
                Player p = this.logic.LogIn(userName, password);
                return p;
            }
            catch (NullReferenceException e)
            {

                return null;
            }
            
           
        }
    }
}
