using Darts_App.Models;
using Darts_App.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darts_App.Logic
{
    public class PlayerGameConnectionLogic : IPlayerGameConnectionLogic
    {
        IRepository<PlayerGameConnection> repo;
        public PlayerGameConnectionLogic(IRepository<PlayerGameConnection> repo)
        {
            this.repo = repo;
        }

        public void Create(PlayerGameConnection item)
        {
            this.repo.Create(item);
        }

        public void Delete(int id)
        {
            this.repo.Delete(id);
        }

        public PlayerGameConnection Read(int id)
        {
            return this.repo.Read(id);
        }

        public IQueryable<PlayerGameConnection> ReadAll()
        {
            return this.repo.ReadAll();
        }

        public void Update(PlayerGameConnection item)
        {
            this.repo.Update(item);
        }
    }
}
