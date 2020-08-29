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
        public bool IsModified { get; }

        public EntityUpdatedEventArgs(T entity, bool isModified) : base(entity)
        {
            IsModified = isModified;
        }
    }

    public class EntityPropertyUpdatedEventArgs<T> : EntityEventArgsBase<T> where T : class, IEntityBase
    {
        public string PropertyName { get; set; }

        public object OldValue { get; set; }

        public object NewValue { get; set; }

        public EntityPropertyUpdatedEventArgs(T entity, string propertyName, object newValue, object oldValue) :
            base(entity)
        {
            PropertyName = propertyName;
            NewValue = newValue;
            OldValue = oldValue;
        }
    }
}
