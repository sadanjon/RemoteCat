using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RdpBridge
{
    public enum FrgVerifyCertFlags : uint
    {
        None = 0x00,

        // Redirected or Gateway certificate
        Legacy = 0x02,

        // Redirected RDP certificate
        Redirect = 0x10,

        // Gateway certificate
        Gateway = 0x20,

        // Certificate was changed mid-session
        Changed = 0x40,

        // Host mismatch detected
        Mismatch = 0x80, 
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate uint FrgOnVerifyCertificateFn(IntPtr x509CertBytes, UIntPtr x509CertBytesLength, IntPtr hostnameUtf8, ushort port, uint flags);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgOnContextCreatedFn(IntPtr frgContext);

    [StructLayout(LayoutKind.Sequential)]
    public struct FrgOnAuthenticateResult
    {
        public IntPtr UsernameUtf8;
        public IntPtr PasswordUtf8;
        public IntPtr DomainUtf8;
        public bool PasswordEntered;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate FrgOnAuthenticateResult FrgOnAuthenticateFn(IntPtr usernameUtf8, IntPtr passwordUtf8, IntPtr domainUtf8);

    [StructLayout(LayoutKind.Sequential)]
    public struct FrgEntryPoints
    {
        public IntPtr OnContextCreated;
        public IntPtr OnVerifyCertificate;
        public IntPtr OnAuthenticate;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class FrgMainOptions
    {
        public int Argc;
        public IntPtr Argv;
        public FrgEntryPoints EntryPoints;
    }

    public class FreeRdpGlueNative
    {
        private const string DllName = "FreeRDPGlue";

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "frgMain")]
        public static extern int Main(FrgMainOptions options);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "frgDisconnect")]
        public static extern void Disconnect(IntPtr frgContext);
    }
}
