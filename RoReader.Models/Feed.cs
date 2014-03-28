using Microsoft.WindowsAzure.Storage.Table;

namespace RoReader.Models
{
    public class Feed : TableEntity
    {
        public Feed(string PK,string RK)
        {
            this.PartitionKey = PK;
            this.RowKey = RK;
        }

        public Feed()
        {
        }

        public string FeedGroup { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}