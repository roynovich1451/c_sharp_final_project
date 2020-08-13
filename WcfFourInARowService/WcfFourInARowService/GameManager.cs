using System;

namespace WcfFourInARowService
{
    internal class GameManager
    {
        private static readonly int SIZE = 7;
        private static readonly int FULL_BOARD = 49;
        public string p1; //blue userName
        public string p2; //red userName
        private char current_player;
        private int blue_circles;
        private int red_circles;
        private char[,] board_state;
        private IFourInARowCallback p1Callback;
        private IFourInARowCallback p2Callback;

        public GameManager(string p1user, string p2user, IFourInARowCallback p1Callback, IFourInARowCallback p2Callback)
        {
            board_state = new char[SIZE, SIZE];
            current_player = 'b';
            blue_circles = 0;
            red_circles = 0;
            p1 = p1user;
            p2 = p2user;
            this.p1Callback = p1Callback;
            this.p2Callback = p2Callback;
        }

        internal MoveResult VerifyMove(int col, char player)
        {
            if (player != current_player)
            {
                return MoveResult.NotYourTurn;
            }
            if (board_state[0, col] != '\0')//check if col is ful
                return MoveResult.InvalidMove;

            int i = SIZE - 1;
            for (; i >= 0; i--)             //find an empty spot to insert
            {
                if (board_state[i, col] == '\0')
                    break;
            }
            if (ItsAWin(i, col))
            {
                return MoveResult.YouWon;
            }
            board_state[i, col] = current_player;
            if (current_player == 'b')      //counter for score purposes
                blue_circles++;
            else
                red_circles++;
            if (blue_circles + red_circles == FULL_BOARD)
            {
                return MoveResult.Draw;
            }
            return MoveResult.GameOn;
        }

        private bool ItsAWin(int row, int col)
        {
            Console.WriteLine("row " + row + " col " + col);
            if (board_state[row, col] == '\0')
                return false;
            return CheckRow(row, col) || CheckCol(row, col) || CheckDiagonal(row, col);
        }

        #region winChecks

        private bool CheckRow(int row, int col)
        {
            int sum = 1;

            for (int i = 1; i < 4; i++)
            {
                if (col - i >= 0)
                {
                    if (board_state[row, col - i] == board_state[row, col])
                    {
                        sum++; if (sum == 4)
                            return true;
                    }
                    else
                        break;
                }
                else break;
            }
            for (int i = 1; i < 4; i++)
            {
                if (col + i < SIZE)
                {
                    if (board_state[row, col + i] == board_state[row, col])
                    {
                        sum++; if (sum == 4)
                            return true;
                    }
                    else
                        break;
                }
                else break;
            }
            return false;
        }

        private bool CheckCol(int row, int col)
        {
            int sum = 1;

            for (int i = 1; i < 4; i++)
            {
                if (row - i >= 0)
                {
                    if (board_state[row - i, col] == board_state[row, col])
                    {
                        sum++; if (sum == 4)
                            return true;
                    }
                    else
                        break;
                }
                else break;
            }
            for (int i = 1; i < 4; i++)
            {
                if (row + i < SIZE)
                {
                    if (board_state[row + i, col] == board_state[row, col])
                    {
                        sum++; if (sum == 4)
                            return true;
                    }
                    else
                        break;
                }
                else break;
            }
            return false;
        }

        private bool CheckDiagonal(int row, int col)
        {
            //top left to bottom right
            int sum = 1;
            for (int i = 1; i < 4; i++)
            {
                if (row - i >= 0 && col - i >= 0)
                {
                    if (board_state[row - i, col - i] == board_state[row, col])
                    {
                        sum++;
                        if (sum == 4)
                            return true;
                    }
                    else
                        break;
                }
                else break;
            }
            for (int i = 1; i < 4; i++)
            {
                if (row + i < SIZE && col + i < SIZE)
                {
                    if (board_state[row + i, col + i] == board_state[row, col])
                        sum++; if (sum == 4)
                        return true;
                    else
                        break;
                }
                else break;
            }

            //bottom left to top right
            sum = 1;
            for (int i = 1; i < 4; i++)
            {
                if (row + i < SIZE && col - i >= 0)
                {
                    if (board_state[row + i, col - i] == board_state[row, col])
                    {
                        sum++; if (sum == 4)
                            return true;
                    }
                    else
                        break;
                }
                else break;
            }
            for (int i = 1; i < 4; i++)
            {
                if (row - i >= 0 && col + i < SIZE)
                {
                    if (board_state[row - i, col + i] == board_state[row, col])
                    {
                        sum++; if (sum == 4)
                            return true;
                    }
                    else
                        break;
                }
                else break;
            }
            return false;
        }

        #endregion winChecks

        public int CalculateScore(char player, char winner)
        {
            int score = 1;
            if (player == 'b')
                score *= blue_circles * 10;
            else
                score *= red_circles * 10;
            if (player == winner)
                score += 1000;
            if (CheckInCol(player))
                score += 100;
            return score;
        }

        private bool CheckInCol(char player)
        {
            bool ret;
            for (int i = 0; i < SIZE; i++)
            {
                ret = false;
                for (int j = 0; j < SIZE; j++)
                    if (board_state[i, j] == player)
                        ret = true;
                if (!ret)
                    return false;
            }
            return true;
        }

        /************for testing****************/

        #region testing

        public void Simulation()
        {
            while (true)
            {
                Random rnd = new Random();
                int m = 0;
                while (m < 100000000)
                    m++;
                int col = rnd.Next(7);
                while (board_state[0, col] != '\0')
                    col = rnd.Next(7);
                MoveResult res = VerifyMove(col, current_player);
                if (res == MoveResult.InvalidMove)
                    continue;
                if (res == MoveResult.YouWon || res == MoveResult.Draw)
                    break;
                PrintBoard();
                SwitchPlayer();
            }
            PrintBoard();
            Console.WriteLine(current_player + " WINS with " + (CalculateScore(current_player, current_player) + 1000));
            SwitchPlayer();
            //Console.WriteLine(current_player + " LOSES with " + CalculateScore(current_player));
            //Environment.Exit(1);
        }

        private void PrintBoard()
        {
            for (int i = 0; i < SIZE; i++)
            {
                Console.Write(i + " ");
                for (int j = 0; j < SIZE; j++)
                {
                    Console.Write(board_state[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("  0 1 2 3 4 5 6 " + Environment.NewLine);
        }

        private void SwitchPlayer()
        {
            if (current_player == 'b')
                current_player = 'r';
            else
                current_player = 'b';
        }

        #endregion testing
    }
}