using Darts_App.Models;
using System.Linq;

namespace Darts_App.Logic
{
    public interface IGameLogic
    {
        void Create(Game item);
        void Delete(int id);
        Game Read(int id);
        IQueryable<Game> ReadAll();
    }
}