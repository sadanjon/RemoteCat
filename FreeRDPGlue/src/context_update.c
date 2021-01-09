#include "context_update.h"

#include "glue.h"

static
BOOL dstBlt(rdpContext *context, const DSTBLT_ORDER *dstblt)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->dstBlt, frgContext, dstblt);

	return TRUE;
}

static
BOOL patBlt(rdpContext *context, PATBLT_ORDER *patblt)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->patBlt, frgContext, patblt);

	return TRUE;
}

static
BOOL scrBlt(rdpContext *context, const SCRBLT_ORDER *scrblt)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;
	
	IFCALL(frgCallbacks->scrBlt, frgContext, scrblt);

	return TRUE;
}

static
BOOL opaqueRect(rdpContext *context, const OPAQUE_RECT_ORDER *opaque_rect)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->opaqueRect, frgContext, opaque_rect);

	return TRUE;
}

static
BOOL multiOpaqueRect(rdpContext *context, const MULTI_OPAQUE_RECT_ORDER *multi_opaque_rect)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->multiOpaqueRect, frgContext, multi_opaque_rect);

	return TRUE;
}

static
BOOL lineTo(rdpContext *context, const LINE_TO_ORDER *line_to)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->lineTo, frgContext, line_to);

	return TRUE;
}

static
BOOL polyline(rdpContext *context, const POLYLINE_ORDER *polyline)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->polyline, frgContext, polyline);

	return TRUE;
}

static
BOOL memBlt(rdpContext *context, MEMBLT_ORDER *memblt)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->memBlt, frgContext, memblt);

	return TRUE;
}

static
BOOL mem3Blt(rdpContext *context, MEM3BLT_ORDER *mem3blt)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->mem3Blt, frgContext, mem3blt);

	return TRUE;
}

static
BOOL glyphIndex(rdpContext *context, GLYPH_INDEX_ORDER *glyph_index)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->glyphIndex, frgContext, glyph_index);

	return TRUE;
}

static
BOOL fastIndex(rdpContext *context, const FAST_INDEX_ORDER *fast_index)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->fastIndex, frgContext, fast_index);

	return TRUE;
}

static
BOOL fastGlyph(rdpContext *context, const FAST_GLYPH_ORDER *fast_glyph)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->fastGlyph, frgContext, fast_glyph);

	return TRUE;
}

static
BOOL cacheBitmap(rdpContext *context, const CACHE_BITMAP_ORDER *cache_bitmap_order)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->cacheBitmap, frgContext, cache_bitmap_order);

    return TRUE;
}

static
BOOL cacheBitmapV2(rdpContext *context, CACHE_BITMAP_V2_ORDER *cache_bitmap_v2_order)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->cacheBitmapV2, frgContext, cache_bitmap_v2_order);

    return TRUE;
}

static
BOOL cacheBitmapV3(rdpContext *context, CACHE_BITMAP_V3_ORDER *cache_bitmap_v3_order)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->cacheBitmapV3, frgContext, cache_bitmap_v3_order);

    return TRUE;
}

static
BOOL cacheColorTable(rdpContext *context, const CACHE_COLOR_TABLE_ORDER *cache_color_table_order)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->cacheColorTable, frgContext, cache_color_table_order);

    return TRUE;
}

static
BOOL cacheGlyph(rdpContext *context, const CACHE_GLYPH_ORDER *cache_glyph_order)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->cacheGlyph, frgContext, cache_glyph_order);

    return TRUE;
}

static
BOOL cacheGlyphV2(rdpContext *context, const CACHE_GLYPH_V2_ORDER *cache_glyph_v2_order)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->cacheGlyphV2, frgContext, cache_glyph_v2_order);

    return TRUE;
}

static
BOOL cacheBrush(rdpContext *context, const CACHE_BRUSH_ORDER *cache_brush_order)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->cacheBrush, frgContext, cache_brush_order);

    return TRUE;
}

static
BOOL createOffscreenBitmap(rdpContext *context, const CREATE_OFFSCREEN_BITMAP_ORDER *create_offscreen_bitmap)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->createOffscreenBitmap, frgContext, create_offscreen_bitmap);

    return TRUE;
}

