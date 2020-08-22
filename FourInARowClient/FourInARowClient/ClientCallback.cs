using FourInARowClient.FourInARowServiceReference;
using System;
using System.Windows;

namespace FourInARowClient
{
    public class ClientCallback : IFourInARowServiceCallback
    {
        #region Delegates
        internal Func<string, bool> popUpGameInvitation;
        internal Action<int> updateGame;
        internal Action<string> startGame;
        internal Action<string, bool> updateRivalList;
        internal Action<string> endGame;
        internal Action<int> updateLiveGameId;
        internal string myUser;
        #endregion

        public void NewPlayerConnected(string user)
        {
            updateRivalList(user, true);
        }

        public void OtherPlayerMoved(MoveResult moveResult, int col)
        {
            updateGame(col);
            if (moveResult == MoveResult.Draw)
            {
                endGame("It's a draw");
            }
            else if (moveResult == MoveResult.YouWon)
            {
                endGame("You lost...");
            }
        }

        public bool SendGameInvitation(string rival, string challenger)
        {
            MessageBoxResult res = MessageBox.Show($"{rival} has challenged you\nDo you want to start game?", "Game invitation",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            return res == MessageBoxResult.Yes ? true : false;

        }

        public void StartGameAgainstRival(string challenger)
        {
            startGame(challenger);
        }

        public void RivalStartGame(string p1, string p2)
        {
            throw new NotImplementedException();
        }

        public void NotifyNewGameId(int gameId)
        {
            updateLiveGameId(gameId);
        }

        public void OtherPlayerDisOConnnectd(string user, bool connect)
        {
            updateRivalList(user, connect);
        }
    }
}
