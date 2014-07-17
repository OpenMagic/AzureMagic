using System;
using Humanizer;

namespace AzureMagic
{
    public class TableNameResolver : ITableNameResolver
    {
        public string GetTableName(Type tableType)
        {
            return tableType.Name.Pluralize();
        }
    }
}