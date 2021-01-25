using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RdpBridge
{
    public class LogService
    {
        public static void Log(string str)
        {
            Debug.WriteLine(str);
        }

        public static void Log(string str, Exception ex)
        {
            Debug.WriteLine(str);
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
        }
    }
}

