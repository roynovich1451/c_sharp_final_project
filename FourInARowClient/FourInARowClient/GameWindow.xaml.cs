using FourInARowClient.FourInARowServiceReference;
using System;
using System.Collections.Generic;
using System.ServiceModel;
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
        private string myRival;
        private string myUser;
        #endregion

        #region Private fields       
        private bool serverOn = false;

        private readonly static int DISC_SIZE = 60;
        private readonly static int BOARD_SIZE = 7;
        private readonly static int HEIGHT_MARGIN = 20;
        private readonly static int BOTTOM_MARGIN = 10;
        private readonly static double WIDTH_MARGIN = 0.15;

        private SolidColorBrush brush;
        private int discCounter = 0;
        private readonly int[] board_state = new int[BOARD_SIZE];
        #endregion
        public GameWindow(string myUser, string rival, FourInARowServiceClient clientToServer, ClientCallback clientCallback)
        {
            callback = clientCallback;
            this.clientToServer = clientToServer;
            myRival = rival;
            this.myUser = myUser;

            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (serverOn)
            {
                Client.Disconnect(NumUser.ToString());
            }
            Environment.Exit(Environment.ExitCode);
        }
        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(myCanvas);
            int col = (int)Math.Floor(p.X / myCanvas.ActualWidth * BOARD_SIZE);
            if (!InWindow(p))
                return;
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
                b.Y += 1;
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

        /*        private void button1_Click(object sender, RoutedEventArgs e)
                {
                    try
                    {
                        Button clickedButton = (Button)sender;
                        int location = Convert.ToInt32(clickedButton.Name.Substring(6, 1)) - 1;
                        var moveResult = Client.ReportMove(location, NumUser);
                        if (moveResult == MoveResult.NotYourTurn)
                        {
                            MessageBox.Show("Not your turn");
                            return;
                        }               
                        if (moveResult == MoveResult.Draw)
                        {
                            EndGame("Its a Draw");
                        }
                        else if (moveResult == MoveResult.YouWon)
                        {
                            EndGame("You won!");
                        }

                    }
                    catch (FaultException<OpponentDisconnectedFault> ex)
                    {
                        DisableBoard();
                        MessageBox.Show(ex.Detail.Details);
                        Client.Disconnect(NumUser);
                    }
                    catch (TimeoutException)
                    {
                        MessageBox.Show("Server disconnected");
                        DisableBoard();
                        serverOn = false;
                    }
                }
        */

        private void EndGame(string message)
        {
            MessageBox.Show(message);
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}
