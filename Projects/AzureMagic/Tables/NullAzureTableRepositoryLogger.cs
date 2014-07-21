using Microsoft.WindowsAzure.Storage.Table;
using NullGuard;

namespace AzureMagic.Tables
{
    [NullGuard(ValidationFlags.None)]
    public class NullAzureTableRepositoryLogger : IAzureTableRepositoryLogger
    {
        public void OnAddingEntity(ITableEntity entity)
        {
        }

        public void OnAddedEntity(ITableEntity entity, TableResult result)
        {
        }

        public void OnConstructing(CloudTable table, bool createTableIfNotExists)
        {
        }

        public void OnConstructed(CloudTable table, bool createTableIfNotExists)
        {
        }

        public void OnAddingEntityAsync(ITableEntity entity)
        {
        }

        public void OnAddedEntityAsync(ITableEntity entity, TableResult result)
        {
        }

        public void OnDeletingEntityAsync(ITableEntity entity)
        {
        }

        public void OnDeletedEntityAsync(ITableEntity entity, TableResult result)
        {
        }

        public void OnUpdatingEntityAsync(ITableEntity entity)
        {
        }

        public void OnUpdatedEntityAsync(ITableEntity entity, TableResult result)
        {
        }

        public void OnQueryingEntity()
        {
        }

        public void OnQueriedEntity(object result)
        {
        }
    }
}