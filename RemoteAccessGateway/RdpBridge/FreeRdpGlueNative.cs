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

    public enum FrgVerifyCertResult : uint
    {
        NotTrusted = 0,
        PermenantlyTrusted = 1,
        TemporarilyTrusted = 2,
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate FrgVerifyCertResult FrgOnVerifyCertificateFn(IntPtr x509CertBytes, UIntPtr x509CertBytesLength, IntPtr hostnameUtf8, ushort port, uint flags);

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
        public FrgUpdateCallbacks UpdateCallbacks;
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

    [StructLayout(LayoutKind.Sequential)]
    public struct FrgUpdateCallbacks
    {
        public IntPtr BeginPaint;
        public IntPtr EndPaint;
        public IntPtr DesktopResize;
        public IntPtr Palette;
        public IntPtr SetBounds;
        public IntPtr SurfaceBits;
        public IntPtr SurfaceFrameMarker;
        public IntPtr BitmapUpdate;
        public IntPtr DstBlt;
        public IntPtr PatBlt;
        public IntPtr ScrBlt;
        public IntPtr OpaqueRect;
        public IntPtr MultiOpaqueRect;
        public IntPtr LineTo;
        public IntPtr Polyline;
        public IntPtr MemBlt;
        public IntPtr Mem3Blt;
        public IntPtr GlyphIndex;
        public IntPtr FastIndex;
        public IntPtr FastGlyph;
        public IntPtr CacheColorTable;
        public IntPtr CacheBrush;
        public IntPtr CacheGlyph;
        public IntPtr CacheGlyphV2;
        public IntPtr CacheBitmap;
        public IntPtr CacheBitmapV2;
        public IntPtr CacheBitmapV3;
        public IntPtr CreateOffscreenBitmap;
        public IntPtr SwitchSurface;
        public IntPtr FrameMarker;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateBeginPaintFn(IntPtr frgContext);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateEndPaintFn(IntPtr frgContext);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateDesktopResizeFn(IntPtr frgContext, uint width, uint height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdatePaletteFn(IntPtr frgContext, PaletteUpdate palette);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateSetBoundsFn(IntPtr frgContext, RdpBounds bounds);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateSurfaceBitsFn(IntPtr frgContext, SurfaceBitsCommand surfaceBitsCommand);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateSurfaceFrameMarkerFn(IntPtr frgContext, SurfaceFrameMarker surfaceFrameMarker);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateBitmapUpdateFn(IntPtr frgContext, BitmapUpdate bitmap);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateDstBltFn(IntPtr frgContext, DstBltOrder dstblt);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdatePatBltFn(IntPtr frgContext, PatBltOrder patblt);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateScrBltFn(IntPtr frgContext, ScrBltOrder scrblt);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateOpaqueRectFn(IntPtr frgContext, OpaqueRectOrder opaqueRect);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateMultiOpaqueRectFn(IntPtr frgContext, MultiOpaqueRectOrder multiOpaqueRect);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateLineToFn(IntPtr frgContext, LineToOrder lineTo);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdatePolylineFn(IntPtr frgContext, PolylineOrder polyline);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateMemBltFn(IntPtr frgContext, MemBltOrder memBlt);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateMem3BltFn(IntPtr frgContext, Mem3BltOrder memBlt);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateGlyphIndexFn(IntPtr frgContext, GlyphIndexOrder glyphIndex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateFastIndexFn(IntPtr frgContext, FastIndexOrder fastIndex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateFastGlyphFn(IntPtr frgContext, FastGlyphOrder fastGlyph);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateCacheColorTableFn(IntPtr frgContext, CacheColorTableOrder cacheColorTableOrder);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateCacheBrushFn(IntPtr frgContext, CacheBrushOrder cacheBrushOrder);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateCacheGlyphFn(IntPtr frgContext, CacheGlyphOrder cacheGlyphOrder);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateCacheGlyphV2Fn(IntPtr frgContext, CacheGlyphV2Order cacheGlyphV2Order);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateCacheBitmapFn(IntPtr frgContext, CacheBitmapOrder cacheBitmapOrder);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateCacheBitmapV2Fn(IntPtr frgContext, CacheBitmapV2Order cacheBitmapV2Order);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateCacheBitmapV3Fn(IntPtr frgContext, CacheBitmapV3Order cacheBitmapV3Order);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateCreateOffscreenBitmapFn(IntPtr frgContext, CreateOffscreenBitmapOrder createOffscreenBitmapOrder);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateSwitchSurfaceFn(IntPtr frgContext, SwitchSurfaceOrder switchSurfaceOrder);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FrgUpdateFrameMarkerFn(IntPtr frgContext,  FrameMarkerOrder frameMarkerOrder);

    [StructLayout(LayoutKind.Sequential)]
    public struct PaletteEntry
    {
        public byte Red;
        public byte Green;
        public byte Blue;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class PaletteUpdate
    {
        public uint Number;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public PaletteEntry[] Entries;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class RdpBounds
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class SurfaceBitsCommand
    {
        public uint CmdType;
        public uint DestLeft;
        public uint DestTop;
        public uint DestRight;
        public uint DestBottom;
        public TSBitmapDataEx bmp;
        public bool SkipCompression;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TSBitmapDataEx
    {
        public byte Bpp;
        public byte Flags;
        public ushort CodecID;
        public ushort Width;
        public ushort Height;
        public uint BitmapDataLength;
        public TSCompressedBitmapHeaderEx exBitmapDataHeader;
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
        public byte[] bitmapData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TSCompressedBitmapHeaderEx
    {
        public uint HighUniqueId;
        public uint LowUniqueId;
        public ulong TmMilliseconds;
        public ulong TmSeconds;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class SurfaceFrameMarker
    {
        public uint FrameAction;
        public uint FrameId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BitmapData
    {
        public uint DestLeft;
        public uint DestTop;
        public uint DestRight;
        public uint DestBottom;
        public uint Width;
        public uint Height;
        public uint BitsPerPixel;
        public uint Flags;
        public uint BitmapLength;
        public uint CbCompFirstRowSize;
        public uint CbCompMainBodySize;
        public uint CbScanWidth;
        public uint CbUncompressedSize;
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 8)]
        public byte[] BitmapDataStream;
        public bool compressed;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class BitmapUpdate
    {
        public uint Count;
        public uint Number;
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
        public BitmapData[] Rectangles;
        public bool SkipCompression;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class DstBltOrder
    {
        public int LeftRect;
        public int TopRect;
        public int Width;
        public int Height;
        public uint BRop;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RdpBrush
    {
        public uint X;
        public uint Y;
        public uint Bpp;
        public uint Style;
        public uint Hatch;
        public uint Index;
        [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]
        public byte[] Data;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] P8x8;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class PatBltOrder
    {
        public int LeftRect;
        public int TopRect;
        public int Width;
        public int Height;
        public uint BRop;
        public uint BackColor;
        public uint ForeColor;
        public RdpBrush Brush;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class ScrBltOrder
    {
        public int LeftRect;
        public int TopRect;
        public int Width;
        public int Height;
        public uint BRop;
        public int XSrc;
        public int YSrc;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class OpaqueRectOrder
    {
        public int LeftRect;
        public int TopRect;
        public int Width;
        public int Height;
        public uint Color;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DeltaRect
    {
        public int Left;
        public int Top;
        public int Width;
        public int Height;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class MultiOpaqueRectOrder 
    {
        public int LeftRect;
        public int TopRect;
        public int Width;
        public int Height;
        public uint Color;
        public uint NumRectangles;
        public uint CBData;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 45)]
        public DeltaRect[] Rectangles;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class LineToOrder
    {
        public uint BackMode;
        public int XStart;
        public int YStart;
        public int XEnd;
        public int YEnd;
        public uint BackColor;
        public uint BRop2;
        public uint PenStyle;
        public uint PenWidth;
        public uint PenColor;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DeltaPoint
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class PolylineOrder
    {
        public int xStart;
        public int yStart;
        public uint bRop2;
        public uint penColor;
        public uint numDeltaEntries;
        public uint cbData;
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
        public DeltaPoint[] points;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class RdpBitmap
    {
        public UIntPtr Size;
        public IntPtr New;
        public IntPtr Free;
        public IntPtr Paint;
        public IntPtr Decompress;
        public IntPtr SetSurface;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16 - 6)]
        private uint[] PaddingA;

        public uint Left;
        public uint Top;
        public uint Right;
        public uint Bottom;
        public uint Width;
        public uint Height;
        public uint Format;
        public uint Flags;
        public uint Length;
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 15)]
        public byte[] Data;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32 - 26)]
        private uint[] PaddingB;

        public bool Compressed;
        public bool Ephemeral;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64 - 34)]
        private uint[] PaddingC;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class MemBltOrder
    {
        public uint CacheId;
        public uint ColorIndex;
        public int LeftRect;
        public int TopRect;
        public int Width;
        public int Height;
        public uint BRop;
        public int XSrc;
        public int YSrc;
        public uint CacheIndex;
        public RdpBitmap Bitmap;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class Mem3BltOrder
    {
        public uint CacheId;
        public uint ColorIndex;
        public int LeftRect;
        public int TopRect;
        public int Width;
        public int Height;
        public uint BRop;
        public int XSrc;
        public int YSrc;
        public uint BackColor;
        public uint ForeColor;
        public RdpBrush Brush;
        public uint CacheIndex;
        public RdpBitmap Bitmap;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class GlyphIndexOrder
    {
        public uint CacheId;
        public uint FLAccel;
        public uint ULCharInc;
        public uint FOpRedundant;
        public uint BackColor;
        public uint ForeColor;
        public int BKLeft;
        public int BKTop;
        public int BKRight;
        public int BKBottom;
        public int OPLeft;
        public int OPTop;
        public int OPRight;
        public int OPBottom;
        public RdpBrush Brush;
        public int X;
        public int Y;
        public uint CBData;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] Data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class FastIndexOrder
    {
        public uint CacheId;
        public uint FLAccel;
        public uint ULCharInc;
        public uint BackColor;
        public uint ForeColor;
        public int BKLeft;
        public int BKTop;
        public int BKRight;
        public int BKBottom;
        public int OPLeft;
        public int OPTop;
        public int OPRight;
        public int OPBottom;
        public bool OpaqueRect;
        public int X;
        public int Y;
        public uint CBData;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] Data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GlyphData
    {
        public uint CacheIndex;
        public short X;
        public short Y;
        public uint CX;
        public uint CY;
        public uint CB;
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
        public byte[] AJ;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GlyphDataV2
    {
        public uint CacheIndex;
        public int X;
        public int Y;
        public uint CX;
        public uint CY;
        public uint CB;
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
        public byte[] AJ;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class FastGlyphOrder
    {
        public uint CacheId;
        public uint FLAccel;
        public uint ULCharInc;
        public uint BackColor;
        public uint ForeColor;
        public int BKLeft;
        public int BKTop;
        public int BKRight;
        public int BKBottom;
        public int OPLeft;
        public int OPTop;
        public int OPRight;
        public int OPBottom;
        public int X;
        public int Y;
        public uint CBData;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] Data;
        public GlyphDataV2 GlyphData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class CacheColorTableOrder
    {
        public uint cacheIndex;
        public uint numberColors;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public uint[] colorTable;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class CacheBrushOrder
    {
        public uint Index;
        public uint Bpp;
        public uint CX;
        public uint CY;
        public uint Style;
        public uint Length;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] Data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class CacheGlyphOrder
    {
        public uint CacheId;
        public uint CGlyphs;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public GlyphData[] GlyphData;
        public IntPtr UnicodeCharacters; // null terminated wchar array
    }

    [StructLayout(LayoutKind.Sequential)]
    public class CacheGlyphV2Order
    {
        public uint CacheId;
        public uint Flags;
        public uint CGlyphs;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public GlyphDataV2[] GlyphData;
        public IntPtr UnicodeCharacters; // null terminated wchar array
    }

    [StructLayout(LayoutKind.Sequential)]
    public class CacheBitmapOrder
    {
        public uint cacheId;
        public uint bitmapBpp;
        public uint bitmapWidth;
        public uint bitmapHeight;
        public uint bitmapLength;
        public uint cacheIndex;
        public bool compressed;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] BitmapComprHdr;
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
        public byte[] BitmapDataStream;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class CacheBitmapV2Order
    {
        public uint CacheId;
        public uint Flags;
        public uint Key1;
        public uint Key2;
        public uint BitmapBpp;
        public uint BitmapWidth;
        public uint BitmapHeight;
        public uint BitmapLength;
        public uint CacheIndex;
        public bool Compressed;
        public uint CBCompFirstRowSize;
        public uint CBCompMainBodySize;
        public uint CBScanWidth;
        public uint CBUncompressedSize;
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 7)]
        public byte[] BitmapDataStream;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BitmapDataEx
    {
        public uint Bpp;
        public uint CodecID;
        public uint Width;
        public uint Height;
        public uint Length;
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)]
        public byte[] Data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class CacheBitmapV3Order
    {
        public uint CacheId;
        public uint Bpp;
        public uint Flags;
        public uint CacheIndex;
        public uint Key1;
        public uint Key2;
        public BitmapDataEx BitmapData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OffscreenDeleteList
    {
        public uint SIndices;
        public uint CIndices;
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
        public ushort[] indices;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class CreateOffscreenBitmapOrder
    {
        public uint Id;
        public uint CX;
        public uint CY;
        public OffscreenDeleteList DeleteList;
    };

    [StructLayout(LayoutKind.Sequential)]
    public class SwitchSurfaceOrder
    {
        public uint BitmapId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class FrameMarkerOrder
    {
        public uint Action;
    }
}
