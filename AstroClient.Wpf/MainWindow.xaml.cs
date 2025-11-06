using AstroClient.Wpf.ViewModels;
using AstroClient.Wpf.Services;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace AstroClient.Wpf
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new MainViewModel(); //create the ViewModel
            DataContext = vm;

            vm.LanguageChanged += (sender, cultureName) => App.SetCulture(cultureName);
        }

        // Handles the Velocity button click
        private async void VelocityButton_Click(object sender, RoutedEventArgs e)
        {
            // Call the public method on the ViewModel
            await vm.CalcVelocityAsync();
        }

        // Handles the Distance button click
        private async void DistanceButton_Click(object sender, RoutedEventArgs e)
        {
            await vm.CalcDistanceAsync();
        }

        // Handles the Kelvin button click
        private async void KelvinButton_Click(object sender, RoutedEventArgs e)
        {
            await vm.CalcKelvinAsync();
        }

        // Handles the Event Horizon button click
        private async void EventHorizonButton_Click(object sender, RoutedEventArgs e)
        {
            await vm.CalcEventHorizonAsync();
        }

        private void RootGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // move focus to grid so any TextBox loses focus
            RootGrid.Focus();
        }
    }
}