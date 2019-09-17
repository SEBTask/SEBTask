using System;

namespace Domain.Exceptions
{
    public class IntegrationFailedException : Exception
    {
        public IntegrationFailedException() : base() { }
        public IntegrationFailedException(string message) : base(message) { }
    }
}
