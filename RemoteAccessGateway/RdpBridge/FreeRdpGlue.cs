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
        public UpdateCallbacks UpdateCallbacks;
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
                    OnContextCreated    = GetDelegatePtr(options.EntryPoints.OnContextCreated),
                    OnVerifyCertificate = GetDelegatePtr(options.EntryPoints.OnVerifyCertificate),
                    OnAuthenticate      = GetDelegatePtr(options.EntryPoints.OnAuthenticate),
                    UpdateCallbacks = new FrgUpdateCallbacks
                    {
                        BeginPaint            = GetDelegatePtr(options.EntryPoints.UpdateCallbacks.BeginPaint),
                        EndPaint              = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.EndPaint),
                        DesktopResize         = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.DesktopResize),
                        Palette               = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.Palette),
                        SetBounds             = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.SetBounds),
                        SurfaceBits           = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.SurfaceBits),
                        SurfaceFrameMarker    = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.SurfaceFrameMarker),
                        BitmapUpdate          = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.BitmapUpdate),
                        DstBlt                = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.DstBlt),
                        PatBlt                = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.PatBlt),
                        ScrBlt                = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.ScrBlt),
                        OpaqueRect            = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.OpaqueRect),
                        MultiOpaqueRect       = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.MultiOpaqueRect),
                        LineTo                = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.LineTo),
                        Polyline              = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.Polyline),
                        MemBlt                = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.MemBlt),
                        Mem3Blt               = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.Mem3Blt),
                        GlyphIndex            = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.GlyphIndex),
                        FastIndex             = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.FastIndex),
                        FastGlyph             = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.FastGlyph),
                        CacheColorTable       = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.CacheColorTable),
                        CacheBrush            = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.CacheBrush),
                        CacheGlyph            = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.CacheGlyph),
                        CacheGlyphV2          = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.CacheGlyphV2),
                        CacheBitmap           = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.CacheBitmap),
                        CacheBitmapV2         = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.CacheBitmapV2),
                        CacheBitmapV3         = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.CacheBitmapV3),
                        CreateOffscreenBitmap = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.CreateOffscreenBitmap),
                        SwitchSurface         = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.SwitchSurface),
                        FrameMarker           = GetDelegatePtr(options.EntryPoints.UpdateCallbacks?.FrameMarker),
                    },
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

        private IntPtr GetDelegatePtr<T>(T @delegate) where T : class
        {
            return @delegate != null ? Marshal.GetFunctionPointerForDelegate(@delegate) : IntPtr.Zero;
        }
    }

    public class UpdateCallbacks
    {
        public FrgUpdateBeginPaintFn BeginPaint;
        public FrgUpdateEndPaintFn EndPaint;
        public FrgUpdateDesktopResizeFn DesktopResize;
        public FrgUpdatePaletteFn Palette;
        public FrgUpdateSetBoundsFn SetBounds;
        public FrgUpdateSurfaceBitsFn SurfaceBits;
        public FrgUpdateSurfaceFrameMarkerFn SurfaceFrameMarker;
        public FrgUpdateBitmapUpdateFn BitmapUpdate;
        public FrgUpdateDstBltFn DstBlt;
        public FrgUpdatePatBltFn PatBlt;
        public FrgUpdateScrBltFn ScrBlt;
        public FrgUpdateOpaqueRectFn OpaqueRect;
        public FrgUpdateMultiOpaqueRectFn MultiOpaqueRect;
        public FrgUpdateLineToFn LineTo;
        public FrgUpdatePolylineFn Polyline;
        public FrgUpdateMemBltFn MemBlt;
        public FrgUpdateMem3BltFn Mem3Blt;
        public FrgUpdateGlyphIndexFn GlyphIndex;
        public FrgUpdateFastIndexFn FastIndex;
        public FrgUpdateFastGlyphFn FastGlyph;
        public FrgUpdateCacheColorTableFn CacheColorTable;
        public FrgUpdateCacheBrushFn CacheBrush;
        public FrgUpdateCacheGlyphFn CacheGlyph;
        public FrgUpdateCacheGlyphV2Fn CacheGlyphV2;
        public FrgUpdateCacheBitmapFn CacheBitmap;
        public FrgUpdateCacheBitmapV2Fn CacheBitmapV2;
        public FrgUpdateCacheBitmapV3Fn CacheBitmapV3;
        public FrgUpdateCreateOffscreenBitmapFn CreateOffscreenBitmap;
        public FrgUpdateSwitchSurfaceFn SwitchSurface;
        public FrgUpdateFrameMarkerFn FrameMarker;
    }
}
