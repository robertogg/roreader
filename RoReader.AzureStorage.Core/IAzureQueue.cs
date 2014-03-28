using Microsoft.WindowsAzure.Storage.Queue;

namespace RoReader.AzureStorage.Core
{
    public interface IAzureQueue
    {
        CloudQueueMessage GetMessage();
        void AddMessage(CloudQueueMessage message);
        void DeleteMessage(CloudQueueMessage message);
    }
}