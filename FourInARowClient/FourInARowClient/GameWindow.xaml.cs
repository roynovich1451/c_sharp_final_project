using FourInARowClient.FourInARowServiceReference;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FourInARowClient
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        #region DATA
        private FourInARowServiceClient clientToServer;
        private ClientCallback callback;
        private string rival;
        private string challanger;
        private string myUser;
        private int playerNum;
        private LobbyWindow lobbyWindow;
        private int gameID;
        #endregion

        #region Private fields       


        private readonly static int DISC_SIZE = 60;
        private readonly static int BOARD_SIZE = 7;
        private readonly static int HEIGHT_MARGIN = 20;
        private readonly static int BOTTOM_MARGIN = 10;
        private readonly static double WIDTH_MARGIN = 0.15;

        private SolidColorBrush brush;
        private int discCounter = 0;
        private readonly int[] board_state = new int[BOARD_SIZE];
        #endregion
        public GameWindow(string myUser, string challanger, string rival, 
            FourInARowServiceClient clientToServer, ClientCallback clientCallback, int gameID, LobbyWindow lobbyWindow)
        {
            InitializeComponent();
            applyWindowValues(myUser, challanger, rival, clientToServer, clientCallback, gameID, lobbyWindow);
            initCallbacks();
            makeTitle();
            lbID.Content = $"GameID: {gameID}";
        }
        private void applyWindowValues(string myUser, string challanger, string rival,
            FourInARowServiceClient clientToServer, ClientCallback clientCallback, int gameID, LobbyWindow lobbyWindow)
        {
            this.callback = clientCallback;
            this.clientToServer = clientToServer;
            this.rival = rival;
            this.challanger = challanger;
            this.myUser = myUser;
            this.gameID = gameID;
            this.lobbyWindow = lobbyWindow;
            this.playerNum = (myUser == challanger ? 0 : 1);
        }
        private void makeTitle()
        {
            if (myUser == challanger)
            {
                lbGameTitle.Content = $"{myUser} Vs {rival}";
            } else
            {
                lbGameTitle.Content = $"{myUser} Vs {challanger}";
            }
            
            if (myUser == challanger) lbGameTitle.Foreground = Brushes.Red;
            else lbGameTitle.Foreground = Brushes.Green;
        }
        private void initCallbacks()
        {
            callback.updateGame = DrawDisc;
            callback.endGame = EndGame;
        }
        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(myCanvas);
            if (!InWindow(p))
                return;
            int col = (int)Math.Floor(p.X / myCanvas.ActualWidth * BOARD_SIZE);
            MoveResult res = clientToServer.ReportMove(gameID, col, playerNum, false);
            if (res == MoveResult.InvalidMove)
            {
                MessageBox.Show("Invalid move", "Invalid", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }   
            if(res == MoveResult.NotYourTurn)
            {
                MessageBox.Show("Not your turn","Invalid", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DrawDisc(col);
            if (res == MoveResult.Draw) EndGame("Game Ended with Draw");
            if (res == MoveResult.YouWon) EndGame("You Won the Game");
        }

        private void DrawDisc(int col)
        { 
            brush = Brushes.Green;
            if (discCounter % 2 == 0)
                brush = Brushes.Red;
            Ellipse el = new Ellipse
            {
                Fill = brush,
                Height = DISC_SIZE,
                Width = DISC_SIZE
            };
            Disc newDisc = new Disc
            {
                Circle = el,
                Column = col,
                X = (col + WIDTH_MARGIN) / BOARD_SIZE * myCanvas.ActualWidth
            };
            if (board_state[newDisc.Column] == BOARD_SIZE)
                return;
            board_state[newDisc.Column]++;

            Canvas.SetTop(el, newDisc.Y);
            Canvas.SetLeft(el, newDisc.X);
            myCanvas.Children.Add(el);
            tbNumDiscs.Text = (++discCounter).ToString();
            ThreadPool.QueueUserWorkItem(DisplayDisc, newDisc);
        }

        private void DisplayDisc(Object obj)
        {
            Disc b = obj as Disc;
            while (true)
            {
                if (YOut(b))
                {
                    break;
                }
                Thread.Sleep(2);
                b.Y += 10;
                Dispatcher.Invoke(() =>
                {
                    Canvas.SetTop(b.Circle, b.Y);
                    Canvas.SetLeft(b.Circle, b.X);
                });
            }
        }

        private bool InWindow(Point p)
        {
            bool result = false;
            Dispatcher.Invoke(() =>
            {
                result = p.X > 0 &&
                p.X < myCanvas.ActualWidth &&
                p.Y > 0 &&
                p.Y < myCanvas.ActualHeight;
            });
            return result;
        }

        private bool YOut(Disc d)
        {
            bool result = false;
            Dispatcher.Invoke(() =>
            {
                result = d.Y > myCanvas.ActualHeight - ((DISC_SIZE + HEIGHT_MARGIN) * board_state[d.Column]) + BOTTOM_MARGIN || d.Y < 0;
            });
            return result;
        }

        private void EndGame(string message)
        {
            MessageBox.Show(message, "Game ended", MessageBoxButton.OK, MessageBoxImage.Information);
            Thread updateOtherThread = new Thread(() =>
            {
                clientToServer.NoticeAllGameStarted(challanger, rival, true);
            });
            updateOtherThread.Start();
            lobbyWindow.Show();
            this.Close();
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            Thread updateOtherThread = new Thread(() =>
            {
                clientToServer.ReportMove(gameID, 1, myUser==challanger? 1:0, true);
            });
            updateOtherThread.Start();
            lobbyWindow.Show();
        }
    }
}
