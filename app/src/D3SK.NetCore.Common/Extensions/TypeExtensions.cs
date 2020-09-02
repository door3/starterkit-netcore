using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Utilities;

namespace D3SK.NetCore.Common.Extensions
{
    public static class TypeExtensions
    {
        public static object AsObject<T>(this T source)
        {
            return (object) source;
        }

        public static bool IsDefault<T>(this T source, params object[] otherDefaults)
        {
            if (source == null && default(T) == null)
            {
                return true;
            }

            return source != null && (source.Equals(default(T)) || (otherDefaults?.Contains(source)).GetValueOrDefault());
        }
       
        public static T Default<T>(this T source, T defaultValue)
        {
            return source.IsDefault() ? defaultValue : source;
        }

        public static bool IsNull(this object source)
        {
            return source == null;
        }
        
        public static T NotNull<T>(this T source, string argumentName = null)
        {
            if (source == null) throw new ArgumentNullException(argumentName);
            return source;
        }

        public static T NotDefault<T>(this T source, string argumentName, params object[] otherDefaults)
        {
            if (source.IsDefault(otherDefaults))
                throw new ArgumentException($"{argumentName ?? "Argument"} should not be default value.");
            return source;
        }
        
        public static object TryGetValue(this PropertyInfo source, object obj)
        {
            return obj == null ? null : source.GetValue(obj);
        }

        public static TVal GetPropertyValue<T, TVal>(this T source, Func<T, TVal> predicate,
            TVal defaultValue = default)
        {
            return source == null ? defaultValue : predicate(source);
        }

        public static object GetPropertyValue(this object source, string propertyName)
        {
            source.NotNull(nameof(source));
            propertyName.NotNull(nameof(propertyName));

            return source.GetType().GetProperty(propertyName)?.GetValue(source);
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

        public static bool InheritsFrom(this Type source, Type baseType)
        {
            source.NotNull(nameof(source));
            baseType.NotNull(nameof(baseType));

            return source.GetBaseTypes().Contains(baseType);
        }

        public static bool InheritsFromAny(this Type source, params Type[] baseTypes)
        {
            source.NotNull(nameof(source));
            baseTypes.NotNull(nameof(baseTypes));

            return baseTypes.Any(baseType => source.GetBaseTypes().Contains(baseType));
        }

        public static bool ImplementsInterface<TInterface>(this Type source)
        {
            source.NotNull(nameof(source));

            return source.GetInterfaces().Contains(typeof(TInterface));
        }

        public static IEnumerable<Type> GetBaseTypes(this Type source, bool includeActualType = false)
        {
            if (includeActualType)
            {
                yield return source;
            }

            // is there any base type?
            if ((source == null) || (source.BaseType == null))
            {
                yield break;
            }

            // return all implemented or inherited interfaces
            foreach (var i in source.GetInterfaces())
            {
                yield return i;
            }

            // return all inherited types
            var currentBaseType = source.BaseType;
            while (currentBaseType != null)
            {
                yield return currentBaseType;
                currentBaseType = currentBaseType.BaseType;
            }
        }

        public static T CopyAs<T>(this object source) where T : class
        {
            return source == null ? null : JsonHelper.Deserialize<T>(JsonHelper.Serialize(source));
        }

        public static bool CrudeCompare(this object source, object compare)
        {
            var sourceJson = JsonHelper.Serialize(source);
            var compareJson = JsonHelper.Serialize(compare);

            return sourceJson == compareJson;
        }

        public static T Also<T>(this T source, Action<T> alsoAction)
        {
            alsoAction.NotNull(nameof(alsoAction))(source);
            return source;
        }

        public static async Task<T> AlsoAsync<T>(this T source, Func<T, Task> alsoAction)
        {
            await alsoAction.NotNull(nameof(alsoAction))(source);
            return source;
        }
    }
}