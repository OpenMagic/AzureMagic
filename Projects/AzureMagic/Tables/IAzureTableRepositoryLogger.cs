using Microsoft.WindowsAzure.Storage.Table;

namespace AzureMagic.Tables
{
    public interface IAzureTableRepositoryLogger
    {
        void OnConstructing(CloudTable table, bool createTableIfNotExists);
        void OnConstructed(CloudTable table, bool createTableIfNotExists);
        void OnAddingEntity(ITableEntity entity);
        void OnAddedEntity(ITableEntity entity, TableResult result);
        void OnAddingEntityAsync(ITableEntity entity);
        void OnAddedEntityAsync(ITableEntity entity, TableResult result);
        void OnDeletingEntityAsync(ITableEntity entity);
        void OnDeletedEntityAsync(ITableEntity entity, TableResult result);
        void OnUpdatingEntityAsync(ITableEntity entity);
        void OnUpdatedEntityAsync(ITableEntity entity, TableResult result);
        void OnQueryingEntity();
        void OnQueriedEntity(object result);
    }
}