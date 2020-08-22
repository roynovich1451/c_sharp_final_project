using FourInARowClient.FourInARowServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FourInARowClient
{
    /// <summary>
    /// Interaction logic for LobbyWindow.xaml
    /// </summary>
    public partial class LobbyWindow : Window
    {
        private string myUser;
        private ClientCallback callback;
        private FourInARowServiceClient clientToServer;
        private List<string> availableRivals;


        public LobbyWindow(string userName, ClientCallback cc, FourInARowServiceClient fc)
        {
            InitializeComponent();
            lbUser.Content += userName;
            clientToServer = fc;
            callback = cc;
            myUser = userName;
            initRivalList(myUser);
            updateLobbyStats();
            callback.myUser = myUser;
            callback.startGame += challengeAccepted;
            callback.popUpGameInvitation += popInvitation;
            callback.updateRivalList += updateRivalList;
            clientToServer.NoticeAll(myUser, true);
        }

        private void updateLobbyStats()
        {
            updateStats(myUser, true);
            fillTop3();
        }
        private void fillTop3()
        {
            var dict = clientToServer.getTopThreeUsers();
            int cnt = dict.Count;
            
            if(cnt >= 1)
            {
                tbTop1Name.Text = dict[0].Item1;
                tbTop1Points.Text = dict[0].Item2.ToString();
            }
            if (cnt >= 2)
            {
                tbTop2Name.Text = dict[1].Item1;
                tbTop2Points.Text = dict[1].Item2.ToString();
            }
            if (cnt == 3)
            {
                tbTop3Name.Text = dict[2].Item1;
                tbTop3Points.Text = dict[2].Item2.ToString();
            }
        }
        private void lbRivals_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            String userName = lbRivals.SelectedItem.ToString();
            updateStats(userName, false);
        }
        private void updateStats(string userName, bool where)
        {
            var stats = clientToServer.getUserStats(userName);
            if (where == true) //My stats
            {
                tbMyCarrer.Text = stats["Games"];
                tbMyLosses.Text = stats["Losses"];
                tbMyPoint.Text = stats["Points"];
                tbMyWins.Text = stats["Wins"];
                if (int.Parse(tbMyCarrer.Text) != 0)
                {
                    tbMyPercantage.Text = ((Double.Parse(tbMyWins.Text) / Double.Parse(tbMyCarrer.Text)) * 100).ToString().Substring(0,4);
                }
                else
                {
                    tbMyPercantage.Text = "0";
                }
                
            }
            else //Selected rival stats
            {
                tbRivalCarrer.Text = stats["Games"];
                tbRivalLosses.Text = stats["Losses"];
                tbRivalPoint.Text = stats["Points"];
                tbRivalWins.Text = stats["Wins"];
                if (int.Parse(tbMyCarrer.Text) != 0)
                {
                    tbRivalPercantage.Text = ((Double.Parse(tbMyWins.Text) / Double.Parse(tbMyCarrer.Text)) * 100).ToString().Substring(0, 4);
                }
                else
                {
                    tbRivalPercantage.Text = "0";
                }
            }
            
        }
        private void initRivalList(string user)
        {
            availableRivals = new List<string>();
            availableRivals = clientToServer.GetConnectedClients(myUser).Keys.ToList();
            lbRivals.ItemsSource = availableRivals;
        }
        private void updateRivalList(string user, bool add)
        {
            if (user == myUser) return;
            if (add == true)
            {
                availableRivals.Add(user);
                lbRivals.ItemsSource = null;
                lbRivals.ItemsSource = availableRivals;
                lbRivals.Items.Refresh();
            }
            else //remove
            {
                availableRivals.Remove(user);
                lbRivals.ItemsSource = null;
                lbRivals.ItemsSource = availableRivals;
                lbRivals.Items.Refresh();
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            clientToServer.Disconnect(myUser, -1);
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            if (lbRivals.SelectedItem == null)
            {
                MessageBox.Show("Must pick a Rival to start new game", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string rival = lbRivals.SelectedItem.ToString();
             bool response = clientToServer.ChallengeRival(rival, myUser);
            if (response == false)
            {
                MessageBox.Show($"{rival} declined your game invitation...\nwhat a chicken!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                challengeAccepted(rival);
                clientToServer.StartNewGame(myUser, rival);
            }            
        }

        private void challengeAccepted(string rival)
        {
            GameWindow liveGame = new GameWindow(myUser, rival, clientToServer, callback);
            updateRivalList(rival, false);
            liveGame.ShowDialog();
        }

        internal bool popInvitation(string challenger)
        {
            MessageBoxResult res = MessageBox.Show($"{challenger} has sent you a game request\nDo you accept the challenge?", "Game Invitation", MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (res == MessageBoxResult.Yes) return true;
            return false;
        }

        private void btnStatsCenter_Click(object sender, RoutedEventArgs e)
        {
            /* TODO: bring back when games available 
            var list = clientToServer.getGamesHistory().ToList();
            if (list.Count == 0)
            {
                MessageBox.Show("No games played yet.\nNo data available.\nBack to lobby.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            */
            StatsWindow stats = new StatsWindow(clientToServer, callback);
            stats.ShowDialog();
        }

        private void btnLiveGames_Click(object sender, RoutedEventArgs e)
        {
            List<string> list = clientToServer.getLiveGames().ToList();
            if (list.Count == 0)
            {
                MessageBox.Show($"There are no Live games for the moment", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            LiveGamesWindow lg = new LiveGamesWindow(list);
            lg.Show(); //TODO: decide if need to be Show or ShowDialog..
        }


    }
}
