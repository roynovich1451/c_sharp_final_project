using FourInARowClient.FourInARowServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            callback = new ClientCallback();
            clientToServer = new FourInARowServiceClient(new InstanceContext(callback));
        }
        int numUser;
        ClientCallback callback;
        FourInARowServiceClient clientToServer;

        bool gameStarted = false;

        private void StartGame()
        {
            gameStarted = true;
            GameWindow game = new GameWindow();
            game.NumUser = numUser;
            game.Client = clientToServer;
            game.Callback = callback;
            game.Show();
            this.Hide();
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbUser.Text) &&
                !string.IsNullOrEmpty(tbPassword.Password))
            {
                try
                {
                    clientToServer.ClientConnect(tbUser.Text.Trim(), ConvertPass(tbPassword.Password.Trim()));
                }
                catch (FaultException<UserConnectdFault> fault)
                {
                    MessageBox.Show(fault.Detail.Details, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (FaultException<UserNotRegisteredFault> fault)
                {
                    MessageBox.Show(fault.Detail.Details, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (FaultException<IncorectPasswordFault> fault)
                {
                    MessageBox.Show(fault.Detail.Details, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message+"\n"+"Type:"+ex.GetType()+"\n"+ex.InnerException, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                LobbyWindow lw = new LobbyWindow(tbUser.Text.Trim());
                lw.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("User name or password missing", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSigUp_Click(object sender, RoutedEventArgs e)
        {
            SignUpWindow suw = new SignUpWindow(callback, clientToServer);
            suw.Show();
            this.Hide();
        }

        private string ConvertPass(string pass)
        {
            using (SHA256 hashObj = SHA256.Create())
            {
                byte[] hashBytes = hashObj.ComputeHash(Encoding.UTF8.GetBytes(pass));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
