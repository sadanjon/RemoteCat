using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Google.Protobuf;

namespace RdpBridge
{
    public class Program
    {
        public static void Main(string[] args)
        {

        }
    }

    public class RdpBridge
    {
        private Stream _ingress;
        private Stream _egress;

        public RdpBridge(Stream ingress, Stream egress)
        {
            _ingress = ingress;
            _egress = egress;
        }

        public void Start()
        {
            using (var input = new CodedInputStream(_egress, leaveOpen: true))
            {
                var message = Protocol.RdpMessageBase.Parser.ParseDelimitedFrom(_ingress);

                switch (message.MessageCase)
                {
                    case Protocol.RdpMessageBase.MessageOneofCase.Connect:
                    {
                        var connect = message.Connect;
                        FreeRdpGlue.Main(new MainOptions
                        {
                            Hostname = connect.Host,
                            Username = connect.Username,
                            Password = connect.Password,
                            Domain = connect.Domain,
                        });
                    }
                    break;
                }
            }

        }
    }
}
