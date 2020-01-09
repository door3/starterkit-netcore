using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Extensions
{
    public static class TypeExtensions
    {
        public static T NotNull<T>(this T source, string argumentName = null)
        {
            if (source == null) throw new ArgumentNullException(argumentName);
            return source;
        }
    }
}
