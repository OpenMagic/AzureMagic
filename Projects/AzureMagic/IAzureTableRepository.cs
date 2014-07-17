using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureMagic
{
    public interface IAzureTableRepository<TEntity> where TEntity : ITableEntity, new()
    {
        Task<TableResult> AddEntity(TEntity entity);
        Task<TableResult> DeleteEntity(TEntity entity);
        TableQuery<TEntity> Query();
    }
}