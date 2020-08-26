using System;

namespace D3SK.NetCore.Common
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
    }
}
