using Microsoft.WindowsAzure.Storage.Table;

namespace RoReader.Models
{
    public class Post : TableEntity
    {
         public Post(string PK,string RK)
        {
            this.PartitionKey = PK;
            this.RowKey = RK;
        }

        public Post()
        {
        }

        public string FeedId { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }
    }
}
