using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Reportazhyst.WP8
{
    public partial class ArticlePage : PhoneApplicationPage
    {
        private string _categoryIndex;
        private int _category;
        private string _articleIndex;
        private int _article;

        public ArticlePage()
        {
            InitializeComponent();

            Loaded += ArticlePage_Loaded;
        }

        void ArticlePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (NavigationContext.QueryString.TryGetValue("CategoryIndex", out _categoryIndex))
            {
                _category = int.Parse(_categoryIndex);
            }
            if (NavigationContext.QueryString.TryGetValue("ArticleIndex", out _articleIndex))
            {
                _article = int.Parse(_articleIndex);
            }
            if (_category >= 0 && _article >=0 )
            {
                DataContext = App.RssViewModel.ParseRssArticle(App.MainViewModel.Categories[_category].Feed.Items.ToList()[_article]);
            }
        }

        private void ReadMore_Click(object sender, RoutedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
            delegate
            {
                var browser = new WebBrowserTask
                {
                    Uri = App.MainViewModel.Categories[_category].Feed.Items.ToList()[_article].Links[0].Uri
                };
                browser.Show();
            });
        }

        private void TitleInfo_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/InfoPage.xaml", UriKind.Relative));
        }
        private void AppBtnInfo_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/InfoPage.xaml", UriKind.Relative));
        }

    }
}