using AstroClient.Wpf.ViewModels;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainViewModel();
            DataContext = vm;

            vm.PropertyChanged += (s, e) =>
            {
                // Language change (already added)
                if (e.PropertyName == nameof(MainViewModel.SelectedLanguage))
                    App.SetCulture(vm.SelectedLanguage);

                // Theme change: swap the first merged dictionary
                if (e.PropertyName == nameof(MainViewModel.SelectedTheme))
                {
                    // fallback to light when null/unknown
                    var theme = (vm.SelectedTheme ?? "light").ToLowerInvariant() == "dark" ? "Dark" : "Light";
                    var dict = new ResourceDictionary { Source = new Uri($"Themes/{theme}.xaml", UriKind.Relative) };
                    Application.Current.Resources.MergedDictionaries[0] = dict;
                }

                // Background Color picker (Default/Blue/Green/Red)
                if (e.PropertyName == nameof(MainViewModel.SelectedBackgroundColor))
                {
                    var name = (vm.SelectedBackgroundColor ?? "Default").ToLowerInvariant();
                    // Remove override to use theme default
                    if (name == "default")
                    {
                        if (Application.Current.Resources.Contains("Background"))
                            Application.Current.Resources.Remove("Background");
                    }
                    else
                    {
                        Color c = name switch
                        {
                            "blue" => Colors.SteelBlue,
                            "green" => Colors.SeaGreen,
                            "red" => Colors.IndianRed,
                            _ => Colors.Transparent // should not happen
                        };
                        Application.Current.Resources["Background"] = new SolidColorBrush(c);
                    }
                }
            };
        }
    }
}