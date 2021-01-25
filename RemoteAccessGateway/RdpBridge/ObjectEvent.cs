using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RdpBridge
{
    public class ObjectEvent<T> : IDisposable
    {
        private T _object;
        private AutoResetEvent _event = new AutoResetEvent(initialState: false);

        public void Set(T @object)
        {
            _object = @object;
            _event.Set();
        }

        public T Wait()
        {
            _event.WaitOne();
            var result = _object;
            _object = default;
            return result;
        }

        public void Dispose()
        {
            _event.Dispose();
        }
    }

}
