using System.IO.IsolatedStorage;

namespace Reportazhyst.WP8.Common
{
    public static class Constants
    {
        public readonly static string TaskName = "ReportazhystAgent";
        public readonly static string TaskDescription = "Reportazhyst updater";
        public readonly static string ReportazhystUrl = "http://reportazhyst.com/";
        public static IsolatedStorageSettings Settings = IsolatedStorageSettings.ApplicationSettings;
        public static IsolatedStorageFile Storage = IsolatedStorageFile.GetUserStoreForApplication();

        public static string PreviewResizer = "http://reportazhyst.com/wp-content/themes/tribune/scripts/timthumb.php?w=100&h=100&zc=1&src=";
        public static string ImageResizer = "http://reportazhyst.com/wp-content/themes/tribune/scripts/timthumb.php?w=450&h=300&zc=1&src=";
    }
}
