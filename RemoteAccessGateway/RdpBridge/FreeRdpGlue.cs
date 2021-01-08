using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace RdpBridge
{
    public class EntryPoints
    {
        public FrgOnContextCreatedFn OnContextCreated;
        public FrgOnVerifyCertificateFn OnVerifyCertificate;
        public FrgOnAuthenticateFn OnAuthenticate;
    }

    public class MainOptions
    {
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Hostname { get; set; }
        public EntryPoints EntryPoints { get; set; }
    }

    public class FreeRdpGlue
    {
        public static int Main(MainOptions options)
        {
            using (var disposableOptions = new DisposableFrgMainOptions(options))
            {
                return FreeRdpGlueNative.Main(disposableOptions.Value);
            }
        }

        public static void Disconnect(IntPtr frgContext)
        {
            FreeRdpGlueNative.Disconnect(frgContext);
        }
    }

    class DisposableFrgMainOptions : DisposableNative<FrgMainOptions>
    {
        public DisposableFrgMainOptions(MainOptions options)
        {
            var argv = new List<string>();
            argv.Add("a.exe");

            if (options.Hostname != null)
            {
                argv.Add($"/v:{options.Hostname}");
            }

            if (options.Domain != null)
            {
                argv.Add($"/d:{options.Domain}");
            }

            if (options.Username != null)
            {
                argv.Add($"/u:{options.Username}");
            }

            if (options.Password != null)
            {
                argv.Add($"/p:{options.Password}");
            }

            var argvArray = argv.Select((term) => MarshalHelper.AllocUtf8(term)).ToArray();
            var (argvNative, argc) = MarshalHelper.AllocCopyArray(argvArray);

            Value = new FrgMainOptions
            {
                Argc = (int) argc,
                Argv = argvNative,
                EntryPoints = new FrgEntryPoints
                {
                    OnContextCreated = Marshal.GetFunctionPointerForDelegate(options.EntryPoints.OnContextCreated),
                    OnVerifyCertificate = Marshal.GetFunctionPointerForDelegate(options.EntryPoints.OnVerifyCertificate),
                    OnAuthenticate = Marshal.GetFunctionPointerForDelegate(options.EntryPoints.OnAuthenticate),
                },
            };
        }

        protected override void DisposeUnmanagedState()
        {
            var argvArray = MarshalHelper.CopyAllocedIntPtrArray(Value.Argv, Value.Argc);

            foreach (var arg in argvArray)
            {
                MarshalHelper.Free(arg);
            }

            MarshalHelper.Free(Value.Argv);

            Value = new FrgMainOptions();
        }
    }
}
