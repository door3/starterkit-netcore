using System;
using System.Collections.Generic;

namespace D3SK.NetCore.Common.Extensions
{
    public static class ExceptionExtensions
    {
        public static Exception GetInnermostException(this Exception source)
        {
            source.NotNull(nameof(source));

            while (source.InnerException != null)
            {
                source = source.InnerException;
            }

            return source;
        }

        public static object ToTempData(this Exception source)
        {
            source.NotNull(nameof(source));

            return new
            {
                Exception = new {
                    Type = source.GetType().Name, 
                    source.Message,
                    InnermostMessage = GetInnermostException(source).Message,
                    source.StackTrace
                }
            };
        }
    }
}
