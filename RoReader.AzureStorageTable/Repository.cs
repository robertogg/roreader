using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using RoReader.AzureStorage.Core;
using RoReader.Constants;

namespace RoReader.AzureStorage
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : ITableEntity, new()
    {
        private readonly CloudTable _table;
        public Repository()
        {
            var tableName= typeof(TEntity).Name.ToLower();

            var cloudStorageAccount =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(Azure.StorageConnectionString));
            var cloudStorageClient= cloudStorageAccount.CreateCloudTableClient();
            _table = cloudStorageClient.GetTableReference(tableName);
            _table.CreateIfNotExists();
        }

        public TEntity GetByPartitionKeyAndRowKey(string partitionKey,string rowKey)
        {

            //Alternative Query

            //var partitionKeyCondition = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
            //var rowKeyCondition = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey);
            //var finalCondition = TableQuery.CombineFilters(partitionKeyCondition, TableOperators.And, rowKeyCondition);

            //var query = new TableQuery<TEntity>().Where(finalCondition);
            //var result = _table.ExecuteQuery(query).FirstOrDefault();


            TableOperation retrieveOperation = TableOperation.Retrieve<TEntity>(partitionKey, rowKey);
            var result = (TEntity)_table.Execute(retrieveOperation).Result;
            return result;
        }

        public IEnumerable<TEntity> GetByPartitionKey(string partitionKey)
        {
            var query = new TableQuery<TEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            var result = _table.ExecuteQuery(query);
            return result;
        }

        public IEnumerable<TEntity> GetAll()
        {
            var query = _table.ExecuteQuery(new TableQuery<TEntity>());
            return query;
        }

        public void InsertOrReplace(TEntity entity)
        {
            TableOperation tableOperation = TableOperation.InsertOrReplace(entity);
            _table.Execute(tableOperation);
        }
    }
}
