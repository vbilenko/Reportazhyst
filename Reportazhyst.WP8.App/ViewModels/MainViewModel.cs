using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Reportazhyst.WP8.Common.Models;

namespace Reportazhyst.WP8.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            Categories = new ObservableCollection<Category>();
            IsProgressVisible = false;
        }

        public ObservableCollection<Category> Categories { get; private set; }

        private bool _isProgressVisible;
        public bool IsProgressVisible
        {
            get
            {
                return _isProgressVisible;
            }
            set
            {
                if (value != _isProgressVisible)
                {
                    _isProgressVisible = value;
                    NotifyPropertyChanged("isProgressVisible");
                }
            }
        }

        public bool IsCategoriesLoaded
        {
            get;
            private set;
        }

        public void LoadCategories()
        {
            Categories.Add(new Category { Name = "Останні новини", File = "latest.xml", Url = "feed/" });
            Categories.Add(new Category { Name = "Україна", File = "ukrajina.xml", Url = "category/ukrajina/feed/" });
            Categories.Add(new Category { Name = "Політика", File = "polityka.xml", Url = "category/polityka/feed/" });
            Categories.Add(new Category { Name = "Міжнародні новини", File = "mizhnarodni-novyny.xml", Url = "category/mizhnarodni-novyny/feed/" });
            Categories.Add(new Category { Name = "Місцеві новини", File = "mistsevi-novyny.xml", Url = "category/mistsevi-novyny/feed/" });
            Categories.Add(new Category { Name = "Шоу-біз", File = "shou-biz.xml", Url = "category/shou-biz/feed/" });
            Categories.Add(new Category { Name = "Спорт", File = "sport.xml", Url = "category/sport/feed/" });
            Categories.Add(new Category { Name = "Фото-відео", File = "foto-video.xml", Url = "category/foto-video/feed/" });

            IsCategoriesLoaded = true;
        }

        public void UpdateFeeds()
        {
            for (var i = 0; i < Categories.Count; i++ )
            {
                App.RssViewModel.SelectedCategory = i;
                App.RssViewModel.UpdateData();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}