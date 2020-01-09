using System;
using System.Collections.Generic;
using System.Text;

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
