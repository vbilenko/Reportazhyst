using System.ServiceModel.Syndication;

namespace Reportazhyst.WP8.Common.Models
{
    public class Category
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string File { get; set; }
        public SyndicationFeed Feed { get; set; }
        public bool Loaded { get; set; }
        public bool Updated { get; set; }
    }
}
