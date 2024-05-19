﻿using Darts_App.Models;
using Darts_App.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Darts_App.Logic
{
    public delegate int GameLogicDelegate();
    public delegate int OnGoingDelegate(Player p, List<Player> L, Game g);
    public delegate string GameLogicDelegateStirng();
    public class GameLogic : IGameLogic
    {
        IRepository<Game> repo;
        IRepository<PlayerGameConnection> connectionRepo;
        public event GameLogicDelegate GetSets;
        public event GameLogicDelegate GetLegs;
        public event GameLogicDelegate GetStartPoint;
        public event GameLogicDelegateStirng GetChek_out;
        public event OnGoingDelegate OngoingGamePoints;
        public GameLogic(IRepository<Game> repo, IRepository<PlayerGameConnection> connectionRepo)
        {
            this.repo = repo;
            this.connectionRepo = connectionRepo;
        }

        public void Create(Game item)
        {
            this.repo.Create(item);
        }

        public void Delete(int id)
        {
            this.repo.Delete(id);
        }

        public Game Read(int id)
        {
            return this.repo.Read(id);
        }

        public IQueryable<Game> ReadAll()
        {
            return this.repo.ReadAll();
        }

        public void GameSession(List<Player> players, Game game)
        {
            //create connections between the tables
            for (int k = 0; k < players.Count; k++)
            {
                connectionRepo.Create(new PlayerGameConnection()
                {
                    GameId = game.Id,
                    PlayerId = players[k].Id
                });
            }
            //game session

            //get sets from client
            int set =(int) GetSets?.Invoke();
            for (int k = 0; k < set; k++)
            {
                game.Sets.Add(0);
            }
            //get legs from client
            int leg =(int) GetLegs?.Invoke();
            for (int k = 0; k < leg; k++)
            {
                game.Legs.Add(0);
            }
            //get point from client
            game.StartPoints = GetStartPoint?.Invoke();
            int? fixpoints = game.StartPoints;

            //get cheout mode from client
            game.Check_Out = GetChek_out?.Invoke();

            //game session start
            bool finish = false;
            int max = 0;
            //set counter
            while (game.Sets.All(x=>x!=game.Sets.Count))
            {
                for (int i = 0; i < game.Legs.Count; i++)
                {
                    game.Legs[i] = 0;
                }
                //leg counter
                while (game.Legs.All(x=>x<game.Legs.Count))
                {
                    //new leg start
                    for (int k = 0; k < players.Count; k++)
                    {
                        players[k].CurrentPoints = (int)fixpoints;
                    }
                    while (players.All(x => x.CurrentPoints > 0))
                    {
                        for (int k = 0; k < players.Count; k++)
                        {
                            int currentpoints = players[k].CurrentPoints;
                            for (int l = 0; l < 3; l++)
                            {
                                int notZeroResultChek = (int)OngoingGamePoints?.Invoke(players[k], players, game);
                                players[k].CurrentPoints -= notZeroResultChek;
                                if (players[k].CurrentPoints < 0)
                                {
                                    players[k].CurrentPoints = currentpoints;
                                    break;
                                }
                                if (players[k].CurrentPoints == 0)
                                {
                                    finish = true;
                                    game.Legs[k]++;
                                    break;
                                }
                            }
                            if (finish)
                            {
                                break;
                            }
                        }
                    }
                    finish = false;
                }
                max = 0;
                for (int i = 0; i < game.Legs.Count; i++)
                {
                    if (game.Legs[max] < game.Legs[i])
                    {
                        max = i;
                    }
                }
                game.Sets[max]++;
            }

            // game session end
            max = 0;
            for (int i = 0; i < game.Sets.Count; i++)
            {
                if (game.Sets[max] < game.Sets[i])
                {
                    max = i;
                }
            }
            game.WinnerId = players[max].Id;
        }
    } 
}
