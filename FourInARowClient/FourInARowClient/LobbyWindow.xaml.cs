﻿using FourInARowClient.FourInARowServiceReference;
using System;
using System.Linq;
using System.Windows;
using System.Timers;

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
            clientToServer = fc;
            callback = cc;
            lbRivals.ItemsSource = clientToServer.GetConnectedClients().Keys.ToList();
            myUser = userName;
            InitTimer();
            timer1.Start();
        }

        ClientCallback callback;
        FourInARowServiceClient clientToServer;
        string myUser;
        private System.Timers.Timer timer1;

        private void Window_Closed(object sender, EventArgs e)
        {
            clientToServer.Disconnect(myUser);
        }
        
        private void UpdateRivals(Object source, ElapsedEventArgs e)
        {
            lbRivals.ItemsSource = clientToServer.GetConnectedClients().Keys.ToList();
        }

       
        public void InitTimer()
        {
            timer1 = new System.Timers.Timer(5000);
            timer1.Elapsed += UpdateRivals;
            timer1.AutoReset = true;
            timer1.Enabled = true;
        }

        private void BtnRefreshRivals_Click(object sender, RoutedEventArgs e)
        {
            lbRivals.ItemsSource = clientToServer.GetConnectedClients().Keys.ToList();
        }

        private void BtnStartGame_Click(object sender, RoutedEventArgs e)
        {
            if (lbRivals.SelectedItem == null)
            {
                MessageBox.Show("Must pick Rival to start new game", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Console.WriteLine("my rival: " + lbRivals.SelectedItem.ToString());
            clientToServer.StartNewGame(myUser, lbRivals.SelectedItem.ToString());
            GameWindow gw = new GameWindow();
            gw.Show();
            this.Hide();
        }
    }
}
