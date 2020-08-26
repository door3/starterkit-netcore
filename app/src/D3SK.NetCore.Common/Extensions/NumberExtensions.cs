using System;

namespace D3SK.NetCore.Common.Extensions
{
    public static class NumberExtensions
    {
        public static bool IsGreaterThanZero(this int num) => num > 0;

        public static bool IsGreaterThanZero(this int? num) => num.HasValue && num > 0m;

        public static bool IsGreaterThanZero(this decimal num) => num > 0m;

        public static bool IsGreaterThanZero(this decimal? num) => num.HasValue && num > 0m;

        public static int? ToNullableInt(this int source, bool isNegativeNull = false)
        {
            if (source == 0 || (isNegativeNull && source < 0)) return null;
            return source;
        }

        public static int Clamp(this int source, int minValue, int maxValue)
        {
            return Math.Max(Math.Min(source, maxValue), minValue);
        }

        public static int? Clamp(this int? source, int minValue, int maxValue)
        {
            return !source.HasValue ? null : (source < minValue || source > maxValue ? null : source);
        }
    }
}
