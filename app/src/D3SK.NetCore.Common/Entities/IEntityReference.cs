namespace D3SK.NetCore.Common.Entities
{
    public interface IEntityReferenceBase
    {
        string Name { get; }

        object ExtendedData { get; }
    }

    public interface IEntityReference<out TKey> : IEntityReferenceBase
    {
        TKey Id { get; }
    }
}
