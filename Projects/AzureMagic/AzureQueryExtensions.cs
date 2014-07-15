using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;

namespace AzureMagic
{
    public static class AzureQueryExtensions
    {
        public static async Task<IEnumerable<TEntity>> ExecuteAsync<TEntity>(this IQueryable<TEntity> query)
        {
            var tableQuery = query.AsTableQuery();
            var items = new List<TEntity>();

            TableQuerySegment<TEntity> currentSegment = null;

            while (currentSegment == null || currentSegment.ContinuationToken != null)
            {
                var token = currentSegment != null ? currentSegment.ContinuationToken : null;
                currentSegment = await tableQuery.ExecuteSegmentedAsync(token);

                items.AddRange(currentSegment.Results);
            }

            return items;
        }
    }
}