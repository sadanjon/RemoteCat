using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace RdpBridge
{
    public static class MarshalHelper
    {
        public static IntPtr AllocUtf8(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            var mem = Marshal.AllocHGlobal(bytes.Length + 1);
            Marshal.Copy(bytes, 0, mem, bytes.Length);
            Marshal.WriteByte(mem, bytes.Length, 0);
            return mem;
        }

        public static string ReadUtf8IntPtr(IntPtr utf8String)
        {
            return Marshal.PtrToStringUTF8(utf8String);
        }

        public static (IntPtr, UIntPtr) AllocCopyArray(IntPtr[] array, int start = 0, int? length = null)
        {
            var mem = Marshal.AllocHGlobal(array.Length * Marshal.SizeOf<IntPtr>());
            var _length = length ?? array.Length;
            Marshal.Copy(array, start, mem, _length);
            return (mem, (UIntPtr)_length);
        }

        public static (IntPtr, UIntPtr) AllocCopyArray(byte[] array, int start = 0, int? length = null)
        {
            var mem = Marshal.AllocHGlobal(array.Length * Marshal.SizeOf<byte>());
            var _length = length ?? array.Length;
            Marshal.Copy(array, start, mem, _length);
            return (mem, (UIntPtr)_length);
        }

        public static (IntPtr, UIntPtr) AllocCopyArray(char[] array, int start = 0, int? length = null)
        {
            var mem = Marshal.AllocHGlobal(array.Length * Marshal.SizeOf<char>());
            var _length = length ?? array.Length;
            Marshal.Copy(array, start, mem, _length);
            return (mem, (UIntPtr)_length);
        }

        public static (IntPtr, UIntPtr) AllocCopyArray(float[] array, int start = 0, int? length = null)
        {
            var mem = Marshal.AllocHGlobal(array.Length * Marshal.SizeOf<float>());
            var _length = length ?? array.Length;
            Marshal.Copy(array, start, mem, _length);
            return (mem, (UIntPtr)_length);
        }

        public static (IntPtr, UIntPtr) AllocCopyArray(double[] array, int start = 0, int? length = null)
        {
            var mem = Marshal.AllocHGlobal(array.Length * Marshal.SizeOf<double>());
            var _length = length ?? array.Length;
            Marshal.Copy(array, start, mem, _length);
            return (mem, (UIntPtr)_length);
        }

        public static (IntPtr, UIntPtr) AllocCopyArray(short[] array, int start = 0, int? length = null)
        {
            var mem = Marshal.AllocHGlobal(array.Length * Marshal.SizeOf<short>());
            var _length = length ?? array.Length;
            Marshal.Copy(array, start, mem, _length);
            return (mem, (UIntPtr)_length);
        }

        public static (IntPtr, UIntPtr) AllocCopyArray(int[] array, int start = 0, int? length = null)
        {
            var mem = Marshal.AllocHGlobal(array.Length * Marshal.SizeOf<int>());
            var _length = length ?? array.Length;
            Marshal.Copy(array, start, mem, _length);
            return (mem, (UIntPtr)_length);
        }

        public static (IntPtr, UIntPtr) AllocCopyArray(long[] array, int start = 0, int? length = null)
        {
            var mem = Marshal.AllocHGlobal(array.Length * Marshal.SizeOf<long>());
            var _length = length ?? array.Length;
            Marshal.Copy(array, start, mem, _length);
            return (mem, (UIntPtr)_length);
        }

        public static IntPtr[] CopyAllocedIntPtrArray(IntPtr source, int length, int start = 0)
        {
            var dest = new IntPtr[length];
            Marshal.Copy(source, dest, start, length);
            return dest;
        }

        public static byte[] CopyAllocedByteArray(IntPtr source, int length, int start = 0)
        {
            var dest = new byte[length];
            Marshal.Copy(source, dest, start, length);
            return dest;
        }

        public static char[] CopyAllocedCharArray(IntPtr source, int length, int start = 0)
        {
            var dest = new char[length];
            Marshal.Copy(source, dest, start, length);
            return dest;
        }

        public static short[] CopyAllocedShortArray(IntPtr source, int length, int start = 0)
        {
            var dest = new short[length];
            Marshal.Copy(source, dest, start, length);
            return dest;
        }

        public static float[] CopyAllocedFloatArray(IntPtr source, int length, int start = 0)
        {
            var dest = new float[length];
            Marshal.Copy(source, dest, start, length);
            return dest;
        }

        public static double[] CopyAllocedDoubleArray(IntPtr source, int length, int start = 0)
        {
            var dest = new double[length];
            Marshal.Copy(source, dest, start, length);
            return dest;
        }

        public static int[] CopyAllocedIntArray(IntPtr source, int length, int start = 0)
        {
            var dest = new int[length];
            Marshal.Copy(source, dest, start, length);
            return dest;
        }

        public static long[] CopyAllocedLongArray(IntPtr source, int length, int start = 0)
        {
            var dest = new long[length];
            Marshal.Copy(source, dest, start, length);
            return dest;
        }

        public static void Free(IntPtr str)
        {
            Marshal.FreeHGlobal(str);
        }
    }

}
