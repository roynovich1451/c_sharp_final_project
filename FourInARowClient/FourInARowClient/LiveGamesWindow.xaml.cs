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
    /// Interaction logic for LiveGamesWindow.xaml
    /// </summary>
    public partial class LiveGamesWindow : Window
    {
        public LiveGamesWindow(List<string> display)
        {
            InitializeComponent();
            updateDisplay(display);
        }

        private void updateDisplay(List<string> display)
        {
            lbLiveGames.ItemsSource = null;
            lbLiveGames.ItemsSource = display;
        }
    }
}
