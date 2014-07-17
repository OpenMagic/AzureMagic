using System;

namespace AzureMagic
{
    public interface ITableNameResolver
    {
        string GetTableName(Type tableType);
    }
}