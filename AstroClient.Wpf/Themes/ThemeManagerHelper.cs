using System;
using System.Windows;

namespace AstroClient.Wpf.Themes
{
    // Manages dynamic theme switching in XAML through an attached property.
    public static class ThemeManager
    {
        public static readonly DependencyProperty ThemeNameProperty =
            DependencyProperty.RegisterAttached(
                "ThemeName",
                typeof(string),
                typeof(ThemeManager),
                new PropertyMetadata("light", OnThemeNameChanged));

        public static string GetThemeName(DependencyObject obj)
        {
            return (string)obj.GetValue(ThemeNameProperty);
        }

        public static void SetThemeName(DependencyObject obj, string value)
        {
            obj.SetValue(ThemeNameProperty, value);
        }

        private static void OnThemeNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // This method is called whenever the ThemeName property changes on a UI element.
            if (d is FrameworkElement element)
            {
                var themeName = e.NewValue as string ?? "light";
                var dictionaries = element.Resources.MergedDictionaries;

                // To prevent resource conflicts, clear any previously loaded theme dictionary.
                if (dictionaries.Count > 0)
                {
                    dictionaries.RemoveAt(0);
                }

                try
                {
                    var dict = new ResourceDictionary
                    {
                        Source = new Uri($"Themes/{themeName}.xaml", UriKind.Relative)
                    };
                    // Add the new theme dictionary.
                    dictionaries.Insert(0, dict);
                }
                catch (Exception)
                {
                    // Fallback to a default theme if something goes wrong (like file not found error).
                    var fallbackDict = new ResourceDictionary
                    {
                        Source = new Uri("Themes/Light.xaml", UriKind.Relative)
                    };
                    dictionaries.Insert(0, fallbackDict);
                }
            }
        }
    }
}
