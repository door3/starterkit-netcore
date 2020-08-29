namespace D3SK.NetCore.Common.Entities
{
    public interface IConcurrencyEntity
    {
        byte[] RowVersion { get; }
    }
}
