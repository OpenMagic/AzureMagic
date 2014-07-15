using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureMagic
{
    public static class AzureQueryExtensions
    {
        public static Task<IEnumerable<TEntity>> ExecuteAsync<TTableEntity, TEntity>(this IQueryable<TTableEntity> query) where TTableEntity : ITableEntity
        {
            throw new NotImplementedException();
        }
    }
}