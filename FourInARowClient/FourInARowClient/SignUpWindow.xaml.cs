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
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public SignUpWindow(ClientCallback cc, FourInARowServiceClient fc)
        {
            InitializeComponent();
            callback = cc;
            clientToServer = fc;
        }

        ClientCallback callback;
        FourInARowServiceClient clientToServer;

        #region passChecks
        private string ConvertPass(string pass)
        {
            using (SHA256 hashObj = SHA256.Create())
            {
                byte[] hashBytes = hashObj.ComputeHash(Encoding.UTF8.GetBytes(pass));
                StringBuilder builder = new StringBuilder();
                foreach(byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private bool checkPassWeaknnes(string pass)
        {
            if (pass.Length < 8)
            {
                return false;
            }
            foreach(char c in pass)
            {
                if (char.IsUpper(c)) return true;
            }
            return false;
        }
        #endregion
  
        private void btnSigUp_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbUser.Text) ||
               string.IsNullOrEmpty(tbPassword.Password) ||
               string.IsNullOrEmpty(tbRePassword.Password))
            {
                MessageBox.Show("Please fill all data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!tbPassword.Password.Trim().Equals(tbRePassword.Password.Trim()))
            {
                MessageBox.Show("Passwords not equal", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!checkPassWeaknnes(tbPassword.Password.Trim()))
            {
                MessageBox.Show("*Minimum password length 8 characters\n*Password must have one or more capital letter", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                string userName = tbUser.Text.Trim();
                string pass = ConvertPass(tbPassword.Password.Trim());
                clientToServer.Register(userName, pass);
                var list = clientToServer.GetConnectedClients(tbUser.Text.Trim());
                LobbyWindow lw = new LobbyWindow(userName, callback, clientToServer);
                lw.Show();
                this.Hide();
            }
            catch (FaultException<UserNameInUse> fault)
            {
                MessageBox.Show(fault.Detail.Details, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + "Type:" + ex.GetType() + "\n" + ex.InnerException, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
