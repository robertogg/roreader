using System.Collections.Generic;

namespace RoReader.Models
{
    public class FeedViewModel
    {
        public Feed Feed { get; set; }
        public IEnumerable<GroupFeed> GroupFeeds { get; set; } 
    }
}