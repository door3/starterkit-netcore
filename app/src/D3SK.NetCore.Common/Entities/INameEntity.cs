namespace D3SK.NetCore.Common.Entities
{
    public interface INameEntity : ICompositeEntity
    {
        string Prefix { get; set; }

        string FirstName { get; set; }

        string MiddleName { get; set; }

        string LastName { get; set; }

        string Suffix { get; set; }
    }
}
