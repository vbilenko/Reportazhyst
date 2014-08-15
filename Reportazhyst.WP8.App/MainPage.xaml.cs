using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using Reportazhyst.WP8.Common.Models;
using Reportazhyst.WP8.Helpers;

namespace Reportazhyst.WP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        ObservableCollection<Article> _latestData = new ObservableCollection<Article>();

        public MainPage()
        {
            InitializeComponent();

            DataContext = App.RssViewModel;
            CategoriesList.DataContext = App.MainViewModel;

            Loaded += MainPage_Loaded;
        }

        private static void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            App.RssViewModel.SelectedCategory = 0;
            if (!App.MainViewModel.IsCategoriesLoaded)
                App.MainViewModel.LoadCategories();

            Deployment.Current.Dispatcher.BeginInvoke(
                () => App.RssViewModel.LoadData());

            if (!App.MainViewModel.Categories[0].Updated)
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

        private void CategoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoriesList.SelectedIndex <= -1)
                return;
            NavigationService.Navigate(new Uri("/CategoryPage.xaml?SelectedIndex=" + CategoriesList.SelectedIndex, UriKind.Relative));
            CategoriesList.SelectedIndex = -1;
        }
        private void NewsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NewsList.SelectedIndex > -1)
            {
                NavigationService.Navigate(new Uri("/ArticlePage.xaml?CategoryIndex=0&ArticleIndex=" + NewsList.SelectedIndex, UriKind.Relative));
                NewsList.SelectedIndex = -1;
            }
        }
        #region AppBar events

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

        #endregion

        private void TitleInfo_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/InfoPage.xaml", UriKind.Relative));
        }

        private void CategoriesList_Loaded(object sender, RoutedEventArgs e)
        {
            ((ListBoxItem)CategoriesList.ItemContainerGenerator.ContainerFromIndex(0)).Visibility = Visibility.Collapsed;
        }
    }
}