using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureMagic
{
    public interface IAzureTableRepository<TEntity> where TEntity : ITableEntity, new()
    {
        Task<TableResult> AddEntityAsync(TEntity entity);
        Task<TableResult> DeleteEntityAsync(TEntity entity);
        TableQuery<TEntity> Query();
    }
}