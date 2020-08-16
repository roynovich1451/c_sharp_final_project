﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;

namespace WcfFourInARowService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
          ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class FourInARowService : IFourInARowService
    {
        private int gameID = 0; //count games
        public Dictionary<string, IFourInARowCallback> connetedClients = new Dictionary<string, IFourInARowCallback>();
        private readonly Dictionary<int, GameManager> games = new Dictionary<int, GameManager>();

        public void Disconnect(string player, int gameID)
        {
            connetedClients.Remove(player);
            if (games.ContainsKey(gameID))
                games.Remove(gameID);
            foreach (var callBack in connetedClients.Values)
            {
                Thread updateOtherPlayerThread = new Thread(() =>
                {
                    callBack.OtherPlayerDisconnected(player);
                }
              );
                updateOtherPlayerThread.Start();
            }
        }

        public void ClientConnect(string userName, string hashedPassword)
        {
            if (connetedClients.ContainsKey(userName))
            {
                UserConnectedFault userExists = new UserConnectedFault
                {
                    Details = $"{userName} is already connected"
                };
                throw new FaultException<UserConnectedFault>(userExists);
            }
            using (var ctx = new fourinrowDBContext())
            {
                /*
                var user = (from u in ctx.Users
                            where u.UserName == userName
                            select u).FirstOrDefault();
                if (user == null)
                {
                    UserNotRegisteredFault userNotExsists = new UserNotRegisteredFault
                    {
                        Details = $"{userName} not in DB, please sign up."
                    };
                    throw new FaultException<UserNotRegisteredFault>(userNotExsists);
                }
                else if (hashedPassword != user.HassedPassword)
                {
                    IncorectPasswordFault userIncorrectPass = new IncorectPasswordFault
                    {
                        Details = $"Wrong password entered, please try again."
                    };
                    throw new FaultException<IncorectPasswordFault>(userIncorrectPass);
                }
                else
                {*/
                IFourInARowCallback callback = OperationContext.Current.GetCallbackChannel<IFourInARowCallback>();
                connetedClients.Add(userName, callback);
                //NeedToUpdateRivalList(userName); TODO
                //}
            }
        }

        public void Register(string userName, string hashedPassword)
        {/*
            using (var ctx = new fourinrowDBContext())
            {
                var IsExists = (from u in ctx.Users
                                where u.UserName == userName
                                select u).FirstOrDefault();
                if (IsExists != null)
                {
                    UserNameTakenFault userNameTaken = new UserNameTakenFault
                    {
                        Details = $"{userName} is taken, please pick another."
                    };
                    throw new FaultException<UserNameTakenFault>(userNameTaken);
                }
                User newUser = new User
                {
                    UserName = userName,
                    HassedPassword = hashedPassword,
                    Wins = 0,
                    Loosess = 0,
                    Points = 0,
                    CareerGames = 0
                };
                ctx.Users.Add(newUser);
                ctx.SaveChanges();
                */
            if (!UserExists(userName))
            {
                UserNotRegisteredFault userExists = new UserNotRegisteredFault
                {
                    Details = $"No user registered with: {userName}"
                };
                throw new FaultException<UserNotRegisteredFault>(userExists);
            }
            if (string.IsNullOrEmpty(userName))
            {
                EmptyFieldFault noInput = new EmptyFieldFault{
                    Details= $"Must enter a username"
                };
                throw new FaultException<EmptyFieldFault>(noInput);
            }
            if (string.IsNullOrEmpty(hashedPassword))
            {
                EmptyFieldFault noInput = new EmptyFieldFault
                {
                    Details = $"Must enter a password"
                };
                throw new FaultException<EmptyFieldFault>(noInput);
            }
            //TODO: add more errors
            IFourInARowCallback regCallback = OperationContext.Current.GetCallbackChannel<IFourInARowCallback>();
            connetedClients.Add(userName, regCallback);
            NeedToUpdateRivalList(userName);
        }

        private bool UserExists(string name)
        {
            //TODO: need to implemenet search in user list in db
            return true;
        }

        public MoveResult ReportMove(int gameId, int col, int player)
        {
            char sign = player == 1 ? 'b' : 'r';
            MoveResult result = games[gameId].VerifyMove(col, sign);
            if (result == MoveResult.Draw || result == MoveResult.YouWon)
            {
                games.Remove(gameId);
                int p1Score = 0;
                int p2Score = 0;
                /*using (var ctx = new fourinrowDBContext())
                {
                    var updateGame = (from g in ctx.Games
                                      where g.GameId == gameId
                                      select g).First();
                    var updateP1 = (from u in ctx.Users
                                     where u.UserName == games[gameId].p1
                                     select u).First();
                    var updateP2 = (from u in ctx.Users
                                    where u.UserName == games[gameId].p2
                                    select u).First();
                */
                Game updateGame = new Game();
                User updateP1 = new User();
                User updateP2 = new User();
                if (result == MoveResult.Draw) //game end with draw
                {
                    updateGame.Winner = "Draw";
                    p1Score = games[gameId].CalculateScore('b', 'd');
                    p2Score = games[gameId].CalculateScore('r', 'd');
                    updateGame.WinnerPoint = Math.Max(p1Score, p2Score);
                    updateP1.CareerGames += 1;
                    updateP1.Points += p1Score;
                    updateP2.CareerGames += 1;
                    updateP2.Points += p2Score;
                }
                if (result == MoveResult.YouWon) //other player won
                {
                    char charWinner = sign == 'b' ? 'r' : 'b'; //swap char
                    p1Score = games[gameId].CalculateScore('b', charWinner);
                    p2Score = games[gameId].CalculateScore('r', charWinner);
                    if (charWinner == 'b') //P1 win
                    {
                        updateGame.WinnerPoint = p1Score;
                        updateP1.CareerGames += 1;
                        updateP1.Points += p1Score;
                        updateP1.Wins += 1;
                        updateP2.CareerGames += 1;
                        updateP2.Points += p2Score;
                        updateP2.Loosess += 1;
                    }
                    if (charWinner == 'r') //P2 win
                    {
                        updateGame.WinnerPoint = p2Score;
                        updateP2.CareerGames += 1;
                        updateP2.Points += p2Score;
                        updateP2.Wins += 1;
                        updateP1.CareerGames += 1;
                        updateP1.Points += p1Score;
                        updateP1.Loosess += 1;
                    }
                }
                //ctx.SaveChanges();
                //}
            }
            string other_player = player == 1 ? games[gameId].p1 : games[gameId].p2;
            if (!connetedClients.ContainsKey(other_player))
            {
                throw new FaultException<OpponentDisconnectedFault>(new OpponentDisconnectedFault());
            }
            if (result != MoveResult.NotYourTurn && result != MoveResult.InvalidMove)
            {
                Thread updateOtherThread = new Thread(() =>
                {
                    connetedClients[other_player].OtherPlayerMoved(result, col);
                });
                updateOtherThread.Start();
            }
            return result;
        }

        public void StartNewGame(string player1, string player2)
        {
            using (var ctx = new fourinrowDBContext())
            {
                ++gameID;
                DateTime localTime = DateTime.Now;
                /*
                Game newGame = new Game
                {
                    GameId = ++gameID,
                    Date = localTime,
                    Player1 = player1,
                    Player2 = player2,
                    WinnerPoint = 0,
                    Winner = "Live" //means game still running, will change in the end of game
                };
                ctx.Games.Add(newGame);
                ctx.SaveChanges();
                */
                connetedClients[player2].StartGameAgainstRival(player1);
                GameManager gm = new GameManager(player1, player2, connetedClients[player1], connetedClients[player2]);
                games.Add(gameID, gm);
                connetedClients[player1].NotifyNewGameId(gameID);
                connetedClients[player2].NotifyNewGameId(gameID);
            }
        }

        public Dictionary<string, IFourInARowCallback> GetConnectedClients(string myUser)
        {
            var ret = new Dictionary<string, IFourInARowCallback>(connetedClients);
            ret.Remove(myUser);
            return ret;
        }

        public bool ChallengeRival(string rival, string Challenger)
        {
            if (!connetedClients.ContainsKey(rival))
            {               
                throw new FaultException<OpponentDisconnectedFault>(new OpponentDisconnectedFault());
            }
            bool res = connetedClients[rival].SendGameInvitation(rival, Challenger);
            return res;
        }

        private void NeedToUpdateRivalList(string player)
        {
            foreach (var client in connetedClients.Values)
            {
                client.NewPlayerConnected(player);
            }
        }
    }
}