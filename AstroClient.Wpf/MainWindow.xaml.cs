using AstroClient.Wpf.ViewModels;
using AstroClient.Wpf.Services;
using Astro.Contracts.Dtos;
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

                    if (name == "default")
                    {
                        if (Application.Current.Resources.Contains("Background"))
                            Application.Current.Resources.Remove("Background");
                        if (Application.Current.Resources.Contains("PanelBackground"))
                            Application.Current.Resources.Remove("PanelBackground");
                    }
                    else
                    {
                        (Color bg, Color panel) = name switch
                        {
                            "blue" => (Color.FromRgb(0xE9, 0xF2, 0xFF), Color.FromRgb(0xF7, 0xFB, 0xFF)),
                            "green" => (Color.FromRgb(0xEC, 0xF7, 0xEF), Color.FromRgb(0xF7, 0xFC, 0xF8)),
                            "red" => (Color.FromRgb(0xFF, 0xEF, 0xF0), Color.FromRgb(0xFF, 0xF8, 0xF8)),
                            _ => (Colors.Transparent, Colors.Transparent)
                        };

                        Application.Current.Resources["Background"] = new SolidColorBrush(bg);
                        Application.Current.Resources["PanelBackground"] = new SolidColorBrush(panel);
                    }
                }

                // Font family change
                if (e.PropertyName == nameof(MainViewModel.SelectedFontFamily))
                {
                    Application.Current.Resources["AppFontFamily"] = new FontFamily(vm.SelectedFontFamily ?? "Segoe UI");
                }

                // Font size change
                if (e.PropertyName == nameof(MainViewModel.SelectedFontSize))
                {
                    Application.Current.Resources["AppFontSize"] = vm.SelectedFontSize <= 0 ? 12.0 : vm.SelectedFontSize;
                }
            };
        }

        private void RootGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // move focus to grid so any TextBox loses focus
            RootGrid.Focus();
        }
    }
}