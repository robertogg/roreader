using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using RoReader.AzureStorage.Core;
using RoReader.Constants;

namespace RoReader.AzureStorageTable
{
    public class AzureQueue : IAzureQueue
    {
        private readonly CloudQueue _cloudQueue;
        public AzureQueue()
        {
            var cloudStorageAccount =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(Azure.StorageConnectionString));
            var cloudQueueClient= cloudStorageAccount.CreateCloudQueueClient();

            _cloudQueue= cloudQueueClient.GetQueueReference(CloudConfigurationManager.GetSetting(Azure.QueueName));
            _cloudQueue.CreateIfNotExists();
        }

        public CloudQueueMessage GetMessage()
        {
            return _cloudQueue.GetMessage();
        }

        public void AddMessage(CloudQueueMessage message)
        {
            _cloudQueue.AddMessage(message);
        }

        public void DeleteMessage(CloudQueueMessage message)
        {
            _cloudQueue.DeleteMessage(message);
        }
    }
}
