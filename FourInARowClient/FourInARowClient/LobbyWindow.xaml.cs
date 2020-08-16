using FourInARowClient.FourInARowServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
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
            callback.startGame += chalangeAccepted;
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
            if(add == true)
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
            clientToServer.Disconnect(myUser);
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            if (lbRivals.SelectedItem == null)
            {
                MessageBox.Show("Must pick Rival to start new game", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string rival = lbRivals.SelectedItem.ToString();
            bool respond = clientToServer.ChalangeRival(rival, myUser);
            if (respond == false)
            {
                MessageBox.Show($"{rival} diclined your game invitation...\nwhat a chicken!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                chalangeAccepted(rival);
                clientToServer.StartNewGame(myUser, rival);
            }
        }
        
        private void chalangeAccepted(string rival)
        {
            GameWindow liveGame = new GameWindow(myUser, rival, clientToServer, callback);
            updateRivalList(rival, false);
            liveGame.ShowDialog();
        }

        internal bool popInvitation(string chalanger)
        {
            MessageBoxResult res = MessageBox.Show($"{chalanger} has sent you a game request\nDo you exept the chalange?", "Game Invitation", MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (res == MessageBoxResult.Yes) return true;
            return false;
        }

        private
    }
}
