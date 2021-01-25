using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using RdpBridge.Protocol;

namespace RdpBridge
{
    public class ProgramOptions
    {
        public string ChannelHost { get; set; }
        public int? ChannelPort { get; set; }
        public string SessionId { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public bool PrintHelp { get; set; }
    }

    public class ProgramArgumentsParseResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class Program
    {
        public static int Main(string[] args)
        {
            var result = 0;
            var parseResult = ParseArguments(args, out ProgramOptions options);

            if (!parseResult.Success)
            {
                if (parseResult.ErrorMessage != null)
                {
                    result = -1;
                    Console.Error.WriteLine($"ERROR: {parseResult.ErrorMessage}");
                }
                PrintHelp();
                return result;
            }

            using (var rdpBridge = new RdpBridge(options.ChannelHost, options.ChannelPort.Value))
            {
                rdpBridge.Start(new RdpBridgeStartOptions 
                { 
                    Host = options.Host,
                    Username = options.Username,
                    Password = options.Password,
                    Domain = options.Domain,
                    SessionId = options.SessionId,
                });
            }

            return result;
        }

        private static ProgramArgumentsParseResult ParseArguments(string[] args, out ProgramOptions options)
        {
            var result = new ProgramArgumentsParseResult();
            result.Success = true;

            options = new ProgramOptions();
            for (var argIndex = 0; argIndex < args.Length - 1; argIndex += 2)
            {
                var key = args[argIndex];
                var value = args[argIndex + 1];

                if (key.Equals("-host", StringComparison.OrdinalIgnoreCase))
                {
                    options.Host = value;
                }
                else if (key.Equals("-username", StringComparison.OrdinalIgnoreCase))
                {
                    options.Username = value;
                }
                else if (key.Equals("-password", StringComparison.OrdinalIgnoreCase))
                {
                    options.Password = value;
                }
                else if (key.Equals("-domain", StringComparison.OrdinalIgnoreCase))
                {
                    options.Domain = value;
                }
                else if (key.Equals("-channel-host", StringComparison.OrdinalIgnoreCase))
                {
                    options.ChannelHost = value;
                }
                else if (key.Equals("-channel-port", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(value, out int port))
                    {
                        options.ChannelPort = port;
                    }
                    else
                    {
                        result.Success = false;
                        result.ErrorMessage = $"-channel-port should be an integer number";
                        break;
                    }
                }
                else if (key.Equals("-session-id", StringComparison.OrdinalIgnoreCase))
                {
                    options.SessionId = value;
                }
                else if (key.Equals("-help", StringComparison.OrdinalIgnoreCase))
                {
                    options.PrintHelp = value.Equals("true");
                }
                else
                {
                    result.Success = false;
                    result.ErrorMessage = $"Unknown argument: {key}";
                    break;
                }
            }

            if (options.Host == null)
            {
                result.Success = false;
                result.ErrorMessage = "-host option is required";
            }

            if (options.ChannelHost == null)
            {
                result.Success = false;
                result.ErrorMessage = "-channel-host option is required";
            }

            if (options.ChannelPort == null)
            {
                result.Success = false;
                result.ErrorMessage = "-channel-port option is required";
            }

            if (options.SessionId == null)
            {
                result.Success = false;
                result.ErrorMessage = "-session-id option is required";
            }

            return result;
        }

        private static void PrintHelp()
        {
            Console.Error.WriteLine("Useage: <program> [options]");
            Console.Error.WriteLine("    -host             The host to connect to (required)");
            Console.Error.WriteLine("    -username         The username to use");
            Console.Error.WriteLine("    -password         The user password to use");
            Console.Error.WriteLine("    -domain           The user domain to use");
            Console.Error.WriteLine("    -ingress-handle   The ingress anonymous pipe handle (required)");
            Console.Error.WriteLine("    -egress-handle    The egress anonymous pipe handle (required)");
            Console.Error.WriteLine("    -help             Print this help");
        }
    }
}
