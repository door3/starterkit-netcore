using System.Collections.Generic;
using System.Linq;

namespace D3SK.NetCore.Common.Entities
{
    public abstract class ValueEntityBase : IEntityBase
    {
        public static bool operator ==(ValueEntityBase left, ValueEntityBase right)
        {
            if (left is null && right is null)
            {
                return true;
            }

            return !(left is null) && left.Equals(right);
        }

        public static bool operator !=(ValueEntityBase left, ValueEntityBase right) => !(left == right);

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueEntityBase)obj;
            var values = GetUniqueValues();
            var otherValues = other.GetUniqueValues();

            return values.SequenceEqual(otherValues);
        }

        public override int GetHashCode()
        {
            return GetUniqueValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        public abstract IEnumerable<object> GetUniqueValues();
    }
}
