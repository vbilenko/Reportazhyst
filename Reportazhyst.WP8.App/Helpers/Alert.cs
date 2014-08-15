using System.Windows;

namespace Reportazhyst.WP8.Helpers
{
    public static class Alert
    {
        public static void Show(string title, string message)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                            MessageBox.Show(message, title, MessageBoxButton.OK));
        }
    }
}
