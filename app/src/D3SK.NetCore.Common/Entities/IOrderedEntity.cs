using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Entities
{
    public interface IOrderedEntity : IOrderedEntity<int>
    {
    }

    public interface IOrderedEntity<out TOrderKey> : IEntityBase where TOrderKey : IComparable
    {
        TOrderKey Order { get; }
    }
}
