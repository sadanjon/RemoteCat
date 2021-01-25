using Microsoft.VisualStudio.TestTools.UnitTesting;
using RdpBridge;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;

namespace RdpBridgeTests
{
    [TestClass]
    public class FreeRdpGlueTests
    {
        private const string RdpTargetHostname = "uvo18yjszrh5hp79zne.vm.cld.sr";
        private const string RdpTargetUsername = "Administrator";
        private const string RdpTargetPassword = "Qb79K43W2B";

        private IntPtr _frgContext;

        [TestInitialize]
        public void TestInitialize()
        {
            Environment.SetEnvironmentVariable("WLOG_LEVEL", "TRACE");
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            Environment.SetEnvironmentVariable("WLOG_LEVEL", null);
        }

        [TestMethod]
        public void Main_print_certificate_and_disconnect()
        {
            FreeRdpGlue.Main(new MainOptions
            {
                Hostname = RdpTargetHostname,
                Username = RdpTargetUsername,
                Password = RdpTargetPassword,
                EntryPoints = new EntryPoints
                {
                    OnContextCreated = (IntPtr __frgContext) =>
                    {
                        _frgContext = __frgContext;
                    },
                    OnVerifyCertificate = (IntPtr x509CertBytes, UIntPtr x509CertBytesLength, IntPtr hostnameUtf8, ushort port, uint flags) =>
                    {
                        PrintCertificateData(x509CertBytes, x509CertBytesLength, hostnameUtf8, port, flags);
                        FreeRdpGlue.Disconnect(_frgContext);
                        return FrgVerifyCertResult.TemporarilyTrusted;
                    }
                }
            });
        }

        [TestMethod]
        public void Main_authenticate()
        {
            FreeRdpGlue.Main(new MainOptions
            {
                Hostname = RdpTargetHostname,
                Username = RdpTargetUsername,
                Password = null,
                EntryPoints = new EntryPoints
                {
                    OnContextCreated = (IntPtr frgContext) =>
                    {
                        _frgContext = frgContext;
                    },
                    OnAuthenticate = (IntPtr usernameUtf8, IntPtr passwordUtf8, IntPtr domainUtf8) => 
                    {
                        var username = MarshalHelper.ReadUtf8IntPtr(usernameUtf8);
                        var usernameEntered = false;
                        var password = MarshalHelper.ReadUtf8IntPtr(passwordUtf8);
                        var passwordEntered = false;
                        var domain = MarshalHelper.ReadUtf8IntPtr(domainUtf8);
                        var domainEntered = false;

                        if (username == null)
                        {
                            username = RdpTargetUsername;
                            usernameEntered = true;
                        }

                        if (password == null)
                        {
                            password = RdpTargetPassword;
                            passwordEntered = true;
                        }

                        if (domain == null)
                        {
                            domain = "";
                            domainEntered = true;
                        }

                        return new FrgOnAuthenticateResult
                        {
                            UsernameUtf8 = usernameEntered ? MarshalHelper.AllocUtf8(username) : usernameUtf8,
                            PasswordUtf8 = passwordEntered ? MarshalHelper.AllocUtf8(password) : passwordUtf8,
                            DomainUtf8 = domainEntered ? MarshalHelper.AllocUtf8(domain) : domainUtf8,
                            PasswordEntered = usernameEntered || passwordEntered || domainEntered,
                        };
                    },
                    OnVerifyCertificate = (IntPtr x509CertBytes, UIntPtr x509CertBytesLength, IntPtr hostnameUtf8, ushort port, uint flags) =>
                    {
                        FreeRdpGlue.Disconnect(_frgContext);
                        return FrgVerifyCertResult.TemporarilyTrusted;
                    }
                }
            });
        }

        [TestMethod]
        public void Main_calling_update_callbacks()
        {
            int counter = 0;

            FreeRdpGlue.Main(new MainOptions
            {
                Hostname = RdpTargetHostname,
                Username = RdpTargetUsername,
                Password = RdpTargetPassword,
                EntryPoints = new EntryPoints
                {
                    OnContextCreated = (IntPtr frgContext) =>
                    {
                        _frgContext = frgContext;
                    },
                    OnVerifyCertificate = (IntPtr x509CertBytes, UIntPtr x509CertBytesLength, IntPtr hostnameUtf8, ushort port, uint flags) =>
                    {
                        return FrgVerifyCertResult.TemporarilyTrusted;
                    },
                    UpdateCallbacks = new UpdateCallbacks
                    {
                        BeginPaint = (IntPtr frgContext) =>
                        {
                            Debug.WriteLine($"{counter}: Begin Paint");
                        },
                        EndPaint = (IntPtr frgContext) =>
                        {
                            Debug.WriteLine($"{counter}: End Paint");
                            counter += 1;

                            if (counter == 10)
                            {
                                FreeRdpGlue.Disconnect(frgContext);
                            }
                        },
                    },
                },
            });
        }

        private static void PrintCertificateData(IntPtr x509CertBytes, UIntPtr x509CertBytesLength, IntPtr hostnameUtf8, ushort port, uint flags)
        {
            var hostname = MarshalHelper.ReadUtf8IntPtr(hostnameUtf8);
            var x509CertBytesArray = new byte[(int) x509CertBytesLength];
            Marshal.Copy(x509CertBytes, x509CertBytesArray, 0, x509CertBytesArray.Length);
            var bytesString = string.Join(" ", x509CertBytesArray.Select(b => $"{b:X2}"));

            Console.WriteLine($"Hostname: {hostname}");
            Console.WriteLine($"Port: {port}");
            Console.WriteLine($"Certificate: {bytesString}");

            var flagStrings = new List<string>();
            var flagOptions = (FrgVerifyCertFlags)flags;
            
            if ((flagOptions & FrgVerifyCertFlags.Changed) != 0)
            {
                flagStrings.Add("Changed-Certificate");
            }

            if ((flagOptions & FrgVerifyCertFlags.Redirect) != 0)
            {
                flagStrings.Add("RDP-Redirect");
            }

            if ((flagOptions & FrgVerifyCertFlags.Gateway) != 0)
            {
                flagStrings.Add("RDP-Gateway");
            }

            if ((flagOptions & FrgVerifyCertFlags.Mismatch) != 0)
            {
                flagStrings.Add("Host-Mismatch");
            }

            Console.WriteLine($"Flags: {string.Join(",", flagStrings)}");
        }
    }
}
