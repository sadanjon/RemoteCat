using Google.Protobuf;
using RdpBridge.Protocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RdpBridge
{
    public class ChannelServer : IDisposable
    {
        private int _port;
        private TcpListener _tcpListener;
        private bool _running = false;
        private AutoResetEvent _newChannelEvent = new AutoResetEvent(initialState: false);
        private ConcurrentDictionary<string, LocalChannel> _channelDictionary = new ConcurrentDictionary<string, LocalChannel>();

        public ChannelServer(int port)
        {
            _port = port;
        }

        public void Listen()
        {
            _running = true;
            _tcpListener = new TcpListener(IPAddress.Loopback, _port);
            _tcpListener.Start();

            while (_running)
            {
                TcpClient newClient = null;
                try
                {
                    newClient = _tcpListener.AcceptTcpClient();
                    var stream = newClient.GetStream();
                    var sessionMessage = new SessionMessage();
                    sessionMessage.MergeDelimitedFrom(stream);
                    var sessionId = sessionMessage.SessionStart.SessionId;
                    _channelDictionary.AddOrUpdate(sessionId, new LocalChannel(newClient), (k, v) => v);
                    _newChannelEvent.Set();
                }
                catch (Exception e)
                {
                    newClient?.Dispose();

                    if (_running)
                    {
                        LogService.Log("Error on accepting new channel session", e);
                        throw;
                    }
                }
            }
            LogService.Log("XXX: Channel Server exiting...");
        }

        public LocalChannel WaitForSession(string sessionId)
        {
            LocalChannel localChannel;
            while (!_channelDictionary.TryRemove(sessionId, out localChannel))
            {
                _newChannelEvent.WaitOne();
            }
            return localChannel;
        }

        public void Stop()
        {
            _tcpListener?.Stop();
            _running = false;
        }

        public void Dispose()
        {
            _tcpListener?.Stop();
            _running = false;
        }
    }

    public class LocalChannel : IDisposable
    {
        private const int ReadTimeoutMs = 5000;

        private TcpClient _tcpClient;
        private NetworkStream _stream;
        private object _ingressLock = new object();
        private object _egressLock = new object();

        public LocalChannel(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _stream = tcpClient.GetStream();
        }

        public LocalChannel(string address, int port)
        {
            _tcpClient = new TcpClient(AddressFamily.InterNetwork);
            _tcpClient.Connect(address, port);
            _stream = _tcpClient.GetStream();

            if (_stream.CanTimeout)
            {
                _stream.ReadTimeout = ReadTimeoutMs;
            }
        }

        public bool WriteDelimited(IMessage message)
        {
            lock (_egressLock)
            {
                try
                {
                    message.WriteDelimitedTo(_stream);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool ReadDelimited<T>(out T message) where T : IMessage, new()
        {
            lock (_ingressLock)
            {
                message = new T();
                try
                {
                    message.MergeDelimitedFrom(_stream);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public void Close()
        {
            _tcpClient.Close();
            _stream.Close();
        }

        public void Dispose()
        {
            _tcpClient.Dispose();
            _stream.Dispose();
        }
    }

    

}
