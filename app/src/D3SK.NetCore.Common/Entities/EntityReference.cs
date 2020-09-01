using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Entities
{
    public class EntityReference : EntityReference<int>
    {
        public EntityReference(int id, string name, object extendedData = null) : base(id, name, extendedData)
        {
        }
    }

    public class EntityReference<TKey> : IEntityReference<TKey>
    {
        public TKey Id { get; }

        public string Name { get; }

        public object ExtendedData { get; }

        public EntityReference(TKey id, string name, object extendedData = null)
        {
            Id = id;
            Name = name;
            ExtendedData = extendedData;
        }
    }
}
