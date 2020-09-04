namespace D3SK.NetCore.Common.Entities
{

    public interface IShortNameEntity : ICompositeEntity
    {
        string FirstName { get; }

        string LastName { get; }
    }
    
    public interface INameEntity : IShortNameEntity
    {
        string MiddleName { get; }
    }

    public interface IFullNameEntity : INameEntity
    {
        string FullName { get; }
    }

    public interface IExtendedNameEntity : INameEntity
    {
        string Prefix { get; }

        string Suffix { get; }
    }
}
