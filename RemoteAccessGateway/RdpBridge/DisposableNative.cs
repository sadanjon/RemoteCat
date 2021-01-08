using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RdpBridge
{
    public abstract class DisposableNative<T> : IDisposable
    {
        public T Value { get; protected set; }
        private bool disposedValue;

        protected abstract void DisposeUnmanagedState();

        protected virtual void DisposeManagedState() { }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeManagedState();
                }

                DisposeUnmanagedState();
                disposedValue = true;
            }
        }

        ~DisposableNative()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
