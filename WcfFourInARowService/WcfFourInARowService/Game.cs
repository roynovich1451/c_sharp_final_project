using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfFourInARowService
{
    class Game
    {
        int numPlays = 0;
        private char[] board; // the local representation of the game board  
        private int currentPlayer; // keep track of whose turn it is         
        static private int[,] triples ={{0,1,2},// possible winning triples
                            {3,4,5},
                            {6,7,8},
                            {0,3,6},
                            {1,4,7},
                            {2,5,8},
                            {0,4,8},
                            {2,4,6}};

        public Game()
        {
            board = new char[9];
            currentPlayer = 1;
        }

        internal MoveResult VerifyMove(int location, char player)
        {
            int index = player == 'X' ? 1 : 0;

            if (index != currentPlayer)
            {
                return MoveResult.NotYourTurn;
            }

            numPlays++;
            board[location] = player;
            currentPlayer = (currentPlayer + 1) % 2;
            if (ItsAWin())
            {
                return MoveResult.YouWon;
            }
            if (numPlays == 9)
            {
                return MoveResult.Draw;
            }
            return MoveResult.GameOn;
        }

        private bool ItsAWin()
        {
            for (int i = 0; i < 8; i++)
            {
                if (board[triples[i, 0]] != 0 &&
                    board[triples[i, 0]] == board[triples[i, 1]] &&
                   board[triples[i, 1]] == board[triples[i, 2]])
                    return true;
            }
            return false;
        } // end method GameOver
    }
}
