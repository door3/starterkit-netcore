namespace D3SK.NetCore.Common.Entities
{
    public interface IReferenceCodeEntity : IReferenceCodeEntity<string>
    {
    }

    public interface IReferenceCodeEntity<TCodeType> : IEntityBase
    {
        TCodeType ReferenceCode { get; set; }
    }
}
