using System;

namespace D3SK.NetCore.Common.Entities
{
    public interface IClientEntity : IClientEntity<Guid>
    {
    }

    public interface IClientEntity<out TClientKey> : IEntityBase
    {
        TClientKey ClientId { get; }
    }
}
