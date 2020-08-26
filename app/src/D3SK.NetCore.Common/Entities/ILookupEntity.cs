﻿using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Entities
{
    public interface IOrderedLookupEntity : IOrderedLookupEntity<int>
    {
    }

    public interface IOrderedLookupEntity<TKey> : ILookupEntity<TKey> where TKey : IComparable
    {
        int Order { get; }
    }

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
