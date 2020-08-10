using FourInARowClient.FourInARowServiceReference;
using System;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Windows;

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
        private bool CheckPassWeaknnes(string pass)
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
  
        private void BtnSigUp_Click(object sender, RoutedEventArgs e)
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
            if (!CheckPassWeaknnes(tbPassword.Password.Trim()))
            {
                MessageBox.Show("*Minimum password length 8 characters\n*Password must have one or more capital letter", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                clientToServer.Register(tbUser.Text.Trim(), ConvertPass(tbPassword.Password.Trim()));
                var list = clientToServer.GetConnectedClients();
                LobbyWindow lw = new LobbyWindow(tbUser.Text.Trim(), callback, clientToServer);
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
