using Darts_App.Models;
using System.Linq;

namespace Darts_App.Logic
{
    public interface IPlayerLogic
    {
        void Create(Player item);
        void Delete(int id);
        Player Read(int id);
        Player SignIn(string name, string pass);
        void Update(Player item);
        public IQueryable<Player> ReadAll();
    }
}