using System;

namespace D3SK.NetCore.Common.Entities
{
    public interface ICodedLookupEntity : ICodedLookupEntity<int>
    {
    }

    public interface ICodedLookupEntity<TKey> : ILookupEntity<TKey> where TKey : IComparable
    {
        string Code { get; }
    }

    public interface ILookupEntity : ILookupEntity<int>
    {
    }

    public interface ILookupEntity<TKey> : IEntity<TKey>, IComparable<TKey> where TKey : IComparable
    {
        string Name { get; }
    }
}
