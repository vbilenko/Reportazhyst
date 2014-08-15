using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Reportazhyst.WP8.Helpers;

namespace Reportazhyst.WP8
{
    public partial class CategoryPage : PhoneApplicationPage
    {

        private string _selectedIndex;
        private int _categoryIndex;

        public CategoryPage()
        {
            InitializeComponent();

            App.RssViewModel.Data = null;
            DataContext = App.RssViewModel;

            Loaded += CategoryPage_Loaded;
        }

        void CategoryPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (NavigationContext.QueryString.TryGetValue("SelectedIndex", out _selectedIndex))
            {
                _categoryIndex = int.Parse(_selectedIndex);
                App.RssViewModel.SelectedCategory = _categoryIndex;
                CategoryName.Text = App.MainViewModel.Categories[_categoryIndex].Name;
            }

            Deployment.Current.Dispatcher.BeginInvoke(
                () => App.RssViewModel.LoadData());


            if (!App.MainViewModel.Categories[_categoryIndex].Updated)
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    if (!File.Exists(App.MainViewModel.Categories[0].File))
                        Alert.Show("Помилка мережі", "Немає доступу до мережі інтернет. Перевірте з'єднання.");
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(
                        () => App.RssViewModel.UpdateData());
                }
            }
        }

        private void NewsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NewsList.SelectedIndex > -1)
            {
                NavigationService.Navigate(new Uri("/ArticlePage.xaml?CategoryIndex=" + _categoryIndex + "&ArticleIndex=" + NewsList.SelectedIndex, UriKind.Relative));
                NewsList.SelectedIndex = -1;
            }
        }

        private void AppBtnUpdate_Click(object sender, EventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                if (!File.Exists(App.MainViewModel.Categories[0].File))
                    Alert.Show("Помилка мережі", "Немає доступу до мережі інтернет. Перевірте з'єднання.");
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(
                    () => App.RssViewModel.UpdateData());
            }
        }

        private void AppBtnInfo_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/InfoPage.xaml", UriKind.Relative));
        }

        private void TitleInfo_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/InfoPage.xaml", UriKind.Relative));
        }

    }
}