using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using HtmlAgilityPack;
using Reportazhyst.WP8.Common;
using Reportazhyst.WP8.Common.Models;
using VB;
using VB.RequestManager;

namespace Reportazhyst.WP8.ViewModels
{
    public class RssViewModel : INotifyPropertyChanged
    {
        public RssViewModel()
        {
            SelectedCategory = 0;
            IsProgressVisible = false;
        }

        public Action<Exception> OnDataUpdateError;
        public Action<Exception> OnDataLoadError;

        private int _selectedCategory;
        public int SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                NotifyPropertyChanged("SelectedCategory");
            }
        }

        private ObservableCollection<Article> _data;
        public ObservableCollection<Article> Data
        {
            get { return _data; }
            set
            {
                _data = value;
                NotifyPropertyChanged("Data");
            }
        }

        private bool _isProgressVisible;
        public bool IsProgressVisible
        {
            get
            {
                return _isProgressVisible;
            }
            set
            {
                _isProgressVisible = value;
                NotifyPropertyChanged("IsProgressVisible");
            }
        }

        public Article ParseRssArticle(SyndicationItem item)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(item.Summary.Text);

            var article = new Article
            {
                Title = item.Title.Text,
                Date = item.PublishDate.DateTime,
                ArticleUrl = item.Links.First().Uri,
                ImageUrl = doc.DocumentNode.SelectSingleNode("//img").Attributes["src"].Value
                //Author = doc.DocumentNode.SelectNodes("//a").Last().InnerText
            };
            //doc.DocumentNode.SelectNodes("//a").Last().Remove();
            article.Content = doc.DocumentNode.InnerText;

            return article;
        }

        private ObservableCollection<Article> ParseRssFeed(SyndicationFeed feed)
        {
            var articles = new ObservableCollection<Article>();
            if (feed == null)
                return articles;
            foreach (var i in feed.Items)
            {
                articles.Add(ParseRssArticle(i));
            }

            return articles;
        }

        #region update data
        public void UpdateData()
        {
            var dataLoader = new RequestRestManager();
            dataLoader.OnError += ex => { };
            dataLoader.OnStart += () =>
            {
                IsProgressVisible = true;
                App.MainViewModel.Categories[SelectedCategory].Loaded = false;
                App.MainViewModel.Categories[SelectedCategory].Updated = false;
            };
            dataLoader.OnFinish += () =>
            {
                IsProgressVisible = false;
            };
            dataLoader.OnReady += response =>
            {
                App.MainViewModel.Categories[SelectedCategory].Updated = true;
                LoadData();
            };
            dataLoader.SaveTo = App.MainViewModel.Categories[SelectedCategory].File;
            dataLoader.Get(new Uri(Constants.ReportazhystUrl + App.MainViewModel.Categories[SelectedCategory].Url));
        }
        #endregion

        public void LoadData()
        {
            Data = ParseRssFeed(App.MainViewModel.Categories[SelectedCategory].Feed);
            if (App.MainViewModel.Categories[SelectedCategory].Loaded)
                return;
            Data = null;
            var file = new FileManager();
            file.OnReadReady += File_OnReadOpen;
            file.OnReadFileMissing += () =>
            {

            };
            file.Read(App.MainViewModel.Categories[SelectedCategory].File);
        }

        void File_OnReadOpen(StreamReader stream)
        {
            try
            {
                using (var xmlReader = XmlReader.Create(stream))
                {
                    App.MainViewModel.Categories[SelectedCategory].Feed = SyndicationFeed.Load(xmlReader);
                    Data = ParseRssFeed(App.MainViewModel.Categories[SelectedCategory].Feed);
                    App.MainViewModel.Categories[SelectedCategory].Loaded = true;
                }
            }
            catch (Exception ex)
            {
                File.Delete(App.MainViewModel.Categories[SelectedCategory].File);
                if (OnDataLoadError != null)
                    OnDataLoadError(ex);

            }
            finally
            {
                stream.Close();
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
