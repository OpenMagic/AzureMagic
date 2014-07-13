using System;

namespace AzureMagic.Exceptions
{
    public class AzureTableRepositoryException : AzureMagicException
    {
        public AzureTableRepositoryException(string message)
            : base(message)
        {
        }

        public AzureTableRepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}