#pragma once

#include <freerdp/freerdp.h>

typedef void (*FrgUpdateBeginPaint)(void *frgContext);
typedef void (*FrgUpdateEndPaint)(void *frgContext);
typedef void (*FrgUpdateDesktopResize)(void *frgContext, UINT32 width, UINT32 height);
typedef void (*FrgUpdatePalette)(void *frgContext, const PALETTE_UPDATE *palette);
typedef void (*FrgUpdateSetBounds)(void *frgContext, const rdpBounds *bounds);
typedef void (*FrgUpdateSurfaceBits)(void *frgContext, const SURFACE_BITS_COMMAND *surfaceBitsCommand);
typedef void (*FrgUpdateSurfaceFrameMarker)(void *frgContext, const SURFACE_FRAME_MARKER *surfaceFrameMarker);
typedef void (*FrgUpdateBitmapUpdate)(void *frgContext, const BITMAP_UPDATE *bitmap);

typedef void (*FrgUpdateDstBltFn)(void *frgContext, const DSTBLT_ORDER *dstblt);
typedef void (*FrgUpdatePatBlt)(void *frgContext, PATBLT_ORDER *patblt);
typedef void (*FrgUpdateScrBlt)(void *frgContext, const SCRBLT_ORDER *scrblt);
typedef void (*FrgUpdateOpaqueRect)(void *frgContext, const OPAQUE_RECT_ORDER *opaque_rect);
typedef void (*FrgUpdateMultiOpaqueRect)(void *frgContext, const MULTI_OPAQUE_RECT_ORDER *multi_opaque_rect);
typedef void (*FrgUpdateLineTo)(void *frgContext, const LINE_TO_ORDER *line_to);
typedef void (*FrgUpdatePolyline)(void *frgContext, const POLYLINE_ORDER *polyline);
typedef void (*FrgUpdateMemBlt)(void *frgContext, MEMBLT_ORDER *memblt);
typedef void (*FrgUpdateMem3Blt)(void *frgContext, MEM3BLT_ORDER *memblt);
typedef void (*FrgUpdateGlyphIndex)(void *frgContext, GLYPH_INDEX_ORDER *glyph_index);
typedef void (*FrgUpdateFastIndex)(void *frgContext, const FAST_INDEX_ORDER *fast_index);
typedef void (*FrgUpdateFastGlyph)(void *frgContext, const FAST_GLYPH_ORDER *fast_glyph);

typedef void (*FrgUpdateCacheColorTable)(void *context, const CACHE_COLOR_TABLE_ORDER *cache_color_table_order);
typedef void (*FrgUpdateCacheBrush)(void *context, const CACHE_BRUSH_ORDER *cache_brush_order);
typedef void (*FrgUpdateCacheGlyph)(void *context, const CACHE_GLYPH_ORDER *cache_glyph_order);
typedef void (*FrgUpdateCacheGlyphV2)(void *context, const CACHE_GLYPH_V2_ORDER *cache_glyph_v2_order);
typedef void (*FrgUpdateCacheBitmap)(void *context, const CACHE_BITMAP_ORDER *cache_bitmap_order);
typedef void (*FrgUpdateCacheBitmapV2)(void *context, CACHE_BITMAP_V2_ORDER *cache_bitmap_v2_order);
typedef void (*FrgUpdateCacheBitmapV3)(void *context, CACHE_BITMAP_V3_ORDER *cache_bitmap_v3_order);

typedef void (*FrgUpdateCreateOffscreenBitmap)(void *context, const CREATE_OFFSCREEN_BITMAP_ORDER *create_offscreen_bitmap);
typedef void (*FrgUpdateSwitchSurface)(void *context, const SWITCH_SURFACE_ORDER *switch_surface);
typedef void (*FrgUpdateFrameMarker)(void *context, const FRAME_MARKER_ORDER *frame_marker);

typedef struct
{
    FrgUpdateBeginPaint beginPaint;
    FrgUpdateEndPaint endPaint;
    FrgUpdateDesktopResize desktopResize;
    FrgUpdatePalette palette;
    FrgUpdateSetBounds setBounds;
    FrgUpdateSurfaceBits surfaceBits;
    FrgUpdateSurfaceFrameMarker surfaceFrameMarker;
    FrgUpdateBitmapUpdate bitmapUpdate;

    FrgUpdateDstBltFn dstBlt;
    FrgUpdatePatBlt patBlt;
    FrgUpdateScrBlt scrBlt;
    FrgUpdateOpaqueRect opaqueRect;
    FrgUpdateMultiOpaqueRect multiOpaqueRect;
    FrgUpdateLineTo lineTo;
    FrgUpdatePolyline polyline;
    FrgUpdateMemBlt memBlt;
    FrgUpdateMem3Blt mem3Blt;
    FrgUpdateGlyphIndex glyphIndex;
    FrgUpdateFastIndex fastIndex;
    FrgUpdateFastGlyph fastGlyph;

    FrgUpdateCacheColorTable cacheColorTable;
    FrgUpdateCacheBrush cacheBrush;
    FrgUpdateCacheGlyph cacheGlyph;
    FrgUpdateCacheGlyphV2 cacheGlyphV2;
    FrgUpdateCacheBitmap cacheBitmap;
    FrgUpdateCacheBitmapV2 cacheBitmapV2;
    FrgUpdateCacheBitmapV3 cacheBitmapV3;

    FrgUpdateCreateOffscreenBitmap createOffscreenBitmap;
    FrgUpdateSwitchSurface switchSurface;
    FrgUpdateFrameMarker frameMarker;
} FrgUpdateCallbacks;