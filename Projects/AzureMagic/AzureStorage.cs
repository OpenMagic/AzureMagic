using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Anotar.CommonLogging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

// todo: move to AzureMagic.

namespace AzureMagic
{
    public static class AzureStorage
    {
        public const string DevelopmentConnectionString = "UseDevelopmentStorage=true;";

        // todo: thread safe collection
        private static readonly HashSet<string> TableNames = new HashSet<string>();

        public static void DeleteTableIfExists(string connectionString, string tableName)
        {
            // todo: unit tests
            GetTable(connectionString, tableName).DeleteIfExists();
        }

        public static CloudTable GetTable(string connectionString, string tableName)
        {
            // todo: unit tests
            return GetTableClient(connectionString).GetTableReference(tableName);
        }

        public static CloudTableClient GetTableClient(string connectionString)
        {
            // todo: unit tests
            return GetAccount(connectionString).CreateCloudTableClient();
        }

        public static CloudStorageAccount GetAccount(string connectionString)
        {
            // todo: unit tests
            return CloudStorageAccount.Parse(connectionString);
        }

        public static bool OnceOnlyCreateTableIfNotExists(this CloudTable cloudTable)
        {
            // todo: unit tests
            if (TableNames.Contains(cloudTable.Name))
            {
                return false;
            }

            TableNames.Add(cloudTable.Name);

            return cloudTable.CreateIfNotExists();
        }

        public static Task<bool> OnceOnlyCreateTableIfNotExistsAsync(this CloudTable cloudTable)
        {
            // todo: unit tests
            if (TableNames.Contains(cloudTable.Name))
            {
                return Task.FromResult(false);
            }

            TableNames.Add(cloudTable.Name);

            return cloudTable.CreateIfNotExistsAsync();
        }

        /// <summary>
        ///     Converts a GUID to a partition key.
        /// </summary>
        /// <param name="value">The GUID to convert.</param>
        /// <remarks>
        ///     This method simply uses Guid.ToString(). The method's value is code readability.
        /// </remarks>
        public static string ToPartitionKey(this Guid value)
        {
            // todo: unit tests
            return value.ToString();
        }

        public static string ToRowKey(this DateTimeOffset value)
        {
            // todo: unit tests
            return value.ToString("O");
        }

        public static string ToRowKey(this int value)
        {
            // todo: unit tests
            // int.MaxValue is 2,147,483,647. Therefore pad <value> is with leading 0 to 10 characters.
            return value.ToString("D10");
        }
    }
}