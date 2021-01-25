using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace RdpBridge
{
    public class AnonymousPipeChannel : IDisposable
    {
        public const int INITIAL_BUFFER_SIZE = 4092;
        public const int MESSAGE_SIZE_BYTES = 8;

        private PipeStream _ingress;
        private PipeStream _egress;
        private string _clientIngressHandle;
        private string _clientEgressHandle;
        private Action _disposeLocalCopyOfClientHandles;

        private object _egressLock = new object();
        private object _ingressLock = new object();
        private byte[] _egressBuffer = new byte[INITIAL_BUFFER_SIZE];

        public static AnonymousPipeChannel CreateServer()
        {
            var ingress = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.Inheritable);
            var egress = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);

            return new AnonymousPipeChannel
            {
                _ingress = ingress,
                _egress = egress,
                _clientEgressHandle = ingress.GetClientHandleAsString(),
                _clientIngressHandle = egress.GetClientHandleAsString(),
                _disposeLocalCopyOfClientHandles = () =>
                {
                    ingress.DisposeLocalCopyOfClientHandle();
                    egress.DisposeLocalCopyOfClientHandle();
                }
            };
        }

        public static AnonymousPipeChannel CreateClient(string ingressHandle, string egressHandle)
        {
            var ingress = new AnonymousPipeClientStream(PipeDirection.In, ingressHandle);
            var egress = new AnonymousPipeClientStream(PipeDirection.Out, egressHandle);

            return new AnonymousPipeChannel
            {
                _ingress = ingress,
                _egress = egress,
            };
        }

        private AnonymousPipeChannel() { }

        public string GetClientIngressHandle()
        {
            return _clientIngressHandle;
        }

        public string GetClientEgressHandle()
        {
            return _clientEgressHandle;
        }

        public void DisposeLocalCopyOfClientHandles()
        {
            _disposeLocalCopyOfClientHandles();
        }

        public void WriteDelimited(IMessage message)
        {
            lock (_egressLock)
            {
                var messageSize = message.CalculateSize();
                GrowBuffer(ref _egressBuffer, messageSize + MESSAGE_SIZE_BYTES);
                long messageAndDelimSize;
                using (var m = new MemoryStream(_egressBuffer, writable: true))
                {
                    message.WriteDelimitedTo(m);
                    messageAndDelimSize = m.Position;
                }
                _egress.Write(new ReadOnlySpan<byte>(_egressBuffer, 0, (int)messageAndDelimSize));
            }
        }

        public bool ReadDelimited<T>(out T message) where T : IMessage, new()
        {
            lock (_ingressLock)
            {
                message = new T();
                try
                {
                    message.MergeDelimitedFrom(_ingress);
                }
                catch
                {
                    return _ingress.IsConnected;
                }
            }
            return true;
        }

        public bool IsConnected()
        {
            return (_ingress?.IsConnected ?? false) && (_egress?.IsConnected ?? false);
        }

        public void Close()
        {
            _ingress?.Close();
            _egress?.Close();
        }

        private void GrowBuffer(ref byte[] buffer, int requiredSize)
        {
            var length = buffer.Length;
            while (length < requiredSize)
            {
                length <<= 2;
            }
            buffer = new byte[length];
        }

        public void Dispose()
        {
            _ingress?.Dispose();
            _egress?.Dispose();
        }
    }

    public class ChannelStreamClosedException : Exception { }
}
