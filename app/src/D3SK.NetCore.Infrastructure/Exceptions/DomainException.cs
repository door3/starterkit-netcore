using System;

namespace D3SK.NetCore.Infrastructure.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string exception): base(exception)
        {
        }
    }
}
