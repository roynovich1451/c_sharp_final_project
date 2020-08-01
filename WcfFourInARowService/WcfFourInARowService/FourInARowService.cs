using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace WcfFourInARowService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
          ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class FourInARowService : IFourInARowService
    {
        int gameID = 0; //count games
        int numClients = 0;
        Dictionary<string, IFourInARowCallback> connetedClients = new Dictionary<string, IFourInARowCallback>();
        Dictionary<int, GameManager> games = new Dictionary<int, GameManager>();

        public void Disconnect(string player)
        {
            connetedClients.Remove(player);
        }

        public void ClientConnect(string userName, string hashedPassword)
        {
            if (connetedClients.ContainsKey(userName))
            {
                UserConnectdFault userExsists = new UserConnectdFault
                {
                    Details = $"{userName} alrady connected..."
                };
                throw new FaultException<UserConnectdFault>(userExsists);
            }
            using (var ctx = new fourinrowDBContext())
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
                    InccorectPasswordFault userIncorrectPass = new InccorectPasswordFault
                    {
                        Details = $"Wrong password entered, please try again."
                    };
                    throw new FaultException<InccorectPasswordFault>(userIncorrectPass);
                }
                else
                {
                    IFourInARowCallback callback = OperationContext.Current.GetCallbackChannel<IFourInARowCallback>();
                    connetedClients.Add(user.UserName, callback);
                    //TODO: user exists and entered correct password need to open LobbyWindow
                }
            }
        }

        IFourInARowCallback waitingCallback;
        public int Register(string userName, string hashedPassword)
        {
            using (var ctx = new fourinrowDBContext())
            {
                if()
                User newUser = new User
                {
                    UserName = userName,
                    HassedPassword = hashedPassword,
                    Wins = 0,
                    Loosess = 0,
                    Points = 0
                };
                ctx.Users.Add(newUser);
                ctx.SaveChanges();
            }
                IFourInARowCallback callback = OperationContext.Current.GetCallbackChannel<IFourInARowCallback>();
            if (numClients % 2 == 1)
            {
                waitingCallback = callback;
                return numClients;
            }
            else
            {
                /*  clients.Add(numClients - 1, waitingCallback);
                  waitingCallback.OtherPlayerConnected();
                  clients.Add(numClients, callback);
                  games.Add((numClients - 1) / 2, new Game());
                  return numClients;
                */
                return 0;
            }
        }

        public MoveResult ReportMove(int location, int player)
        { 
            char sign = player % 2 == 1 ? 'X' : 'O';
            //MoveResult result = games[(player - 1) / 2].VerifyMove(location, sign);
            MoveResult result = MoveResult.GameOn;
            if (result == MoveResult.Draw || result == MoveResult.YouWon)
            {
                games.Remove((player - 1) / 2);
            }
            int other_player = player % 2 == 1 ? player + 1 : player - 1;
            /*if (!clients.ContainsKey(other_player))
            {
                OpponentDisconnectedFault fault = new OpponentDisconnectedFault();
                fault.Details = "The other player quit";
                throw new FaultException<OpponentDisconnectedFault>(fault);
            }
            if (result != MoveResult.NotYourTurn)
            {
                Thread updateOtherPlayerThread = new Thread(() =>
                {
                    clients[other_player].OtherPlayerMoved(result, location);
                }
                );
                updateOtherPlayerThread.Start();
            }*/
            return result;
        }

        public void DisconnectDuringGame(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
