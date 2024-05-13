using Darts_App.Models;
using Darts_App.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Darts_App.Logic
{
    public class PlayerLogic : IPlayerLogic
    {
        Repository<Player> repo;
        public PlayerLogic(Repository<Player> repo)
        {
            this.repo = repo;
        }

        public void Create(Player item)
        {
            if (ReadAll().Where(x => x.Name == item.Name).FirstOrDefault() != null)
            {
                throw new InvalidOperationException("This username is already exist...");
            }
            if (item.Name.Length < 3)
            {
                throw new ArgumentException("Name is too short...");
            }
            else if (item.Password.Length < 4)
            {
                throw new ArgumentException("Password is too short...");
            }
            this.repo.Create(item);
        }

        public void Delete(int id)
        {
            this.repo.Delete(id);
        }

        private Player Read(int id)
        {
            return this.repo.Read(id);
        }

        private IQueryable<Player> ReadAll()
        {
            return this.repo.ReadAll();
        }

        public void Update(Player item)
        {
            this.repo.Update(item);
        }

        public Player SignIn(string name, string pass)
        {
            Player player;
            player = this.repo.ReadAll().FirstOrDefault(x => x.Name == name && x.Password == pass);
            if (player == null)
            {
                throw new NullReferenceException("Username or password does not exist");
            }
            return this.Read(player.Id);
        }

    }
}
