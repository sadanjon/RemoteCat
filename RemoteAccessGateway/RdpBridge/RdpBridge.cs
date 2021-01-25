using Google.Protobuf;
using RdpBridge.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RdpBridge
{
    public class RdpBridgeStartOptions
    {
        public string SessionId { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
    }

    public class RdpBridge : IDisposable
    {
        private IntPtr _frgContext;
        private LocalChannel _channel;
        private Thread _inputThread;
        private Exception _exitError;
        private ObjectEvent<RdpVerifyCertificateResultMessage> _rdpVerifyCertificateResultMessage = new ObjectEvent<RdpVerifyCertificateResultMessage>();
        private ObjectEvent<RdpAuthenticateResultMessage> _rdpAuthenticateResultMessage = new ObjectEvent<RdpAuthenticateResultMessage>();
        private bool _running;

        public RdpBridge(string channelHostname, int channelPort)
        {
            _channel = new LocalChannel(channelHostname, channelPort);
        }

        public void Start(RdpBridgeStartOptions options)
        {
            _running = true;

            StartIngressThread();

            _channel.WriteDelimited(new SessionMessage
            {
                SessionStart = new SessionStartMessage
                {
                    SessionId = options.SessionId,
                }
            });

            FreeRdpGlue.Main(new MainOptions
            {
                Hostname = options.Host,
                Username = options.Username,
                Password = options.Password,
                Domain = options.Domain,
                EntryPoints = new EntryPoints
                {
                    OnContextCreated = OnContextCreated,
                    OnAuthenticate = OnAuthenticate,
                    OnVerifyCertificate = FrgOnVerifyCertificateFn,
                    UpdateCallbacks = new UpdateCallbacks
                    {

                    }
                }
            });

            _channel.WriteDelimited(new RdpMessage
            {
                SessionEnd = new RdpSessionEndMessage { },
            });

            _running = false;
            Console.WriteLine("YYY 1");
            _inputThread.Join();
            Console.WriteLine("YYY 2");
            _channel.Close();
            Console.WriteLine("YYY 3");

            if (_exitError != null)
            {
                throw _exitError;
            }
        }

        private void StartIngressThread()
        {
            _inputThread = new Thread(() =>
            {
                try
                {
                    while (_running)
                    {
                        if (!_channel.ReadDelimited(out RdpMessage rdpMessage))
                        {
                            _running = false;
                            continue;
                        }

                        Console.WriteLine($"XXX {rdpMessage.MessageCase}");
                        switch (rdpMessage.MessageCase)
                        {
                            case RdpMessage.MessageOneofCase.VerifyCertificateResult:
                                _rdpVerifyCertificateResultMessage.Set(rdpMessage.VerifyCertificateResult);
                                break;
                            case RdpMessage.MessageOneofCase.AuthenticateResult:
                                _rdpAuthenticateResultMessage.Set(rdpMessage.AuthenticateResult);
                                break;
                            case RdpMessage.MessageOneofCase.Disconnect:
                                FreeRdpGlue.Disconnect(_frgContext);
                                break;
                            case RdpMessage.MessageOneofCase.SessionEnd:
                                _running = false;
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    _exitError = e;

                    if (_frgContext != IntPtr.Zero)
                    {
                        FreeRdpGlue.Disconnect(_frgContext);
                    }
                }
            });
            _inputThread.Start();
        }

        private void OnContextCreated(IntPtr frgContext)
        {
            _frgContext = frgContext;

            if (_exitError != null)
            {
                FreeRdpGlue.Disconnect(_frgContext);
                return;
            }
        }

        private FrgOnAuthenticateResult OnAuthenticate(IntPtr usernameUtf8, IntPtr passwordUtf8, IntPtr domainUtf8)
        {
            _channel.WriteDelimited(new RdpMessage
            {
                Authenticate = new RdpAuthenticateMessage
                {
                    Username = MarshalHelper.ReadUtf8IntPtr(usernameUtf8),
                    Password = MarshalHelper.ReadUtf8IntPtr(passwordUtf8),
                    Domain = MarshalHelper.ReadUtf8IntPtr(domainUtf8),
                }
            });

            var result = _rdpAuthenticateResultMessage.Wait();

            return new FrgOnAuthenticateResult
            {
                UsernameUtf8 = result.Username != null ? MarshalHelper.AllocUtf8(result.Username) : usernameUtf8,
                PasswordUtf8 = result.Password != null ? MarshalHelper.AllocUtf8(result.Password) : passwordUtf8,
                DomainUtf8 = result.Domain != null ? MarshalHelper.AllocUtf8(result.Domain) : domainUtf8,
                PasswordEntered = result.PasswordEntered,
            };
        }

        private FrgVerifyCertResult FrgOnVerifyCertificateFn(IntPtr x509CertBytes, UIntPtr x509CertBytesLength, IntPtr hostnameUtf8, ushort port, uint flags)
        {
            var x509CertBytesArray = MarshalHelper.CopyAllocedByteArray(x509CertBytes, (int)x509CertBytesLength);

            _channel.WriteDelimited(new RdpMessage
            {
                VerifyCertificate = new RdpVerifyCertificateMessage
                {
                    X509CertBytes = ByteString.CopyFrom(x509CertBytesArray),
                    Flags = flags,
                    Hostname = MarshalHelper.ReadUtf8IntPtr(hostnameUtf8),
                    Port = port,
                }
            });

            var verifyResult = _rdpVerifyCertificateResultMessage.Wait();

            switch (verifyResult.Result)
            {
                case RdpVerifyCertificateResultMessage.Types.RdpVerifyCertificateResult.PermenantlyTrusted:
                    return FrgVerifyCertResult.PermenantlyTrusted;
                case RdpVerifyCertificateResultMessage.Types.RdpVerifyCertificateResult.TemporarilyTrusted:
                    return FrgVerifyCertResult.TemporarilyTrusted;
                case RdpVerifyCertificateResultMessage.Types.RdpVerifyCertificateResult.NotTrusted:
                default:
                    return FrgVerifyCertResult.NotTrusted;
            }
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _rdpVerifyCertificateResultMessage?.Dispose();
            _rdpAuthenticateResultMessage?.Dispose();
        }
    }
}
