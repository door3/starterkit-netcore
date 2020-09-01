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

    public interface IRootEntity : IRootEntity<int>
    {
    }

    public interface IRootEntity<out TKey> : IEntity<TKey>
    {
    }

    public interface IChildEntity : IChildEntity<int>
    {
    }

    public interface IChildEntity<out TKey> : IEntity<TKey>
    {
    }

    public interface IRootChildEntity : IRootChildEntity<int>
    {
    }

    public interface IRootChildEntity<out TKey> : IRootEntity<TKey>, IChildEntity<TKey>
    {
    }

    public interface ICompositeEntity : IEntityBase
    {
    }
}
