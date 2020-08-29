using System;
using D3SK.NetCore.Common;

namespace D3SK.NetCore.Infrastructure.Domain
{
    public class DomainClock : IClock
    {
        private readonly DateTimeOffset? _initUtcNow = null;

        public DateTimeOffset UtcNow => _initUtcNow ?? DateTimeOffset.UtcNow;

        public DomainClock()
        {
        }

        public DomainClock(DateTimeOffset utcNow)
        {
            _initUtcNow = utcNow;
        }
    }
}
