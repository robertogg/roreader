using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace RoReader.AzureStorage.Core
{
    public interface IRepository<TEntity> where TEntity : ITableEntity
    {
        TEntity GetByPartitionKeyAndRowKey(string partitionKey,string rowKey);
        IEnumerable<TEntity> GetByPartitionKey(string partitionKey);
        IEnumerable<TEntity> GetAll();
        void InsertOrReplace(TEntity entity);
    }
}
