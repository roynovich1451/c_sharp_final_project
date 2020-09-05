using FourInARowClient.FourInARowServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

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
        private bool hadGame = true;

        public LobbyWindow(string userName, ClientCallback cc, FourInARowServiceClient fc)
        {
            InitializeComponent();
            clientToServer = fc;
            callback = cc;
            myUser = userName;
            lbUser.Content += myUser;
            initRivalList(myUser);
            updateLobbyStats();
            initCallbacks();
        }

        #region callbacks
        /// <summary>
        /// initial client callbacks
        /// </summary>
        private void initCallbacks()
        {
            callback.myUser = myUser;
            callback.startGame += challengeAccepted;
            callback.popUpGameInvitation += popInvitation;
            callback.updateRivalList += updateRivalList;
            clientToServer.NoticeAll(myUser, true);
        }
        /// <summary>
        /// open new game after confirmation
        /// </summary>
        /// <param name="challanger"></param>
        /// <param name="rival"></param>
        /// <param name="gameID"></param>
        private void challengeAccepted(string challanger, string rival, int gameID)
        {
            GameWindow liveGame = new GameWindow(myUser, challanger, rival, clientToServer, callback, gameID, this);
            liveGame.Show();
            hadGame = true;
            this.Hide();
        }
        /// <summary>
        /// show invitation message
        /// </summary>
        /// <param name="Challenger"></param>
        /// <returns></returns>
        internal bool popInvitation(string Challenger)
        {
            MessageBoxResult res = MessageBox.Show($"{Challenger} has sent you a game request\nDo you accept the challenge?", "Game Invitation", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (res == MessageBoxResult.Yes)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region helpers
        /// <summary>
        /// update the stats in lobby
        /// </summary>
        public void updateLobbyStats()
        {
            if (hadGame == false) return;
            hadGame = false;
            updateStats(myUser, true);
            fillTop3();
        }
        /// <summary>
        /// show top 3 users point-wise
        /// </summary>
        public void fillTop3()
        {
            var dict = clientToServer.getTopThreeUsers();
            int cnt = dict.Count;

            if (cnt >= 1)
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
        /// <summary>
        /// reset rival's stats if no rivals available
        /// </summary>
        private void resetRivalStats()
        {
            tbRivalCarrer.Text = "";
            tbRivalLosses.Text = "";
            tbRivalPoint.Text = "";
            tbRivalWins.Text = "";
            tbRivalPercantage.Text = "";
        }
        /// <summary>
        /// update grid stats for user or rival
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="where"></param>
        public void updateStats(string userName, bool where)
        {
            var stats = clientToServer.getUserStats(userName);
            if (where == true) //My stats
            {
                tbMyCarrer.Text = stats["Games"];
                tbMyLosses.Text = stats["Losses"];
                tbMyPoint.Text = stats["Points"];
                tbMyWins.Text = stats["Wins"];
                float wins = float.Parse(stats["Wins"]);
                int games = int.Parse(stats["Games"]);
                if (int.Parse(tbMyCarrer.Text) != 0)
                {
                    string per = (wins / games * 100).ToString();
                    if (per.Length > 4)
                        tbMyPercantage.Text = per.Substring(0, 4);
                    else tbMyPercantage.Text = per;

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
                float wins = float.Parse(stats["Wins"]);
                int games = int.Parse(stats["Games"]);
                if (int.Parse(tbRivalCarrer.Text) != 0)
                {
                    string per = (wins / games * 100).ToString();
                    if (per.Length > 4)
                        tbRivalPercantage.Text = per.Substring(0, 4);
                    else tbRivalPercantage.Text = per;
                }
                else
                {
                    tbRivalPercantage.Text = "0";
                }
            }

        }
        /// <summary>
        /// update available rivals list
        /// </summary>
        /// <param name="user"></param>
        /// <param name="add"></param>
        private void updateRivalList(string user, bool add)
        {
            if (user == myUser) return;
            if (availableRivals.Contains(user) && add) return;
            if (!availableRivals.Contains(user) && false) return;
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
        /// <summary>
        /// take available rivals from service in boot time
        /// </summary>
        /// <param name="user"></param>
        private void initRivalList(string user)
        {
            availableRivals = new List<string>();
            availableRivals = clientToServer.GetConnectedClients(myUser).ToList();
            lbRivals.ItemsSource = availableRivals;
        }
        #endregion

        #region buttons
        private void lbRivals_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lbRivals.SelectedItem != null)
            {
                String userName = lbRivals.SelectedItem.ToString();
                updateStats(userName, false);
            }
            else
            {
                resetRivalStats();
            }
        }
        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            if (lbRivals.SelectedItem == null)
            {
                MessageBox.Show("Must pick a Rival to start new game", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string rival = lbRivals.SelectedItem.ToString();
            int gameID = clientToServer.ChallengeRival(rival, myUser);
            if (gameID == -1)
            {
                MessageBox.Show($"{rival} declined your game invitation...\nwhat a chicken!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                challengeAccepted(myUser, rival, gameID);
                clientToServer.StartNewGame(myUser, rival);
            }
        }
        private void btnStatsCenter_Click(object sender, RoutedEventArgs e)
        {
            var list = clientToServer.getGamesHistory().ToList();
            if (list.Count == 0)
            {
                MessageBox.Show("No games played yet.\nNo data available.\nBack to lobby.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
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
            lg.Show();
        }
        #endregion

        #region windowFunc
        private void Window_Closing(object sender, EventArgs e)
        {
            clientToServer.Disconnect(myUser, -1);
            Environment.Exit(Environment.ExitCode);
        }

        private void gameGrid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            updateLobbyStats();
        }
        #endregion
    }
}
