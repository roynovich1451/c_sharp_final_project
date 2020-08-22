using FourInARowClient.FourInARowServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for StatsWindow.xaml
    /// </summary>
    public partial class StatsWindow : Window
    {
        List<string> sortOptions;
        List<string> users;
        FourInARowServiceClient clientToServer;
        ClientCallback callback;

        public StatsWindow(FourInARowServiceClient fc, ClientCallback cc)
        {
            clientToServer = fc;
            callback = cc;
            InitializeComponent();
            initAllGames();
            initComboBoxes(); 
        }

        private void initAllGames()
        {
            var list = clientToServer.getGamesHistory().ToList();
            lbAllGames.ItemsSource = list; 
        }

        private void initComboBoxes()
        {
            labelP1.Visibility = Visibility.Hidden;
            labelP2.Visibility = Visibility.Hidden;
            sortOptions = new List<string> { "Name", "Games", "Wins", "Looses", "Points" };
            cmbSort.ItemsSource = sortOptions;
            users = clientToServer.getAllUserNames().ToList();
            cmbP1.ItemsSource = users;
            cmbP2.ItemsSource = users;
        }

        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            string type = cmbSort.SelectedItem.ToString();
            gbSort.Header = $"Sort by {type} results";
            lbSortResults.ItemsSource = null;
            lbSortResults.ItemsSource = clientToServer.createSortedList(type);
            return;
        }

        private void btnRivary_Click(object sender, RoutedEventArgs e)
        {
            if (cmbP1.SelectedIndex == -1 || cmbP2.SelectedIndex == -1)
            {
                MessageBox.Show("Must pick 2 players to see rivary history", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string p1 = cmbP1.SelectedItem.ToString();
            string p2 = cmbP2.SelectedItem.ToString();
            if (p1.Equals(p2))
            {
                MessageBox.Show("Must pick 2 diffrent players", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var ret = clientToServer.createRivaryData(p1, p2).ToList();
            if(ret.Count == 0)
            {
                lbMatchup.ItemsSource = null;
                MessageBox.Show($"No games played between {p1} and {p2}", "Rivary info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                labelP1.Content = $"{p1} wins percantage: {ret[0]}";
                labelP2.Content = $"{p2} wins percantage: {ret[1]}";
                labelP1.Visibility = Visibility.Visible;
                labelP2.Visibility = Visibility.Visible;
                ret.RemoveAt(1);
                ret.RemoveAt(0);
                lbMatchup.ItemsSource = ret;
            }
        }
    }
}
