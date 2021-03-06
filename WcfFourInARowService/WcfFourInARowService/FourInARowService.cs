﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Linq;
using System.Reflection.Emit;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace WcfFourInARowService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
          ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class FourInARowService : IFourInARowService
    {
        private int gameID = 0; 
        public Dictionary<string, IFourInARowCallback> connetedClients = new Dictionary<string, IFourInARowCallback>();
        private readonly Dictionary<int, GameManager> games = new Dictionary<int, GameManager>();
        private const int TOP3 = 3;
        /// <summary>
        /// remove client from connected clients
        /// </summary>
        /// <param name="player"></param>
        /// <param name="gameID"></param>
        public void Disconnect(string player, int gameID)
        {
            connetedClients.Remove(player);
            if (games.ContainsKey(gameID))
                games.Remove(gameID);
            NoticeAll(player, false);
        }
        /// <summary>
        /// notify all new user entered
        /// </summary>
        /// <param name="player"></param>
        /// <param name="connected"></param>
        public void NoticeAll(string player, bool connected)
        //update all connected client, other client just connect/disconnect
        {

            var connectAtLobby = new Dictionary<string, IFourInARowCallback>(connetedClients);
            connectAtLobby.Remove(player);
            foreach (var game in games.Values)
            {
                if (connectAtLobby.ContainsKey(game.p1)) connectAtLobby.Remove(game.p1);
                if (connectAtLobby.ContainsKey(game.p2)) connectAtLobby.Remove(game.p2);
            }
            foreach (var callBack in connectAtLobby.Values)
            {
                Thread updateOtherPlayerThread = new Thread(() =>
                {
                    callBack.OtherPlayerDisOConnnectd(player, connected);
                }
              );
                updateOtherPlayerThread.Start();
            }
        }
        /// <summary>
        /// notify all 2 users start game between them and wont be available
        /// </summary>
        /// <param name="challanger"></param>
        /// <param name="rival"></param>
        /// <param name="connected"></param>
        public void NoticeAllGameStarted(string challanger, string rival, bool connected)
        //update all connected client, other client just connect/disconnect
        {
            var connectAtLobby = new Dictionary<string, IFourInARowCallback>(connetedClients);
            connectAtLobby.Remove(challanger);
            connectAtLobby.Remove(rival);
            foreach (var game in games.Values)
            {
                if (connectAtLobby.ContainsKey(game.p1)) connectAtLobby.Remove(game.p1);
                if (connectAtLobby.ContainsKey(game.p2)) connectAtLobby.Remove(game.p2);
            }
            foreach (var callBack in connectAtLobby.Values)
            {
                Thread updateOtherPlayerThread = new Thread(() =>
                {
                    callBack.OtherPlayerDisOConnnectd(challanger, connected);
                    callBack.OtherPlayerDisOConnnectd(rival, connected);
                }
              );
                updateOtherPlayerThread.Start();
            }
        }
        /// <summary>
        /// client connect in sign-in
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="hashedPassword"></param>
        public void ClientConnect(string userName, string hashedPassword)
        {
            if (connetedClients.ContainsKey(userName))
            {
                UserConnectedFault userExsists = new UserConnectedFault
                {
                    Details = $"{userName} alrady connected..."
                };
                throw new FaultException<UserConnectedFault>(userExsists);
            }
            using (var ctx = new fourinrowDBEntities())
            {
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
                    IncorrectPasswordFault userIncorrectPass = new IncorrectPasswordFault
                    {
                        Details = $"Wrong password entered, please try again."
                    };
                    throw new FaultException<IncorrectPasswordFault>(userIncorrectPass);
                }
                else
                {
                    IFourInARowCallback callback = OperationContext.Current.GetCallbackChannel<IFourInARowCallback>();
                    connetedClients.Add(userName, callback);
                }
            }
        }
        /// <summary>
        /// client connect in sign-up
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="hashedPassword"></param>
        public void Register(string userName, string hashedPassword)
        {
            using (var ctx = new fourinrowDBEntities())
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

                IFourInARowCallback regCallback = OperationContext.Current.GetCallbackChannel<IFourInARowCallback>();
                connetedClients.Add(userName, regCallback);
            }
        }
        /// <summary>
        /// notify rival about user move
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="col"></param>
        /// <param name="player"></param>
        /// <param name="middleOfGame"></param>
        /// <returns></returns>
        public MoveResult ReportMove(int gameId, int col, int player, bool middleOfGame)
        {
            if (!games.ContainsKey(gameId)) return MoveResult.GameOn;
            string rival = player == 1 ? games[gameId].p1 : games[gameId].p2;
            char sign = player == 1 ? 'b' : 'r';

            MoveResult result = games[gameId].VerifyMove(col, sign, middleOfGame);
            if (middleOfGame)
            {
                rival = rival == games[gameId].p1 ? games[gameId].p2 : games[gameId].p1;
                result = MoveResult.YouLost;
            }
            if (result == MoveResult.NotYourTurn) return result;
            if (result == MoveResult.Draw || result == MoveResult.YouWon || result == MoveResult.YouLost)
            {
                updateDBAfterGame(result, sign, gameID);
                games.Remove(gameId);
            }
            sendThred(rival, result, col);
            return result;
        }
        /// <summary>
        /// helper for using thread
        /// </summary>
        /// <param name="rival"></param>
        /// <param name="result"></param>
        /// <param name="col"></param>
        void sendThred(string rival, MoveResult result, int col)
        {
            if (!connetedClients.ContainsKey(rival))
            {
                throw new FaultException<OpponentDisconnectedFault>(new OpponentDisconnectedFault());
            }
            Thread updateOtherThread = new Thread(() =>
            {
                connetedClients[rival].OtherPlayerMoved(result, col);
            });
            updateOtherThread.Start();
        }
        /// <summary>
        /// update database when game ended
        /// </summary>
        /// <param name="res"></param>
        /// <param name="sign"></param>
        /// <param name="gameID"></param>
        private void updateDBAfterGame(MoveResult res, char sign, int gameID)
        {
            int p1Score = 0;
            int p2Score = 0; 
            var p1 = games[gameID].getPlayer(1);
            var p2 = games[gameID].getPlayer(2);
            using (var ctx = new fourinrowDBEntities())
            {
                var updateGame = (from g in ctx.Games
                                  where g.GameId == gameID
                                  select g).FirstOrDefault();
                var updateP1 = (from u in ctx.Users
                                where u.UserName == p1
                                select u).FirstOrDefault();
                var updateP2 = (from u in ctx.Users
                                where u.UserName == p2
                                select u).FirstOrDefault();
                switch (res)
                {
                    case MoveResult.Draw:
                        p1Score = games[gameID].CalculateScore('r', 'd');
                        p2Score = games[gameID].CalculateScore('b', 'd');
                        updateGame.Winner = "Draw";
                        updateGame.WinnerPoint = Math.Max(p1Score, p2Score);
                        updateP1.CareerGames += 1;
                        updateP1.Points += p1Score;
                        updateP2.CareerGames += 1;
                        updateP2.Points += p2Score;
                        break;
                    case MoveResult.YouWon:
                    case MoveResult.YouLost:
                        switch (sign)
                        {
                            case 'r':
                                p1Score = games[gameID].CalculateScore('r', 'r');
                                p2Score = games[gameID].CalculateScore('b', 'r');
                                updateGame.Winner = p1;
                                updateGame.WinnerPoint = p1Score;
                                updateP1.Wins += 1;
                                updateP2.Loosess += 1;
                                break;
                            case 'b':
                                p1Score = games[gameID].CalculateScore('r', 'b');
                                p2Score = games[gameID].CalculateScore('b', 'b');
                                updateGame.Winner = p2;
                                updateGame.WinnerPoint = p2Score;
                                updateP2.Wins += 1;
                                updateP1.Loosess += 1;
                                break;
                        }
                        updateP1.CareerGames += 1;
                        updateP1.Points += p1Score;
                        updateP2.CareerGames += 1;
                        updateP2.Points += p2Score;
                        break;
                }
                ctx.SaveChanges();
            }
        }
        /// <summary>
        /// add new game to games list
        /// </summary>
        /// <param name="challanger"></param>
        /// <param name="rival"></param>
        public void StartNewGame(string challanger, string rival)
        {
            using (var ctx = new fourinrowDBEntities())
            {
                DateTime localTime = DateTime.Now;

                Game newGame = new Game
                {
                    GameId = gameID,
                    Date = localTime,
                    Player1 = challanger,
                    Player2 = rival,
                    WinnerPoint = 0,
                    Winner = "Live" //means game still running, will change in the end of game
                };
                ctx.Games.Add(newGame);
                ctx.SaveChanges();

                GameManager gm = new GameManager(challanger, rival, connetedClients[challanger], connetedClients[rival]);
                games.Add(gameID, gm);
                Thread updateRivalThread = new Thread(() =>
                {
                    connetedClients[rival].StartGameAgainstRival(challanger, rival, gameID);
                });
                updateRivalThread.Start();
                NoticeAllGameStarted(challanger, rival, false);
            }
        }
        /// <summary>
        /// retrive available game id
        /// </summary>
        private void getMaxGameID()
        {
            using (var ctx = new fourinrowDBEntities())
            {
                var maxId = (from g in ctx.Games
                             orderby g.GameId descending
                             select g.GameId).FirstOrDefault();
                gameID = maxId + 1;
            }
        }
        /// <summary>
        /// ask for available rivals
        /// </summary>
        /// <param name="myUser"></param>
        /// <returns></returns>
        public List<string> GetConnectedClients(string myUser)
        {
            var ret = new List<string>(connetedClients.Keys.ToList());
            ret.Remove(myUser);
            foreach (var game in games.Values)
            {
                if (ret.Contains(game.p1)) ret.Remove(game.p1);
                if (ret.Contains(game.p2)) ret.Remove(game.p2);
            }
            return ret;
        }
        /// <summary>
        /// send rival ganme challege
        /// </summary>
        /// <param name="rival"></param>
        /// <param name="Challenger"></param>
        /// <returns></returns>
        public int ChallengeRival(string rival, string Challenger)
        {
            if (!connetedClients.ContainsKey(rival))
            {
                throw new FaultException<OpponentDisconnectedFault>(new OpponentDisconnectedFault());
            }
            bool res = connetedClients[rival].SendGameInvitation(rival, Challenger);
            if (res == true)
            {
                getMaxGameID();
                return gameID;
            }
            return -1;
        }
        /// <summary>
        /// helper for retrive sort list by case for stats center
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public List<string> createSortedList(string by)
        {
            using (var ctx = new fourinrowDBEntities())
            {
                switch (by)
                {
                    case "Name":
                        var byName = (from u in ctx.Users
                                      orderby u.UserName descending
                                      select u.UserName).ToList();
                        return byName;
                    case "Games":
                        var byGames = (from u in ctx.Users
                                       orderby u.CareerGames descending
                                       select u).ToList();
                        return convertResultToString(byGames, by);
                    case "Wins":
                        var byWins = (from u in ctx.Users
                                      orderby u.Wins descending
                                      select u).ToList();
                        return convertResultToString(byWins, by);
                    case "Looses":
                        var byLooses = (from u in ctx.Users
                                        orderby u.Loosess descending
                                        select u).ToList();
                        return convertResultToString(byLooses, by);
                    case "Points":
                        var byPoints = (from u in ctx.Users
                                        orderby u.Points descending
                                        select u).ToList();
                        return convertResultToString(byPoints, by);
                    default:
                        return null;
                }
            }
        }
        /// <summary>
        /// helper for retrive data from database
        /// </summary>
        /// <param name="list"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        private List<string> convertResultToString(List<User> list, string by)
        {
            List<string> ret = new List<string>();
            switch (by)
            {
                case "Games":
                    foreach (var user in list)
                    {
                        ret.Add($"{user.UserName} {by}: {user.CareerGames}");
                    }
                    return ret;
                case "Wins":
                    foreach (var user in list)
                    {
                        ret.Add($"{user.UserName} {by}: {user.Wins}");
                    }
                    return ret;
                case "Looses":
                    foreach (var user in list)
                    {
                        ret.Add($"{user.UserName} {by}: {user.Loosess}");
                    }
                    return ret;
                case "Points":
                    foreach (var user in list)
                    {
                        ret.Add($"{user.UserName} {by}: {user.Points}");
                    }
                    return ret;
                default:
                    return null;
            }
        }
        /// <summary>
        /// retrive data about games between 2 users
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public List<string> createRivaryData(string p1, string p2)
        {
            using (var ctx = new fourinrowDBEntities())
            {
                List<string> rivaryData = new List<string>();
                var gamesBetween = (from g in ctx.Games
                                    where (((g.Player1 == p1 && g.Player2 == p2)
                                            || (g.Player1 == p2 && g.Player2 == p1))
                                            && g.Winner != "Live")
                                    select g).ToList();
                if (gamesBetween.Count == 0) return rivaryData;
                rivaryData.Add(winPercentage(gamesBetween, p1).ToString());
                rivaryData.Add(winPercentage(gamesBetween, p2).ToString());
                foreach (var game in gamesBetween)
                {
                    rivaryData.Add(
                        $"Game ID: {game.GameId.ToString()}\n" +
                        $"Date: {game.Date.ToString()}\n" +
                        $"Participants: {game.Player1}, {game.Player2}\n" +
                        $"Winner: {game.Winner}\n" +
                        $"Winner Points: {game.WinnerPoint.ToString()}\n" +
                        $"---------------------------"
                    );
                }
                return rivaryData;
            }
        }
        /// <summary>
        /// calculate win percantage of user
        /// </summary>
        /// <param name="list"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        private double winPercentage(List<Game> list, string player)
        {
            int wins = 0;
            foreach (var game in list)
            {
                if (game.Winner == player) wins++;
            }
            return ((double)wins / list.Count()) * 100;
        }
        /// <summary>
        /// retrive all games ever played and their info
        /// </summary>
        /// <returns></returns>
        public List<string> getGamesHistory()
        {
            List<string> allGamesData = new List<string>();
            gameID = 7; //TODO: remove after game works
            if (gameID == 0) //no games yet
            {
                return allGamesData;
            }
            using (var ctx = new fourinrowDBEntities())
            {
                var allGames = (from g in ctx.Games
                                where g.WinnerPoint != 0
                                select g).ToList();

                foreach (var game in allGames)
                {
                    allGamesData.Add(
                        $"Game ID: {game.GameId.ToString()}\n" +
                        $"Date: {game.Date.ToString()}\n" +
                        $"Participants: {game.Player1}, {game.Player2}\n" +
                        $"Winner: {game.Winner}\n" +
                        $"Winner Points: {game.WinnerPoint.ToString()}\n" +
                        $"---------------------------"
                        );
                }
                return allGamesData;
            }

        }
        /// <summary>
        /// retrive all sign-up user names
        /// </summary>
        /// <returns></returns>
        public List<string> getAllUserNames()
        {
            using (var ctx = new fourinrowDBEntities())
            {
                var allUsers = (from u in ctx.Users
                                select u.UserName).ToList();
                return allUsers;
            }
        }
        /// <summary>
        /// retrive all live games
        /// </summary>
        /// <returns></returns>
        public List<string> getLiveGames()
        {
            using (var ctx = new fourinrowDBEntities())
            {
                List<string> liveGamesData = new List<string>();
                var LiveGames = (from g in ctx.Games
                                 where g.Winner == "Live"
                                 select g).ToList();
                if (LiveGames.Count == 0) return liveGamesData;
                foreach (var game in LiveGames)
                {
                    liveGamesData.Add(
                        $"Game ID: {game.GameId.ToString()}\n" +
                        $"Start Time: {game.Date.ToString()}\n" +
                        $"Participants: {game.Player1}, {game.Player2}\n" +
                        $"---------------------------"
                        );
                }
                return liveGamesData;
            }
        }
        /// <summary>
        /// retrive specific user stats
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Dictionary<string, string> getUserStats(string user)
        {
            using (var ctx = new fourinrowDBEntities())
            {
                Dictionary<string, string> userStatsData = new Dictionary<string, string>();
                var userStats = (from u in ctx.Users
                                 where u.UserName == user
                                 select u).FirstOrDefault();
                if (userStats == null) return userStatsData;
                userStatsData.Add("User", userStats.UserName);
                userStatsData.Add("Games", userStats.CareerGames.ToString());
                userStatsData.Add("Points", userStats.Points.ToString());
                userStatsData.Add("Wins", userStats.Wins.ToString());
                userStatsData.Add("Losses", userStats.Loosess.ToString());
                return userStatsData;
            }
        }
        /// <summary>
        /// retrive top 3 player
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Tuple<string, int>> getTopThreeUsers()
        {
            using (var ctx = new fourinrowDBEntities())
            {
                Dictionary<int, Tuple<string, int>> top3 = new Dictionary<int, Tuple<string, int>>();
                var allSorted = (from u in ctx.Users
                                 orderby u.Points descending
                                 select u).ToList();
                if (allSorted == null) return top3;
                for (int i = 0; i < TOP3 && i < allSorted.Count; i++)
                {
                    var now = Tuple.Create(allSorted[i].UserName, allSorted[i].Points);
                    top3.Add(i, now);
                }
                return top3;
            }
        }
    }
}