static
BOOL switchSurface(rdpContext *context, const SWITCH_SURFACE_ORDER *switch_surface)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->switchSurface, frgContext, switch_surface);

    return TRUE;
}

static
BOOL frameMarker(rdpContext *context, const FRAME_MARKER_ORDER *frame_marker)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->frameMarker, frgContext, frame_marker);

    return TRUE;
}

static
BOOL beginPaint(rdpContext *context)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->beginPaint, frgContext);

    return TRUE;
}

static
BOOL endPaint(rdpContext *context)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->endPaint, frgContext);

    return TRUE;
}

static
BOOL desktopResize(rdpContext *context)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;
	UINT32 width = context->settings->DesktopWidth;
	UINT32 height = context->settings->DesktopHeight;

	IFCALL(frgCallbacks->desktopResize, frgContext, width, height);

    return TRUE;
}

static
BOOL palette(rdpContext *context, const PALETTE_UPDATE *palette)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->palette, frgContext, palette);

    return TRUE;
}

static
BOOL setBounds(rdpContext *context, const rdpBounds *bounds)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->setBounds, frgContext, bounds);

    return TRUE;
}

static
BOOL surfaceFrameMarker(rdpContext *context, const SURFACE_FRAME_MARKER *surfaceFrameMarker)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->surfaceFrameMarker, frgContext, surfaceFrameMarker);

    return TRUE;
}

static
BOOL bitmapUpdate(rdpContext *context, const BITMAP_UPDATE *bitmap)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->bitmapUpdate, frgContext, bitmap);

    return TRUE;
}

static
BOOL surfaceBits(rdpContext *context, const SURFACE_BITS_COMMAND *surfaceBitsCommand)
{
	FrgContext *frgContext = (FrgContext *)context;
	FrgUpdateCallbacks *frgCallbacks = &frgContext->entryPoints.updateCallbacks;

	IFCALL(frgCallbacks->surfaceBits, frgContext, surfaceBitsCommand);

	return TRUE;
}

BOOL frgInitFreeRDPUpdate(freerdp *instance)
{
    rdpUpdate *update = instance->update;
    rdpPrimaryUpdate *primary = update->primary;
	rdpSecondaryUpdate *secondary = update->secondary;
	rdpAltSecUpdate *altsec = update->altsec;

	update->BeginPaint = beginPaint;
	update->DesktopResize = desktopResize;
	update->EndPaint = endPaint;
	update->Palette = palette;
	update->SetBounds = setBounds;
	update->SurfaceBits = surfaceBits;
	update->SurfaceFrameMarker = surfaceFrameMarker;
	update->BitmapUpdate = bitmapUpdate;

	primary->DstBlt = dstBlt;
	primary->PatBlt = patBlt;
	primary->ScrBlt = scrBlt;
	primary->OpaqueRect = opaqueRect;
	primary->DrawNineGrid = NULL;
	primary->MultiDstBlt = NULL;
	primary->MultiPatBlt = NULL;
	primary->MultiScrBlt = NULL;
	primary->MultiOpaqueRect = multiOpaqueRect;
	primary->MultiDrawNineGrid = NULL;
	primary->LineTo = lineTo;
	primary->Polyline = polyline;
	primary->MemBlt = memBlt;
	primary->Mem3Blt = mem3Blt;
	primary->SaveBitmap = NULL;
	primary->GlyphIndex = glyphIndex;
	primary->FastIndex = fastIndex;
	primary->FastGlyph = fastGlyph;
	primary->PolygonSC = NULL;
	primary->PolygonCB = NULL;
	primary->EllipseSC = NULL;
	primary->EllipseCB = NULL;
	
	secondary->CacheColorTable = cacheColorTable;
	secondary->CacheBrush = cacheBrush;
	secondary->CacheGlyph = cacheGlyph;
	secondary->CacheGlyphV2 = cacheGlyphV2;
	secondary->CacheBitmap = cacheBitmap;
	secondary->CacheBitmapV2 = cacheBitmapV2;
	secondary->CacheBitmapV3 = cacheBitmapV3;
	
	altsec->CreateOffscreenBitmap = createOffscreenBitmap;
	altsec->SwitchSurface = switchSurface;
	altsec->FrameMarker = frameMarker;

    return TRUE;
}