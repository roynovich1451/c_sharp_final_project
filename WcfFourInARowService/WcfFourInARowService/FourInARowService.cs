using System;
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
        private int gameID = 0; //count games
        public Dictionary<string, IFourInARowCallback> connetedClients = new Dictionary<string, IFourInARowCallback>();
        private readonly Dictionary<int, GameManager> games = new Dictionary<int, GameManager>();
        private const int TOP3 = 3;
        public void Disconnect(string player, int gameID)
        {
            connetedClients.Remove(player);
            if (games.ContainsKey(gameID))
                games.Remove(gameID);
            NoticeAll(player, false);
        }

        public void NoticeAll(string player, bool connected)
            //update all connected client, other client just connect/disconnect
        {
            var connectButMe = new Dictionary<string, IFourInARowCallback>(connetedClients);
            connectButMe.Remove(player);
            foreach (var callBack in connectButMe.Values)
            {
                Thread updateOtherPlayerThread = new Thread(() =>
                {
                    callBack.OtherPlayerDisOConnnectd(player, connected);
                }
              );
                updateOtherPlayerThread.Start();
            }
        }
        public void NoticeAllGameStarted(string challanger, string rival, bool connected)
        //update all connected client, other client just connect/disconnect
        {
            var connectButMe = new Dictionary<string, IFourInARowCallback>(connetedClients);
            connectButMe.Remove(challanger);
            connectButMe.Remove(rival);
            foreach (var callBack in connectButMe.Values)
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

        public MoveResult ReportMove(int gameId, int col, int player)
        {
            char sign = player == 1 ? 'b' : 'r';
            MoveResult result = games[gameId].VerifyMove(col, sign);
            if (result == MoveResult.Draw || result == MoveResult.YouWon)
            {
                games.Remove(gameId);
                int p1Score = 0;
                int p2Score = 0;
                /*using (var ctx = new fourinrowDBEntities())
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

        public void StartNewGame(string challanger, string rival)
        {
            using (var ctx = new fourinrowDBEntities())
            {
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
                GameManager gm = new GameManager(challanger, rival, connetedClients[challanger], connetedClients[rival]);
                games.Add(gameID, gm);
                Thread updateRivalThread = new Thread(() =>
                {
                    connetedClients[rival].StartGameAgainstRival(challanger, gameID);
                });
                updateRivalThread.Start();
                NoticeAllGameStarted(challanger, rival, false);
            }
        }

        public Dictionary<string, IFourInARowCallback> GetConnectedClients(string myUser)
        {
            var ret = new Dictionary<string, IFourInARowCallback>(connetedClients);
            ret.Remove(myUser);
            return ret;
        }

        public int ChallengeRival(string rival, string Challenger)
        {
            if (!connetedClients.ContainsKey(rival))
            {
                throw new FaultException<OpponentDisconnectedFault>(new OpponentDisconnectedFault());
            }
            bool res = connetedClients[rival].SendGameInvitation(rival, Challenger);
            if (res == true)
            {
                gameID++;
                return gameID;
            }
            return -1;
        }

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
        public List<string> createRivaryData(string p1, string p2)
        {
            using (var ctx = new fourinrowDBEntities())
            {
                List<string> rivaryData = new List<string>();
                var gamesBetween = (from g in ctx.Games
                                    where (g.Player1 == p1 && g.Player2 == p2)
                                            || (g.Player1 == p2 && g.Player2 == p1)
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

        private double winPercentage(List<Game> list, string player)
        {
            int wins = 0;
            foreach (var game in list)
            {
                if (game.Winner == player) wins++;
            }
            return ((double)wins / list.Count())*100;
        }

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

        public List<string> getAllUserNames()
        {
            using (var ctx = new fourinrowDBEntities())
            {
                var allUsers = (from u in ctx.Users
                                select u.UserName).ToList();
                return allUsers;
            }
        }

        public List<string> getLiveGames(){
            using (var ctx = new fourinrowDBEntities())
            {
                List<string> liveGamesData = new List<string>();
                var LiveGames = (from g in ctx.Games
                                 where g.WinnerPoint == 0
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

        public Dictionary<int, Tuple<string, int>> getTopThreeUsers()
        {
            using (var ctx = new fourinrowDBEntities())
            {
                Dictionary<int, Tuple<string, int>> top3 = new Dictionary<int, Tuple<string, int>>();
                var allSorted = (from u in ctx.Users
                                 orderby u.Points descending
                                 select u).ToList();
                if (allSorted == null) return top3;
                for (int i=0; i < TOP3 && i < allSorted.Count; i++)
                {
                    var now = Tuple.Create(allSorted[i].UserName, allSorted[i].Points);
                    top3.Add(i, now);
                }
                return top3;
            }
        }
    }
}