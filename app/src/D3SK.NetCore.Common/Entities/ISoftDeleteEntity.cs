namespace D3SK.NetCore.Common.Entities
{
    public interface ISoftDeleteEntity : IEntityBase
    {
        bool IsDeleted { get; }

        void SetDeleted(bool isDeleted = true);
    }
}
