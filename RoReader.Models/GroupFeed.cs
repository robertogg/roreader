using Microsoft.WindowsAzure.Storage.Table;

namespace RoReader.Models
{
    public class GroupFeed : TableEntity
    {
        public GroupFeed(string PK,string RK)
        {
            this.PartitionKey = PK;
            this.RowKey = RK;
        }

        public GroupFeed()
        {
        }

        public string Title { get; set; }
        public string Description { get; set; }
    }
}