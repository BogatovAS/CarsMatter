using CarsMatter.Infrastructure.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsMatter.Infrastructure.Storages
{
    public class AzureTable<T> : IAzureTable<T> where T : TableEntity, new()
    {
        private readonly CloudStorageAccount storageAccount;

        private readonly string tableName;

        public AzureTable(CloudStorageAccount storageAccount, string tableName)
        {
            this.storageAccount = storageAccount;
            this.tableName = tableName;
        }

        public async Task<List<T>> GetList()
        {
            TableQuery<T> query = new TableQuery<T>();

            List<T> results = new List<T>();
            TableContinuationToken continuationToken = null;

            CloudTable table = this.GetTable();

            do
            {
                TableQuerySegment<T> queryResults = await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                continuationToken = queryResults.ContinuationToken;

                results.AddRange(queryResults.Results);
            } while (continuationToken != null);

            return results;
        }

        public async Task<List<T>> GetList(string propertyName, string propertyValue)
        {
            TableQuery<T> query = new TableQuery<T>()
                .Where(TableQuery.GenerateFilterCondition(propertyName,
                    QueryComparisons.Equal, propertyValue));

            CloudTable table = this.GetTable();

            List<T> results = new List<T>();
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<T> queryResults = await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                continuationToken = queryResults.ContinuationToken;

                results.AddRange(queryResults.Results);
            } while (continuationToken != null);

            return results;
        }

        public async Task<T> GetItem(string partitionKey, string rowKey)
        {
            TableOperation operation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            CloudTable table = this.GetTable();

            TableResult result = await table.ExecuteAsync(operation);

            return (T)result.Result;
        }

        public async Task Insert(T item)
        {
            TableOperation operation = TableOperation.Insert(item);

            CloudTable table = this.GetTable();

            await table.ExecuteAsync(operation);
        }

        public async Task Update(T item)
        {
            TableOperation operation = TableOperation.InsertOrReplace(item);

            CloudTable table = this.GetTable();

            await table.ExecuteAsync(operation);
        }

        public async Task Delete(string partitionKey, string rowKey)
        {
            T item = await this.GetItem(partitionKey, rowKey);

            TableOperation operation = TableOperation.Delete(item);

            CloudTable table = this.GetTable();

            await table.ExecuteAsync(operation);
        }

        private CloudTable GetTable()
        {
            CloudTableClient tableClient = this.storageAccount.CreateCloudTableClient();

            CloudTable cloudTable = tableClient.GetTableReference(this.tableName);

            return cloudTable;
        }

        public async Task CreateTableIfNotExist()
        {
            CloudTable cloudTable = this.GetTable();

            await cloudTable.CreateIfNotExistsAsync();
        }
    }
}
