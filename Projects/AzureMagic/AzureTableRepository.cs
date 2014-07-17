using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using AzureMagic.Exceptions;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureMagic
{
    public class AzureTableRepository<TEntity> : IAzureTableRepository<TEntity> where TEntity : ITableEntity, new()
    {
        private readonly CloudTable Table;

        public AzureTableRepository(string connectionString, string tableName, bool createTableIfNotExists = true) :
            this(AzureStorage.GetTable(connectionString, tableName), createTableIfNotExists)
        {
        }

        public AzureTableRepository(CloudTableClient tableClient, string tableName, bool createTableIfNotExists = true) :
            this(tableClient.GetTableReference(tableName), createTableIfNotExists)
        {
        }

        public AzureTableRepository(CloudTable table, bool createTableIfNotExists = true)
        {
            // todo: unit tests
            Table = table;

            if (createTableIfNotExists)
            {
                Table.OnceOnlyCreateTableIfNotExists();
            }
        }

        public async Task<TableResult> AddEntityAsync(TEntity entity)
        {
            return await ExecuteOperationAsync("add", TableOperation.Insert(entity), entity);
        }

        public async Task<TableResult> DeleteEntityAsync(TEntity entity)
        {
            return await ExecuteOperationAsync("delete", TableOperation.Delete(entity), entity);
        }

        public TableQuery<TEntity> Query()
        {
            return Table.CreateQuery<TEntity>();
        }

        private async Task<TableResult> ExecuteOperationAsync(string operationName, TableOperation operation, TEntity entity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entity.PartitionKey))
                {
                    throw new ValidationException("PartitionKey cannot be null or whitespace.");
                }

                if (entity.RowKey == null)
                {
                    throw new ValidationException("RowKey cannot be null.");
                }

                var result = await Table.ExecuteAsync(operation);

                if (result.HttpStatusCode == (int) HttpStatusCode.NoContent)
                {
                    return result;
                }

                var message = string.Format("Expected result.HttpStatusCode to be {0} but found {1}.", HttpStatusCode.NoContent, (HttpStatusCode) result.HttpStatusCode);
                throw new AzureTableRepositoryException(message);
            }
            catch (Exception exception)
            {
                var message = string.Format("Cannot {3} {0}/{1}/{2}.", Table.Name, entity.PartitionKey, entity.RowKey, operationName);
                throw new AzureTableRepositoryException(message, exception);
            }
        }
    }
}