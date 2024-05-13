using Darts_App.Models;

namespace Darts_App.Logic
{
    public interface IPlayerLogic
    {
        void Create(Player item);
        void Delete(int id);
        Player SignIn(string name, string pass);
        void Update(Player item);
    }
}