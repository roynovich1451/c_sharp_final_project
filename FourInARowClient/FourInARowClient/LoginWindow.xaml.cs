using FourInARowClient.FourInARowReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        int numUser;

        FourInARowServiceClient client;
        ClientCallback callback;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            callback = new ClientCallback();
            client = new FourInARowServiceClient(new InstanceContext(callback));
            numUser = client.Register();
            if (numUser % 2 == 0)
            {
                StartGame();
            }
            else
            {
                //tbMessages.Text = "You are connected.\nWait for other player to connect";
                callback.startGame += StartGame;
                (sender as Button).IsEnabled = false;
            }
        }

        bool gameStarted = false;

        private void StartGame()
        {
            gameStarted = true;
            GameWindow game = new GameWindow();
            game.NumUser = numUser;
            game.Client = client;
            game.Callback = callback;
            game.Show();
            this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (!gameStarted)
                client.DisconnectBeforeGame(numUser);
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbUser.Text) &&
                !string.IsNullOrEmpty(tbPassword.Password))
            {
                FourInARowServiceClient client = new FourInARowServiceClient(new InstanceContext(callback));
                string userName = tbUser.Text.Trim();
                try
                {
                    client.
                }
            }

        }
    }
}
