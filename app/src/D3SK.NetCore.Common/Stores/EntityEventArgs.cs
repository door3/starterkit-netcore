using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Common.Stores
{
    public abstract class EntityEventArgsBase<T> where T : class, IEntityBase
    {
        public T Entity { get; }

        protected EntityEventArgsBase(T entity)
        {
            Entity = entity.NotNull(nameof(entity));
        }
    }

    public class EntityAddedEventArgs<T> : EntityEventArgsBase<T> where T : class, IEntityBase
    {
        public EntityAddedEventArgs(T entity) : base(entity)
        {
        }
    }

    public class EntityDeletedEventArgs<T> : EntityEventArgsBase<T> where T : class, IEntityBase
    {
        public EntityDeletedEventArgs(T entity) : base(entity)
        {
        }
    }

    public class EntityUpdatedEventArgs<T> : EntityEventArgsBase<T> where T : class, IEntityBase
    {
        public EntityUpdatedEventArgs(T entity) : base(entity)
        {
        }
    }

    public class EntityPropertyUpdatedEventArgs<T> : EntityEventArgsBase<T> where T : class, IEntityBase
    {
        public EntityPropertyUpdatedEventArgs(T entity) : base(entity)
        {
        }
    }
}
