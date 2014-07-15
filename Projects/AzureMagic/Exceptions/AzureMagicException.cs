using System;

namespace AzureMagic.Exceptions
{
    public class AzureMagicException : Exception
    {
        public AzureMagicException(string message)
            : base(message)
        {
        }

        public AzureMagicException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}