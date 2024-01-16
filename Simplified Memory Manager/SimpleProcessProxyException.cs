using System;

namespace SimplifiedMemoryManager
{
    public class SimpleProcessProxyException : Exception
    {
        public SimpleProcessProxyException(string message) : base(message)
        {
        }

        public SimpleProcessProxyException(string message, Exception innerException) : base(message, innerException) //TODO: this should be a separate exception
        {
        }
    }
}
