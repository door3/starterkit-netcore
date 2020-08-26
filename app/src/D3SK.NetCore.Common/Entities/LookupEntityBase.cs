using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Entities
{
    public abstract class LookupEntityBase : LookupEntityBase<int>, ILookupEntity
    {
        protected LookupEntityBase()
        {
        }

        protected LookupEntityBase(int id, string name)
            : base(id, name)
        {
        }
    }

    public abstract class LookupEntityBase<TKey> : EntityBase<TKey>, ILookupEntity<TKey> where TKey : IComparable
    {
        public string Name { get; set; }

        protected LookupEntityBase()
        {
        }

        protected LookupEntityBase(TKey id, string name)
            : base(id)
        {
            Name = name;
        }

        public static bool operator ==(LookupEntityBase<TKey> left, LookupEntityBase<TKey> right)
        {
            if (left is null && right is null)
            {
                return true;
            }

            return !(left is null) && left.Equals(right);
        }

        public static bool operator !=(LookupEntityBase<TKey> left, LookupEntityBase<TKey> right) => !(left == right);

        public int CompareTo(TKey other)
        {
            return Id.CompareTo(other);
        }

        public override string ToString() => Name;

        public override bool Equals(object obj)
        {
            if (!(obj is LookupEntityBase<TKey> other))
            {
                return false;
            }

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Id.Equals(other.Id);

            return typeMatches && valueMatches;
        }

        // ReSharper disable once NonReadonlyMemberInGetHashCode
        public override int GetHashCode() => Id.GetHashCode();
    }
}
