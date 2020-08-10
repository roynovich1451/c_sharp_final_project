using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows;

namespace FourInARowHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        ServiceHost host;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            host = new ServiceHost(typeof(WcfFourInARowService.FourInARowService));
            host.Description.Behaviors.Add(
                new ServiceMetadataBehavior { HttpGetEnabled = true });
            try
            {
                host.Open();
                lbStatus.Content = "Service is running...";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
