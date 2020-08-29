namespace D3SK.NetCore.Common.Entities
{
    public interface IEntityBase
    {
    }

    public interface IEntity : IEntity<int>
    {
    }

    public interface IEntity<out TKey> : IEntityBase
    {
        TKey Id { get; }
    }

    public interface IRootEntity : IEntity
    {
    }

    public interface IChildEntity : IEntity
    {
    }

    public interface ICompositeEntity : IEntityBase
    {
    }
}
