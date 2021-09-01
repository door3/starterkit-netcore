using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace D3SK.NetCore.Common.Extensions
{
    public static class StringExtensions
    {
        public static string QuoteEncode(this string source, bool isDoubleQuotes = true)
        {
            var quote = isDoubleQuotes ? "\"" : "'";
            var str = source?.Replace(quote, @"\\{quote}");
            return $"{quote}{str}{quote}";
        }

        public static string Strip(this string source, string valueWhenNull = null)
        {
            var str = source?.Trim();
            return string.IsNullOrWhiteSpace(str) ? valueWhenNull : str;
        }

        public static bool IsEmpty(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        public static bool IsNotEmpty(this string source)
        {
            return !source.IsEmpty();
        }

        public static bool IsSame(this string item, string value)
        {
            return item != null && item.Equals(value, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsAll(this string source, char value)
        {
            return source?.All(x => x == value) ?? false;
        }

        public static string AppendNotEmpty(this string source, string preText = null, string postText = null)
        {
            return source.IsNotEmpty() ? $"{preText}{source}{postText}" : null;
        }

        public static string AppendWord(this string source, string word, string separator = " ")
        {
            return word.IsEmpty() ? source : source.IsNotEmpty() ? $"{source}{separator}{word}" : word;
        }

        public static StringBuilder AppendWord(this StringBuilder source, string word, string separator = " ")
        {
            source = source.NotNull(nameof(source));
            return word.IsEmpty() ? source : source.Append(source.Length > 0 ? $"{separator}{word}" : word);
        }

        public static StringBuilder AppendComma(this StringBuilder source, string word)
        {
            return AppendWord(source, word, ", ");
        }

        public static string ToPascalCase(this string source, string splitString = " ", string joinString = null)
        {
            if (source == null) return null;
            if (source.Length < 2) return source.ToUpper(CultureInfo.InvariantCulture);

            var words = source.Split(splitString, StringSplitOptions.RemoveEmptyEntries);

            var result = new StringBuilder(source.Length);
            foreach (var word in words)
            {
                if (result.Length > 0) result.Append(joinString);
                result.Append($"{word.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture)}{word.Substring(1)}");
            }

            return result.ToString();
        }

        public static string ToPropertyCase(this string source)
        {
            return source.ToPascalCase(".", ".");
        }

        public static string LeftSubstring(this string source, int length, bool requireLength = false)
        {
            if (requireLength && (source.IsEmpty() || length >= source.Length))
            {
                throw new ArgumentException($"Source must be a string with at most {length - 1}.");
            }

            if (source.IsEmpty())
            {
                return null;
            }

            length = Math.Min(source.Length, length);

            return source.Substring(0, length);
        }

        public static string RightSubstring(this string source, int indexFromRight, bool requireLength = false)
        {
            if (requireLength && (source.IsEmpty() || source.Length - indexFromRight < 0))
            {
                throw new ArgumentException($"Source must be a string with length of at least {indexFromRight}.");
            }

            if (source.IsEmpty())
            {
                return null;
            }

            var substrIndex = Math.Max(source.Length - indexFromRight, 0);

            return source.Substring(substrIndex);
        }

        public static string SubstringUntil(this string source, bool ignoreCase, params string[] stopStrings)
        {
            foreach (var stopString in stopStrings)
            {
                var indexOf = source.IndexOf(stopString, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
                if (indexOf < 0) continue;
                return indexOf == 0 ? null : source.Substring(0, indexOf);
            }

            return source;
        }

        public static string SubstringUntil(this string source, params string[] stopStrings)
        {
            return source.SubstringUntil(true, stopStrings);
        }

        public static bool ToBool(this string source, string trueValue = "true", string falseValue = "false",
            bool defaultValue = false, bool useDefaultBoolValues = true)
        {
            var testTrueValues =
                useDefaultBoolValues ? new List<string>(new[] { "true", "T", "1", "yes", "Y" }) : new List<string>();
            var testFalseValues =
                useDefaultBoolValues ? new List<string>(new[] { "false", "F", "0", "no", "N" }) : new List<string>();

            testTrueValues.Add(trueValue);
            testFalseValues.Add(falseValue);

            return defaultValue
                ? testFalseValues.Any(x => x.Equals(source.Strip(), StringComparison.OrdinalIgnoreCase))
                : testTrueValues.Any(x => x.Equals(source.Strip(), StringComparison.OrdinalIgnoreCase));
        }

        public static bool? ToNullableBool(this string source, string trueValue = "true", string falseValue = "false",
            bool useDefaultBoolValues = true)
        {
            var testTrueValues =
                useDefaultBoolValues ? new List<string>(new[] { "true", "T", "1", "yes", "Y", "10" }) : new List<string>();
            var testFalseValues =
                useDefaultBoolValues ? new List<string>(new[] { "false", "F", "0", "no", "N" }) : new List<string>();

            testTrueValues.Add(trueValue);
            testFalseValues.Add(falseValue);

            if (testFalseValues.Any(x => x.Equals(source.Strip(), StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
            else if (testTrueValues.Any(x => x.Equals(source.Strip(), StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return null;
        }

        public static int ToInt(this string source, int defaultValue = default(int))
        {
            return int.TryParse(source, out var result) ? result : defaultValue;
        }

        public static int? ToNullableInt(this string source, params object[] otherDefaults)
        {
            if (source.IsEmpty() || (otherDefaults?.Contains(source)).GetValueOrDefault())
                return null;

            return int.TryParse(source, out var result) && !((otherDefaults?.Contains(result)).GetValueOrDefault())
                ? result
                : (int?)null;
        }

        public static DateTimeOffset? ToNullableDateTimeOffset(this string source)
        {
            if (source?.Replace("/", string.Empty).IsEmpty() ?? true)
                return null;

            return DateTimeOffset.TryParse(source, out var result) ? result : (DateTimeOffset?)null;
        }

        public static DateTimeOffset ToDateTimeOffset(this string source, DateTimeOffset defaultValue = default(DateTimeOffset))
        {
            return DateTimeOffset.TryParse(source, out var result) ? result : defaultValue;
        }

        public static decimal ToDecimal(this string source, decimal defaultValue = default(decimal))
        {
            return decimal.TryParse(source, out var result) ? result : defaultValue;
        }

        public static decimal? ToNullableDecimal(this string source)
        {
            if (source.IsEmpty())
                return null;

            return decimal.TryParse(source, out var result) ? result : (decimal?)null;
        }

        public static string ToTitleCase(this string source)
        {
            if (source.IsEmpty())
            {
                return source;
            }

            // Below check converts to Title Case according to the rules
            // “john, smith” becomes “John, Smith”
            // “DAVE, JONES” becomes “Dave, Jones”
            // “sTAcey, stONE” becomes “Stacey, Stone” (all mixed, first char NOT UPPER Case)
            // “McMann, John” stays “McMann, John” (because first char is UPPER Case)

            var needsCasing = char.IsLower(source[0]) || !source.Any(char.IsLower);

            return needsCasing ? $"{source.Substring(0, 1).ToUpper()}{source.Substring(1).ToLower()}" : source;
        }

        public static string WithoutSpecialCharacters(this string source)
        {
            if (source.IsEmpty())
            {
                return source;
            }

            return Regex.Replace(source, "[^0-9]", string.Empty);
        }

        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return Char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
            return str;
        }
    }
}
