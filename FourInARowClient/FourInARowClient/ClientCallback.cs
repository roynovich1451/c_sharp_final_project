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
        internal Action<string, string, int> startGame;
        internal Action<string, bool> updateRivalList;
        internal Action<string> endGame;
        internal string myUser;
        #endregion

        public void NewPlayerConnected(string user)
        {
            updateRivalList(user, true);
        }

        public void OtherPlayerMoved(MoveResult moveResult, int col)
        {
            if (moveResult == MoveResult.YouLost)
            {
                endGame("Other player retired\nYou Won!!!");
                return;
            }
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
            return popUpGameInvitation(challenger);
        }

        public void StartGameAgainstRival(string challenger, string rival, int gameID)
        {
            startGame(challenger, rival, gameID);
        }

        public void RivalStartGame(string p1, string p2)
        {
            throw new NotImplementedException();
        }


        public void OtherPlayerDisOConnnectd(string user, bool connect)
        {
            updateRivalList(user, connect);
        }

    }
}
