using System.Collections.Generic;

namespace RoReader.Models
{
    public class GroupInfo
    {
        public string Title { get; set; }
        public IEnumerable<FeedInfo> FeedInfo { get; set; }
    }
}