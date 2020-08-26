using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace D3SK.NetCore.Common.Extensions
{
    public static class TypeExtensions
    {
        public static object GetPropertyValue(this object source, string propertyName)
        {
            source.NotNull(nameof(source));
            propertyName.NotNull(nameof(propertyName));

            return source.GetType().GetProperty(propertyName)?.GetValue(source);
        }

        public static bool ImplementsInterface<TInterface>(this Type source)
        {
            source.NotNull(nameof(source));

            return source.GetInterfaces().Contains(typeof(TInterface));
        }

        public static bool IsDefault<T>(this T source, params object[] otherDefaults)
        {
            if (source == null && default(T) == null)
            {
                return true;
            }

            return source != null && (source.Equals(default(T)) || (otherDefaults?.Contains(source)).GetValueOrDefault());
        }

        public static bool IsSimpleType(this Type source)
        {
            source.NotNull(nameof(source));

            return
                source.IsValueType ||
                source.IsPrimitive ||
                new[]
                {
                    typeof(string),
                    typeof(decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
                }.Contains(source) ||
                Convert.GetTypeCode(source) != TypeCode.Object;
        }

        public static T NotNull<T>(this T source, string argumentName = null)
        {
            if (source == null) throw new ArgumentNullException(argumentName);
            return source;
        }

        public static object TryGetValue(this PropertyInfo source, object obj)
        {
            return obj == null ? null : source.GetValue(obj);
        }
    }
}
