using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Entities
{
    public abstract class EntityBase : EntityBase<int>
    {
        protected EntityBase()
        {
        }

        protected EntityBase(int id) : base(id)
        {
        }
    }

    public abstract class EntityBase<TKey> : IEntity<TKey>
    {
        public TKey Id { get; private set; }

        protected EntityBase()
        {
        }

        protected EntityBase(TKey id)
        {
            Id = id;
        }
    }
}
