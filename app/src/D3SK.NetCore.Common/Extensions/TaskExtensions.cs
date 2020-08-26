using System;
using System.Threading.Tasks;

namespace D3SK.NetCore.Common.Extensions
{
    public static class TaskExtensions
    {
        public static Task WithAction(this Task source, Action action)
        {
            action();
            return source;
        }

        public static Task<object> AsTask(this object source)
        {
            return Task.FromResult(source);
        }

        public static Task<T> AsTask<T>(this object source)
        {
            return Task.FromResult((T)source);
        }

        public static Task<T> AsTask<T>(this T source)
        {
            return Task.FromResult((T)source);
        }
    }
}
