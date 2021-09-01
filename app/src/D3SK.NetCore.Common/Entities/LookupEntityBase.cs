using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public static T FromValue<T>(int value)
            where T : LookupEntityBase
        {
            return Parse<T, int>(value, "value", item => item.Id == value);
        }


        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate)
            where T : LookupEntityBase
        {
            var matchingItem = TryParse(predicate);

            if (matchingItem == null)
            {
                var message = $"'{value}' is not a valid {description} in {typeof(T)}";

                throw new InvalidOperationException(message);
            }

            return matchingItem;
        }

        private static T TryParse<T>(Func<T, bool> predicate)
            where T : LookupEntityBase
        {
            return GetAll<T>().FirstOrDefault(predicate);
        }

        public static IEnumerable<T> GetAll<T>()
            where T : LookupEntityBase
        {
            var type = typeof(T);
            var fields = type.GetTypeInfo().GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                var locatedValue = info.GetValue(null) as T;

                if (locatedValue != null)
                {
                    yield return locatedValue;
                }
            }
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
