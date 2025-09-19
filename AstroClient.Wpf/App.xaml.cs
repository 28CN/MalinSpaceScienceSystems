using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows;

namespace AstroClient.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //  runtime switch: recreate MainWindow
        public static void SetCulture(string cultureName)
        {
            var ci = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            var old = Current.MainWindow;
            if (old == null)
            {
                var fresh = new MainWindow();
                Current.MainWindow = fresh;
                fresh.Show();
                return;
            }

            // preserve existing DataContext (ViewModel)
            var vm = old.DataContext;
            var oldState = old.WindowState;
            var win = new MainWindow();

            // copy bounds & state so window won't "jump"
            win.WindowStartupLocation = WindowStartupLocation.Manual;   // manual positioning
            win.Left = old.Left;
            win.Top = old.Top;
            win.Width = old.Width;
            win.Height = old.Height;
            win.WindowState = oldState; // keep Normal/Maximized

            win.DataContext = vm;
            Current.MainWindow = win;

            // if it was maximized, re-apply after Loaded to avoid sizing glitches
            if (oldState == WindowState.Maximized)
            {
                win.Loaded += (_, __) => win.WindowState = WindowState.Maximized;
            }

            win.Show();
            old.Close();

        }
    }

}
