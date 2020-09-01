using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace D3SK.NetCore.Common.Entities
{
    public class EntityReferenceBase : CompositeEntityBase
    {
        public string Name { get; private set; }

        [NotMapped]
        public object ExtendedData { get; private set; }

        public EntityReferenceBase()
        {
        }

        public EntityReferenceBase(string name, object extendedData = null)
        {
            Name = name;
            ExtendedData = extendedData;
        }

        public override IEnumerable<object> GetUniqueValues()
        {
            yield return GetType();
            yield return Name;
        }
    }

    public class EntityReference : EntityReference<int>
    {
        public EntityReference()
        {
        }

        public EntityReference(int id, string name, object extendedData = null) : base(id, name, extendedData)
        {
        }
    }

    public class EntityReference<TKey> : EntityReferenceBase, IEntityReference<TKey>
    {
        public TKey Id { get; private set; }

        public EntityReference()
        {
        }

        public EntityReference(TKey id, string name, object extendedData = null) : base(name, extendedData)
        {
            Id = id;
        }

        public override IEnumerable<object> GetUniqueValues()
        {
            yield return GetType();
            yield return Id;
        }
    }
}
