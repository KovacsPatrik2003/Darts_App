using Darts_App.Models;
using System.Linq;

namespace Darts_App.Logic
{
    public interface IPlayerGameConnectionLogic
    {
        void Create(PlayerGameConnection item);
        void Delete(int id);
        PlayerGameConnection Read(int id);
        IQueryable<PlayerGameConnection> ReadAll();
        void Update(PlayerGameConnection item);
    }
}