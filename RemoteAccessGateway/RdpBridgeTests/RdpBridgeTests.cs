using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RdpBridge;
using RdpBridge.Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace RdpBridgeTests
{
    [TestClass]
    public class RdpBridgeTests
    {
        private const string RdpBridgePathEnvVar = "RDP_BRIDGE_PATH";
        private const string FreeRdpLogLevelEnvVar = "WLOG_LEVEL";

        private const string RdpTargetHostname = "uvo18yjszrh5hp79zne.vm.cld.sr";
        private const string RdpTargetUsername = "Administrator";
        private const string RdpTargetPassword = "Qb79K43W2B";

        private string _rdpBridgePath;

        [TestInitialize]
        public void TestInitialize()
        {
            Environment.SetEnvironmentVariable(FreeRdpLogLevelEnvVar, "TRACE");
            _rdpBridgePath = Environment.GetEnvironmentVariable(RdpBridgePathEnvVar);
            var xxx = Environment.GetEnvironmentVariables();
            if (_rdpBridgePath == null)
            {
                throw new Exception($"Env var '{RdpBridgePathEnvVar}' required");
            }
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            Environment.SetEnvironmentVariable(FreeRdpLogLevelEnvVar, null);
        }

        private string RdpBridgeProgramOptionsToArguments(RdpBridge.ProgramOptions options)
        {
            var arguments = new List<string>();
            
            if (options.Host != null)
            {
                arguments.Add($"-host {options.Host}");
            }

            if (options.Username != null)
            {
                arguments.Add($"-username {options.Username}");
            }

            if (options.Password != null)
            {
                arguments.Add($"-password {options.Password}");
            }

            if (options.Domain != null)
            {
                arguments.Add($"-domain {options.Domain}");
            }

            if (options.ChannelHost != null)
            {
                arguments.Add($"-channel-host {options.ChannelHost}");
            }

            if (options.ChannelPort != null)
            {
                arguments.Add($"-channel-port {options.ChannelPort}");
            }

            if (options.SessionId != null)
            {
                arguments.Add($"-session-id {options.SessionId}");
            }

            return string.Join(" ", arguments);
        }

        public class SubProcessRunner<TMessage> : IDisposable where TMessage : IMessage, new()
        {
            private const int StopProcessWaitTimeoutMs = 5000;
            private const int ReadTimeoutMs = 5000;

            private string _filename;
            private string _arguments;
            private string _sessionId;
            private Thread _stdoutThread;
            private Thread _stderrThread;
            private Thread _ingressChannelThread;
            private ChannelServer _channelServer;
            private Process _process;
            private bool _running;

            public event Action<LocalChannel, TMessage> OnMessage;
            public event Action<string> OnStdOutputLine;
            public event Action<string> OnStdErrorLine;
            public event Action<string, Exception> OnError;

            public SubProcessRunner(string filename, string arguments, string channelSessionId, ChannelServer channelServer)
            {
                _filename = filename;
                _arguments = arguments;
                _sessionId = channelSessionId;
                _channelServer = channelServer;
                _stdoutThread = new Thread(StdoutThreadProc);
                _stderrThread = new Thread(StderrThreadProc);
                _ingressChannelThread = new Thread(IngressChannelThreadProc);
            }

            public void Start()
            {
                try
                {
                    _running = true;
                    var startInfo = new ProcessStartInfo()
                    {
                        FileName = _filename,
                        Arguments = _arguments,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                    };

                    _process = Process.Start(startInfo);
                    _ingressChannelThread.Start();
                    _stdoutThread.Start();
                    _stderrThread.Start();

                    _process.WaitForExit();
                }
                catch (Exception e)
                {
                    OnError?.Invoke($"Error on process run", e);
                }
                finally
                {
                    _running = false;
                    _ingressChannelThread?.Join();
                    _stdoutThread?.Join();
                    _stderrThread?.Join();
                }
            }

            public void Stop()
            {
                _running = false;

                var stopTimeoutThread = new Thread(() =>
                {
                    Thread.Sleep(StopProcessWaitTimeoutMs);

                    if (_process != null && !_process.HasExited)
                    {
                        _process.Close();
                        _process.Kill(entireProcessTree: true);
                    }
                });
            }

            public void Dispose()
            {
                _process?.Dispose();
            }

            private void IngressChannelThreadProc()
            {
                try
                {
                    using (var channel = _channelServer.WaitForSession(_sessionId))
                    {
                        while (_running)
                        {
                            var readError = ReadChannelDelimited(channel, out TMessage message);
                            if (readError == ReadError.Success)
                            {
                                OnMessage?.Invoke(channel, message);
                            }
                            else if (readError == ReadError.Timeout)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    OnError?.Invoke($"Error when reading ingress channel message", e);
                    throw;
                }
            }

            private ReadError ReadChannelDelimited(LocalChannel channel, out TMessage message)
            {
                try
                {
                    if (!channel.ReadDelimited(out message))
                    {
                        return ReadError.Unknown;
                    }

                    return ReadError.Success;
                }
                catch (Exception ex)
                {
                    message = default;

                    if (ex is SocketException socketEx && socketEx.SocketErrorCode == SocketError.TimedOut)
                    {
                        return ReadError.Timeout;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            private void StdoutThreadProc()
            {
                var streamReader = _process?.StandardOutput;

                ReadStream("stdout", streamReader, (line) =>
                {
                    OnStdErrorLine?.Invoke(line);
                });
            }

            private void StderrThreadProc()
            {
                var streamReader = _process?.StandardError;

                ReadStream("stderr", streamReader, (line) =>
                {
                    OnStdErrorLine?.Invoke(line);
                });
            }

            private void ReadStream(string streamName, StreamReader streamReader, Action<string> onLine)
            {
                if (streamReader.BaseStream.CanTimeout)
                {
                    streamReader.BaseStream.ReadTimeout = ReadTimeoutMs;
                }

                try
                {
                    while (_running)
                    {
                        var line = streamReader.ReadLine();

                        if (line == null)
                        {
                            break;
                        }

                        onLine(line);
                    }
                }
                catch (Exception e)
                {
                    OnError?.Invoke($"Error when reading stream '{streamName}'", e);
                    throw;
                }
            }

            

            private enum ReadError
            {
                Success,
                Timeout,
                Unknown,
            }
        }


        [TestMethod]
        public void Xxx()
        {
            var sessionId = Guid.NewGuid().ToString();
            var channelPort = 15433;
            var running = true;
            var channelServer = new ChannelServer(channelPort);
            var arguments = RdpBridgeProgramOptionsToArguments(new ProgramOptions
            {
                Host = RdpTargetHostname,
                Username = RdpTargetUsername,
                Password = RdpTargetPassword,
                ChannelHost = "localhost",
                ChannelPort = channelPort,
                SessionId = sessionId,
            });

            var channelServerThread = new Thread(() => 
            { 
                channelServer.Listen();
            });

            channelServerThread.Start();

            using (var subProcessRunner = new SubProcessRunner<RdpMessage>("dotnet", arguments, sessionId, channelServer))
            {
                subProcessRunner.OnMessage += (channel, rdpMessage) =>
                {
                    Console.WriteLine($"YYY {rdpMessage.MessageCase}");
                    switch (rdpMessage.MessageCase)
                    {
                        case RdpMessage.MessageOneofCase.VerifyCertificate:
                            channel.WriteDelimited(new RdpMessage
                            {
                                VerifyCertificateResult = new RdpVerifyCertificateResultMessage
                                {
                                    Result = RdpVerifyCertificateResultMessage.Types.RdpVerifyCertificateResult.NotTrusted,
                                }
                            });
                            channel.WriteDelimited(new RdpMessage
                            {
                                Disconnect = new RdpDisconnectMessage { },
                            });
                            break;
                        case RdpMessage.MessageOneofCase.SessionEnd:
                            channel.WriteDelimited(new RdpMessage
                            {
                                SessionEnd = new RdpSessionEndMessage { },
                            });
                            return false;
                    }

                    return true;
                };

                subProcessRunner.Start();
            }

            var ingressThread = new Thread(() =>
            {
                using (var channel = channelServer.WaitForSession(sessionId))
                {
                    while (running)
                    {
                        if (!channel.ReadDelimited(out RdpMessage rdpMessage))
                        {
                            break;
                        }

                        Console.WriteLine($"YYY {rdpMessage.MessageCase}");
                        switch (rdpMessage.MessageCase)
                        {
                            case RdpMessage.MessageOneofCase.VerifyCertificate:
                                channel.WriteDelimited(new RdpMessage
                                {
                                    VerifyCertificateResult = new RdpVerifyCertificateResultMessage
                                    {
                                        Result = RdpVerifyCertificateResultMessage.Types.RdpVerifyCertificateResult.NotTrusted,
                                    }
                                });
                                channel.WriteDelimited(new RdpMessage
                                {
                                    Disconnect = new RdpDisconnectMessage { },
                                });
                                break;
                            case RdpMessage.MessageOneofCase.SessionEnd:
                                channel.WriteDelimited(new RdpMessage
                                {
                                    SessionEnd = new RdpSessionEndMessage { },
                                });
                                running = false;
                                break;
                        }
                    }
                    channel.Close();
                }
            });
            ingressThread.Start();

            var arguments = RdpBridgeProgramOptionsToArguments(new ProgramOptions
            {
                Host = RdpTargetHostname,
                Username = RdpTargetUsername,
                Password = RdpTargetPassword,
                ChannelHost = "localhost",
                ChannelPort = channelPort,
                SessionId = sessionId,
            });

            var startInfo = new ProcessStartInfo()
            {
                FileName = "dotnet",
                Arguments = $"{_rdpBridgePath} {arguments}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            Thread stdoutThread = null;
            Thread stderrThread = null;
            try
            {
                using (var process = Process.Start(startInfo))
                {
                    stdoutThread = new Thread(() =>
                    {
                        string line;
                        while ((line = process.StandardOutput.ReadLine()) != null)
                        {
                            Console.WriteLine($"OUT: {line}");
                        }
                    });

                    stderrThread = new Thread(() =>
                    {
                        string line;
                        while ((line = process.StandardError.ReadLine()) != null)
                        {
                            Console.WriteLine($"ERR: {line}");
                        }
                    });

                    stdoutThread.Start();
                    stderrThread.Start();

                    process.WaitForExit();

                    //Console.WriteLine("XXX stdout:");
                    //Console.Write(process.StandardOutput.ReadToEnd());

                    //Console.WriteLine("XXX stderr:");
                    //Console.Write(process.StandardError.ReadToEnd());
                }
            }
            finally
            {
                running = false;
                channelServer.Stop();
                stdoutThread?.Join();
                ingressThread.Join();
                channelServerThread.Join();
            }
        }

    }
}
