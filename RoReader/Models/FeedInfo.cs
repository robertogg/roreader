using System.Collections.Generic;

namespace RoReader.Models
{
    public class FeedInfo
    {
        public string Title { get; set; }
        public IEnumerable<Post> PostInfo { get; set; }
    }
}