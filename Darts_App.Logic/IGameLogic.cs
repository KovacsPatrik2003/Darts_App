using Darts_App.Models;
using System.Collections.Generic;
using System.Linq;

namespace Darts_App.Logic
{
    public interface IGameLogic
    {
        void Create(Game item);
        void Delete(int id);
        Game Read(int id);
        IQueryable<Game> ReadAll();
        public void GameSession(List<Player> players, Game game);
        public event GameLogicDelegate GetSets;
        public event GameLogicDelegate GetLegs;
        public event GameLogicDelegate GetStartPoint;
        public event GameLogicDelegateStirng GetChek_out;
        public event OnGoingDelegate OngoingGamePoints;
    }
}