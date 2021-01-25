using System;
using System.Collections.Generic;
using System.Text;

namespace RdpBridgeTests
{
    public class Program
    {
        public static void Main()
        {
            var tests = new RdpBridgeTests();
            tests.TestInitialize();
            tests.Xxx();
            tests.TestCleanUp();

        }
    }
}
