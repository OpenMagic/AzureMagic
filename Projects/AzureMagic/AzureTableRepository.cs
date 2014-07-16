using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using AzureMagic.Exceptions;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureMagic
{
    public class AzureTableRepository<TEntity> where TEntity : ITableEntity, new()
    {
        private readonly CloudTable Table;

        public AzureTableRepository(string connectionString, string tableName, bool createTableIfNotExists = true)
        {
            // todo: unit tests
            Table = AzureStorage.GetTable(connectionString, tableName);

            if (createTableIfNotExists)
            {
                Table.OnceOnlyCreateTableIfNotExists();
            }
        }

        public async Task<TableResult> AddEntity(TEntity entity)
        {
            return await ExecuteOperation("add", TableOperation.Insert(entity), entity);
        }

        public async Task<TableResult> DeleteEntity(TEntity entity)
        {
            return await ExecuteOperation("delete", TableOperation.Delete(entity), entity);
        }

        private async Task<TableResult> ExecuteOperation(string operationName, TableOperation operation, TEntity entity)
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

                if (result.HttpStatusCode == (int)HttpStatusCode.NoContent)
                {
                    return result;
                }

                var message = string.Format("Expected result.HttpStatusCode to be {0} but found {1}.", HttpStatusCode.NoContent, (HttpStatusCode)result.HttpStatusCode);
                throw new AzureTableRepositoryException(message);
            }
            catch (Exception exception)
            {
                var message = string.Format("Cannot {3} {0}/{1}/{2}.", Table.Name, entity.PartitionKey, entity.RowKey, operationName);
                throw new AzureTableRepositoryException(message, exception);
            }
        }

        public TableQuery<TEntity> Query()
        {
            return Table.CreateQuery<TEntity>();
        }
    }
}