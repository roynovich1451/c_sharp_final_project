using FourInARowClient.FourInARowServiceReference;
using System;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Windows;

namespace FourInARowClient
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        ClientCallback callback;
        FourInARowServiceClient clientToServer;
        public LoginWindow()
        {
            InitializeComponent();
            callback = new ClientCallback();
            clientToServer = new FourInARowServiceClient(new InstanceContext(callback));
 
        }
        #region buttons
        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbUser.Text) &&
                !string.IsNullOrEmpty(tbPassword.Password))
            {
                try
                {
                    clientToServer.ClientConnect(tbUser.Text.Trim(), ConvertPass(tbPassword.Password.Trim()));
                    LobbyWindow lw = new LobbyWindow(tbUser.Text.Trim(), callback, clientToServer);
                    lw.Show();
                    this.Hide();
                }
                catch (FaultException<UserConnectedFault> fault)
                {
                    MessageBox.Show(fault.Detail.Details, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (FaultException<UserNotRegisteredFault> fault)
                {
                    MessageBox.Show(fault.Detail.Details, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (FaultException<UserNameInUse> fault)
                {
                    MessageBox.Show(fault.Detail.Details, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (FaultException<IncorrectPasswordFault> fault)
                {
                    MessageBox.Show(fault.Detail.Details, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (FaultException<ExceptionDetail> fault)
                {
                    MessageBox.Show(fault.Detail.InnerException.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + "Type:" + ex.GetType() + "\n" + ex.InnerException, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
        #endregion
        #region helpers
        /// <summary>
        /// hashes user's password
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
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
        #endregion

    }
}
