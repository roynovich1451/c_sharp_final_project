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
        int numClients = 0;
        Dictionary<int, IFourInARowCallback> clients = new Dictionary<int, IFourInARowCallback>();
        Dictionary<int, Game> games = new Dictionary<int, Game>();
        public void Disconnect(int player)
        {
            clients.Remove(player);
        }

        public void DisconnectBeforeGame(int player)
        {
            --numClients;
            waitingCallback = null;
        }

        IFourInARowCallback waitingCallback;
        public int Register()
        {
            numClients++;
            IFourInARowCallback callback = OperationContext.Current.GetCallbackChannel<IFourInARowCallback>();
            if (numClients % 2 == 1)
            {
                waitingCallback = callback;
                return numClients;
            }
            else
            {
                clients.Add(numClients - 1, waitingCallback);
                waitingCallback.OtherPlayerConnected();
                clients.Add(numClients, callback);
                games.Add((numClients - 1) / 2, new Game());
                return numClients;
            }
        }

        public MoveResult ReportMove(int location, int player)
        {
            char sign = player % 2 == 1 ? 'X' : 'O';
            MoveResult result = games[(player - 1) / 2].VerifyMove(location, sign);
            if (result == MoveResult.Draw || result == MoveResult.YouWon)
            {
                games.Remove((player - 1) / 2);
            }
            int other_player = player % 2 == 1 ? player + 1 : player - 1;

            if (!clients.ContainsKey(other_player))
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
            }
            return result;
        }
    }
}