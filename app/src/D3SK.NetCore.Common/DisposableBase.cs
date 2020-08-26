using System;

namespace D3SK.NetCore.Common
{
    public abstract class DisposableBase : IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources                
            }

            // free native resources if there are any.
        }
    }
}