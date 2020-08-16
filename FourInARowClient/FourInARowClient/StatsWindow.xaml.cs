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
        FourInARowServiceClient clientToServeice;
        ClientCallback callback;

        public StatsWindow(FourInARowServiceClient fc, ClientCallback cc)
        {
            clientToServeice = fc;
            callback = cc;
            InitializeComponent();
        }

        private void initAllGames()
        {
            lbAllGames.ItemsSource = clientToServeice.getGamesHistory().ToList();
        }

        private void initComboBoxes()
        {
            sortOptions = new List<string> { "Name", "Games", "Wins", "Looses", "Points" };
            cmbSort.ItemsSource = sortOptions;
            cmbSort.SelectedIndex = 0;
            users = new List<string>();
            users = clientToServeice.getAllUserNames().ToList();
            cmbP1.ItemsSource = users;
            cmbP2.ItemsSource = users;
        }

        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            lbSortResults.ItemsSource = clientToServeice.createSortedList(cmbSort.SelectedItem.ToString());
            return;
        }

        private void btnRivary_Click(object sender, RoutedEventArgs e)
        {
            if (cmbP1 == null || cmbP2 == null)
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
            List<string> ret = clientToServeice.createRivaryData(p1, p2).ToList<string>();
            labelP1.Content = $"{p1} wins percantage: {ret[0]}";
            labelP2.Content = $"{p2} wins percantage: {ret[1]}";
            ret.RemoveAt(0);
            ret.RemoveAt(1);
            lbMatchup.ItemsSource = ret;
        }
    }
}
