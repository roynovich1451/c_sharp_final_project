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
        public LobbyWindow(string userName, ClientCallback cc, FourInARowServiceClient fc)
        {
            InitializeComponent();
            lbUser.Content += userName;
            clientToServer = fc;
            callback = cc;
            myUser = userName;
            updateRivalList(myUser);

            cc.myUser = myUser;
            cc.updateLiveGameId += updateLiveGameId;
            cc.updateRivalList += updateRivalList;
            
        }

        ClientCallback callback;
        FourInARowServiceClient clientToServer;
        string myUser;
        int LiveGameId;

        private void updateRivalList(string user)
        {
            lbRivals.ItemsSource = clientToServer.GetConnectedClients(myUser);
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            clientToServer.Disconnect(myUser);
        }

        private void btnRefreshRivals_Click(object sender, RoutedEventArgs e)
        {
            refresh_rivals_list();
        }

        private void refresh_rivals_list()
        {
            var list = clientToServer.GetConnectedClients(myUser).Keys.ToList();
            lbRivals.ItemsSource = list;
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
                clientToServer.StartNewGame(myUser, rival);
                GameWindow gw = new GameWindow();
                gw.Show();
                this.Hide();
            }
        }
        
        internal void updateLiveGameId(int id)
        {
            LiveGameId = id;
        }

        internal bool popInvitation(string chalanger)
        {
            MessageBoxResult res = MessageBox.Show($"{chalanger} has sent you a game request\nDo you exept the chalange?", "Game Invitation", MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (res == MessageBoxResult.Yes) return true;
            return false;
        }
    }
}
