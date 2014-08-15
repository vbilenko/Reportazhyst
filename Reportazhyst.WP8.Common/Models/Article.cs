using System;

namespace Reportazhyst.WP8.Common.Models
{
    public class Article
    {
        public string Title { get; set; }
        public Uri ArticleUrl { get; set; }
        public DateTime Date { get; set; }
        private string _previewUrl;
        public string PreviewUrl
        {
            get { return _previewUrl; }
            set { _previewUrl = Constants.PreviewResizer + value; }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = Constants.ImageResizer + value; }
        }
        public string Content { get; set; }
        public string Author { get; set; }
    }
}
