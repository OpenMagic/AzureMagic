using System;

namespace AzureMagic.Tests.Features.Steps
{
    public abstract class StepsBase : IDisposable
    {
        private bool IsDisposed;

        protected virtual void Cleanup()
        {
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code.
            // Put cleanup code in Dispose(bool disposing).
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases unmanaged and optionally managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                Cleanup();
            }

            IsDisposed = true;
        }

        protected string GetValue(string value)
        {
            switch (value)
            {
                case "null":
                    return null;

                case "empty":
                    return string.Empty;

                default:
                    return value;
            }
        }

        protected string GetTableName(string tableName)
        {
            if (tableName.Equals("unique", StringComparison.OrdinalIgnoreCase))
            {
                tableName = this.GetType().Name + DateTime.Now.Ticks;
            }

            return GetValue(tableName);
        }
    }
}