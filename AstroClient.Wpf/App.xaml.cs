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
            var win = new MainWindow();
            win.DataContext = vm;
            Current.MainWindow = win;
            win.Show();
            old.Close();
        }
    }

}
