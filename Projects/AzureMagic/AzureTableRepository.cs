using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AzureMagic.Exceptions;
using Microsoft.WindowsAzure.Storage.Table;
using NullGuard;

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

        public async Task AddEntity(TEntity entity)
        {
            // todo: unit tests
            try
            {
                var insert = TableOperation.Insert(entity);
                var result = await Table.ExecuteAsync(insert);

                if (result.HttpStatusCode == (int)HttpStatusCode.NoContent)
                {
                    return;
                }

                var message = string.Format("Expected result.HttpStatusCode to be {0} but found {1}.", HttpStatusCode.NoContent, (HttpStatusCode)result.HttpStatusCode);
                throw new AzureTableRepositoryException(message);
            }
            catch (Exception exception)
            {
                var message = string.Format("Cannot add {0}/{1}/{2}.", Table.Name, entity.PartitionKey, entity.RowKey);
                throw new AzureTableRepositoryException(message, exception);
            }
        }

        public Task DeleteEntityByPartitionKey(string partitionKey)
        {
            // todo: unit tests
            throw new NotImplementedException("DeleteEntityByPartitionKeyAsync");
        }
        
        public async Task<IEnumerable<TEntity>> FindAllEntities()
        {
            // todo: unit tests
            return await FindEntitiesWhere(null);
        }

        public async Task<IEnumerable<TEntity>> FindEntitiesByPartitionKey(string partitionKey)
        {
            // todo: unit tests
            return await FindEntitiesWhere(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
        }

        public async Task<IEnumerable<TEntity>> FindEntitiesWhere([AllowNull] string filterCondition)
        {
            // todo: unit tests
            var isFiltered = filterCondition != null;

            try
            {
                var query = new TableQuery<TEntity>();

                if (isFiltered)
                {
                    query = query.Where(filterCondition);
                }

                var items = new List<TEntity>();
                TableQuerySegment<TEntity> currentSegment = null;

                while (currentSegment == null || currentSegment.ContinuationToken != null)
                {
                    var token = currentSegment != null ? currentSegment.ContinuationToken : null;
                    currentSegment = await Table.ExecuteQuerySegmentedAsync(query, token);

                    items.AddRange(currentSegment.Results);
                }

                return items;
            }
            catch (Exception exception)
            {
                var message = isFiltered ?
                    string.Format("Cannot get entities from {0} where {1}.", Table.Name, filterCondition) :
                    string.Format("Cannot get entities from {0}.", Table.Name);

                throw new AzureTableRepositoryException(message, exception);
            }
        }

        public TableQuery<TEntity> Query()
        {
            // todo: unit tests
            return Table.CreateQuery<TEntity>();
        }
    }
}