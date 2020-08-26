using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Utilities;

namespace D3SK.NetCore.Common.Extensions
{
    public static class EntityExtensions
    {
        public static T CopyComposite<T>(this T source) where T : class, ICompositeEntity, new()
        {
            source.NotNull(nameof(source));

            var properties = source.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute<NotMappedAttribute>() == null &&
                            p.PropertyType.IsSimpleType() &&
                            p.GetSetMethod() != null)
                .ToList();

            var copy = new T();
            foreach (var prop in properties)
            {
                var val = prop.GetValue(source);
                prop.SetValue(copy, val);
            }

            return copy;
        }

        public static T CopyEntity<T>(this T source) where T : class, IEntityBase
        {
            if (source == null) return null;

            return JsonHelper.Deserialize<T>(JsonHelper.Serialize(source));
        }

        public static bool SameAs<T>(this ICollection<T> source, ICollection<T> other) where T : IManyToManyEntity
        {
            return source.All(s => other.Any(o => o.GetUniqueValues().SequenceEqual(s.GetUniqueValues())));
        }

        public static void SetId<T, TKey>(this T source, TKey id) where T : EntityBase<TKey>
        {
            typeof(EntityBase<TKey>).GetProperty("Id")?.SetValue(source, id, null);
        }

        public static int GetId<T>(this T source) where T : IEntity<int>
        {
            return source.GetId<T, int>();
        }

        public static TKey GetId<T, TKey>(this T source) where T : IEntity<TKey>
        {
            return source.Id;
        }
    }
}
