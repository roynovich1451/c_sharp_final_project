using FourInARowClient.FourInARowServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FourInARowClient
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        #region Properties
        public int NumUser { get; internal set; }
        public FourInARowServiceClient Client { get; internal set; }
        public ClientCallback Callback { get; internal set; }
        #endregion

        #region Private fields
        private Dictionary<int, Button> buttons = new Dictionary<int, Button>();
        private string mySign;
        private string otherSign;
        private bool serverOn = false;
        #endregion
        public GameWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in mainGrid.Children)
            {
                if (item is Button)
                {
                    Button b = item as Button;
                    buttons.Add(Convert.ToInt32(b.Name.Substring(6, 1)), b);
                }
            }
            mySign = NumUser % 2 == 1 ? "X" : "O";
            otherSign = mySign == "O" ? "X" : "O";
            Title = $"Player {mySign}";
            Callback.endGame += EndGame;
            Callback.updateGame += UpdateGame;
            serverOn = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (serverOn)
            {
                //Client.Disconnect(NumUser);
            }
            Environment.Exit(Environment.ExitCode);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button clickedButton = (Button)sender;
                int location = Convert.ToInt32(clickedButton.Name.Substring(6, 1)) - 1;
                var moveResult = Client.ReportMove(1,location, 1);
                if (moveResult == MoveResult.NotYourTurn)
                {
                    MessageBox.Show("Not your turn");
                    return;
                }
                clickedButton.Content = mySign;
                clickedButton.IsEnabled = false;
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
               // Client.Disconnect(NumUser);
            }
            catch (TimeoutException)
            {
                MessageBox.Show("Server disconnected");
                DisableBoard();
                serverOn = false;
            }
        }

        private void UpdateGame(int location)
        {
            Button b = buttons[location + 1];
            b.Content = otherSign;
            b.IsEnabled = false;
        }

        private void EndGame(string message)
        {
            MessageBox.Show(message);
            Thread connectionThread = new Thread(() => { Client.Disconnect(NumUser.ToString()); });
            connectionThread.Start();
            DisableBoard();
        }

        private void DisableBoard()
        {
            foreach (var item in buttons.Keys)
            {
                buttons[item].IsEnabled = false;
            }
        }

    }
}
