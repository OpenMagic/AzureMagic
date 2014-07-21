using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using AzureMagic.Exceptions;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureMagic.Tables
{
    public class AzureTableRepository<TEntity> : IAzureTableRepository<TEntity> where TEntity : ITableEntity, new()
    {
        private readonly CloudTable Table;
        private readonly IAzureTableRepositoryLogger Logger;

        public AzureTableRepository(string connectionString, string tableName, bool createTableIfNotExists = true) :
            this(connectionString, tableName, new NullAzureTableRepositoryLogger(), createTableIfNotExists)
        {
        }

        public AzureTableRepository(string connectionString, string tableName, IAzureTableRepositoryLogger logger, bool createTableIfNotExists = true) :
            this(AzureTableStorage.GetTable(connectionString, tableName), logger, createTableIfNotExists)
        {
        }

        public AzureTableRepository(CloudTable table, bool createTableIfNotExists = true) :
            this(table, new NullAzureTableRepositoryLogger(), createTableIfNotExists)
        {
        }

        public AzureTableRepository(CloudTable table, IAzureTableRepositoryLogger logger, bool createTableIfNotExists = true)
        {
            logger.OnConstructing(table, createTableIfNotExists);

            // todo: unit tests
            Table = table;
            Logger = logger;

            if (createTableIfNotExists)
            {
                Table.OnceOnlyCreateTableIfNotExists();
            }

            logger.OnConstructed(table, createTableIfNotExists);
        }

        public TableResult AddEntity(TEntity entity)
        {
            Logger.OnAddingEntity(entity);

            var result = ExecuteOperation("add", TableOperation.Insert(entity), entity);

            Logger.OnAddedEntity(entity, result);

            return result;
        }

        public async Task<TableResult> AddEntityAsync(TEntity entity)
        {
            Logger.OnAddingEntityAsync(entity);

            var result = await ExecuteOperationAsync("add", TableOperation.Insert(entity), entity);

            Logger.OnAddedEntityAsync(entity, result);

            return result;
        }

        public async Task<TableResult> DeleteEntityAsync(TEntity entity)
        {
            Logger.OnDeletingEntityAsync(entity);

            var result = await ExecuteOperationAsync("delete", TableOperation.Delete(entity), entity);

            Logger.OnDeletedEntityAsync(entity, result);

            return result;
        }

        public async Task<TableResult> UpdateEntityAsync(TEntity entity, bool forceUpdate = false)
        {
            // todo: unit tests

            Logger.OnUpdatingEntityAsync(entity);

            if (forceUpdate)
            {
                entity.ETag = "*";
            }

            if (string.IsNullOrWhiteSpace(entity.ETag))
            {
                throw new ValidationException("ETag cannot be null or whitespace.");
            }

            var result = await ExecuteOperationAsync("update", TableOperation.Replace(entity), entity);

            Logger.OnUpdatedEntityAsync(entity, result);

            return result;
        }

        public TableQuery<TEntity> Query()
        {
            Logger.OnQueryingEntity();
            
            var result = Table.CreateQuery<TEntity>();

            Logger.OnQueriedEntity(result);

            return result;
        }

        private AzureTableRepositoryException CreateExecuteOperationException(Exception exception, TEntity entity, string operationName)
        {
            var message = string.Format("Cannot {3} {0}/{1}/{2}.", Table.Name, entity.PartitionKey, entity.RowKey, operationName);

            return new AzureTableRepositoryException(message, exception);
        }

        private TableResult ExecuteOperation(string operationName, TableOperation operation, TEntity entity)
        {
            try
            {
                ValidateEntity(entity);

                var result = Table.Execute(operation);

                return ValidateResult(result);
            }
            catch (Exception exception)
            {
                throw CreateExecuteOperationException(exception, entity, operationName);
            }
        }

        private async Task<TableResult> ExecuteOperationAsync(string operationName, TableOperation operation, TEntity entity)
        {
            try
            {
                ValidateEntity(entity);

                var result = await Table.ExecuteAsync(operation);

                return ValidateResult(result);
            }
            catch (Exception exception)
            {
                throw CreateExecuteOperationException(exception, entity, operationName);
            }
        }

        private static void ValidateEntity(TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.PartitionKey))
            {
                throw new ValidationException("PartitionKey cannot be null or whitespace.");
            }

            if (entity.RowKey == null)
            {
                throw new ValidationException("RowKey cannot be null.");
            }
        }

        private static TableResult ValidateResult(TableResult result)
        {
            if (result.HttpStatusCode == (int) HttpStatusCode.NoContent)
            {
                return result;
            }

            var message = string.Format("Expected result.HttpStatusCode to be {0} but found {1}.", HttpStatusCode.NoContent, (HttpStatusCode) result.HttpStatusCode);
            throw new AzureTableRepositoryException(message);
        }
    }
}