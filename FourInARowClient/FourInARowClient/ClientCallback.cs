using FourInARowClient.FourInARowServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FourInARowClient
{
    public class ClientCallback : IFourInARowServiceCallback
    {
        #region Delegates
        internal Func<string, bool> popUpGameInvitation;
        internal Action<MoveResult, int> updateGame;
        internal Action<string> startGame;
        internal Action<string,bool> updateRivalList;
        internal Action<string> endGame;
        internal Action<int> updateLiveGameId;
        internal string myUser;
        #endregion

        public void OtherPlayerConnected(string myUser)
        {
            updateRivalList(myUser);
        }

        public void OtherPlayerMoved(MoveResult moveResult, int location)
        {
            updateGame(location);
            if (moveResult == MoveResult.Draw)
            {
                endGame("It's a draw");
            }
            else if (moveResult == MoveResult.YouWon)
            {
                endGame("You lost...");
            }
        }

        public  bool SendGameInvitation(string rival, string chalanger)
        {
            MessageBoxResult res = MessageBox.Show($"{rival} has chalanged you\nDo you want to start game?", "Game invitation", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            return res == MessageBoxResult.Yes ? true : false;

        }

        public void StartGameAgainstRival(string chalanger)
        {
            startGame(chalanger);
        }

        public void OtherPlayerDisconnected(string user)
        {
            updateRivalList(user);
        }

        public void RivalStartGame(string p1, string p2)
        {
            throw new NotImplementedException();
        }

        public void NewPlayerConnected(string user)
        {
            updateRivalList(user);
        }

        public void NotifyNewGameId(int gameId)
        {
            updateLiveGameId(gameId);
        }
    }
}
