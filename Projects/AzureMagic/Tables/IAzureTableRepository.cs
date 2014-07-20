using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureMagic.Tables
{
    public interface IAzureTableRepository<TEntity> where TEntity : ITableEntity, new()
    {
        Task<TableResult> AddEntityAsync(TEntity entity);
        Task<TableResult> DeleteEntityAsync(TEntity entity);
        Task<TableResult> UpdateEntityAsync(TEntity entity, bool forceUpdate);
        TableQuery<TEntity> Query();
    }
}