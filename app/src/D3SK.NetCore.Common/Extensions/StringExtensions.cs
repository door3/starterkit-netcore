using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToPascalCase(this string source, string splitString = " ", string joinString = null)
        {
            if (source == null) return null;
            if (source.Length < 2) return source.ToUpper();

            var words = source.Split(splitString, StringSplitOptions.RemoveEmptyEntries);

            var result = new StringBuilder(source.Length);
            foreach (var word in words)
            {
                if (result.Length > 0) result.Append(joinString);
                result.Append($"{word.Substring(0, 1).ToUpper()}{word.Substring(1)}");
            }

            return result.ToString();
        }

        public static string ToPropertyCase(this string source)
        {
            return source.ToPascalCase(".", ".");
        }
    }
}
