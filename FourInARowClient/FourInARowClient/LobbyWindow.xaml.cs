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

            callback.myUser = myUser;
            callback.startGame += challengeAccepted;
            callback.popUpGameInvitation += popInvitation;
            callback.updateRivalList += updateRivalList;
        }

        private void initRivalList(string user)
        {
            lbRivals.ItemsSource = clientToServer.GetConnectedClients(myUser).Keys.ToList();
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
            //clientToServer.Disconnect(myUser);
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
    }
}
