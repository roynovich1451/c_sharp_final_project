using FourInARowClient.FourInARowServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourInARowClient
{
    public class ClientCallback : IFourInARowServiceCallback
    {
        #region Delegates
        internal Action<string> endGame;
        internal Action<int> updateGame;
        internal Action startGame;

        public void AcceptGameInvitation()
        {
            startGame();
        }
        #endregion

        public void OtherPlayerConnected()
        {
            
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

        public void sendGameInvitation()
        {
            throw new NotImplementedException();
        }
    }
}
