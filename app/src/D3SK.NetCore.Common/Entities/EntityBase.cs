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
        public TKey Id { get; protected set; }

        protected EntityBase()
        {
        }

        protected EntityBase(TKey id)
        {
            Id = id;
        }
    }
}
