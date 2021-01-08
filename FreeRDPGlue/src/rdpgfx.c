#include "rdpgfx.h"

#include <freerdp/codec/color.h>

static
UINT handleResetGraphicsPdu(RdpgfxClientContext *context, const RDPGFX_RESET_GRAPHICS_PDU *resetGraphics)
{
    printf("%s:\n", __FUNCTION__);
    return CHANNEL_RC_OK;
}

static 
UINT handleStartFramePdu(RdpgfxClientContext *context, const RDPGFX_START_FRAME_PDU *startFrame)
{
    printf("%s:\n", __FUNCTION__);
    return CHANNEL_RC_OK;
}

static
UINT handleEndFramePdu(RdpgfxClientContext *context, const RDPGFX_END_FRAME_PDU *endFrame)
{
    printf("%s:\n", __FUNCTION__);
    return CHANNEL_RC_OK;
}

static
const char *getCodecIdName(UINT32 codecId)
{
    switch (codecId)
    {
        case RDPGFX_CODECID_UNCOMPRESSED:
            return "UNCOMPRESSED";
        case RDPGFX_CODECID_CAVIDEO:
            return "CAVIDEO";
        case RDPGFX_CODECID_CLEARCODEC:
            return "CLEARCODEC";
        case RDPGFX_CODECID_PLANAR:
            return "PLANAR";
        case RDPGFX_CODECID_AVC420:
            return "AVC420";
        case RDPGFX_CODECID_ALPHA:
            return "ALPHA";
        case RDPGFX_CODECID_AVC444:
            return "AVC444";
        case RDPGFX_CODECID_AVC444v2:
            return "AVC444v2";
        case RDPGFX_CODECID_CAPROGRESSIVE:
            return "CAPROGRESSIVE";
        case RDPGFX_CODECID_CAPROGRESSIVE_V2:
            return "CAPROGRESSIVE_V2";
        default:
            return "UNKNOWN";
    }
}

static
const char *getPixelFormatName(UINT32 pixelFormat)
{
    switch (pixelFormat) 
    {
        case PIXEL_FORMAT_ARGB32:
            return "ARGB32";
        case PIXEL_FORMAT_XRGB32:
            return "XRGB32";
        case PIXEL_FORMAT_ABGR32:
            return "ABGR32";
        case PIXEL_FORMAT_XBGR32:
            return "XBGR32";
        case PIXEL_FORMAT_BGRA32:
            return "BGRA32";
        case PIXEL_FORMAT_BGRX32:
            return "BGRX32";
        case PIXEL_FORMAT_RGBA32:
            return "RGBA32";
        case PIXEL_FORMAT_RGBX32:
            return "RGBX32";
        case PIXEL_FORMAT_RGB24:
            return "RGB24";
        case PIXEL_FORMAT_BGR24:
            return "BGR24";
        case PIXEL_FORMAT_RGB16:
            return "RGB16";
        case PIXEL_FORMAT_BGR16:
            return "BGR16";
        case PIXEL_FORMAT_ARGB15:
            return "ARGB15";
        case PIXEL_FORMAT_RGB15:
            return "RGB15";
        case PIXEL_FORMAT_ABGR15:
            return "ABGR15";
        case PIXEL_FORMAT_BGR15:
            return "BGR15";
        case PIXEL_FORMAT_RGB8:
            return "RGB8";
        case PIXEL_FORMAT_A4:
            return "A4";
        case PIXEL_FORMAT_MONO:
            return "MONO";
        default:
            return "UNKNOWN";
    }
}

static
UINT handleSurfaceCommandPdu(RdpgfxClientContext *context, const RDPGFX_SURFACE_COMMAND *cmd)
{
    printf(
        "function: %s\n"
        "codecId: %" PRIu32 " (%s)\n"
        "contextId: %" PRIu32 "\n"
        "format: %" PRIu32 " (%s)\n"
        "surfaceId: %" PRIu32 "\n"
        "left, top, right, bottom: %" PRIu32 ", %" PRIu32 ", %" PRIu32 ", %" PRIu32 "\n"
        "width, height: %" PRIu32 ", %" PRIu32 "\n"
        "length: %" PRIu32 "\n",
        __FUNCTION__,
        cmd->codecId, getCodecIdName(cmd->codecId),
        cmd->contextId,
        cmd->format, getPixelFormatName(cmd->format),
        cmd->surfaceId,
        cmd->left, cmd->top, cmd->right, cmd->bottom,
        cmd->width, cmd->height,
        cmd->length);
    return CHANNEL_RC_OK;
}

static
UINT handleDeleteEncodingContextPdu(RdpgfxClientContext *context, const RDPGFX_DELETE_ENCODING_CONTEXT_PDU *deleteEncodingContext)
{
    printf("%s:\n", __FUNCTION__);
    return CHANNEL_RC_OK;
}

static
UINT handleCreateSurfacePdu(RdpgfxClientContext *context, const RDPGFX_CREATE_SURFACE_PDU *createSurface)
{
    printf("%s:\n", __FUNCTION__);
    return CHANNEL_RC_OK;
}

static
UINT handleDeleteSurfacePdu(RdpgfxClientContext *context, const RDPGFX_DELETE_SURFACE_PDU *deleteSurface)
{
    printf("%s:\n", __FUNCTION__);
    return CHANNEL_RC_OK;
}

static
UINT handleSolidFillPdu(RdpgfxClientContext *context, const RDPGFX_SOLID_FILL_PDU *solidFill)
{
    printf("%s:\n", __FUNCTION__);
    return CHANNEL_RC_OK;
}

static
UINT handleSurfaceToSurfacePdu(RdpgfxClientContext *context, const RDPGFX_SURFACE_TO_SURFACE_PDU *surfaceToSurface)
{
    printf("%s:\n", __FUNCTION__);
    return CHANNEL_RC_OK;
}

static
UINT handleSurfaceToCachePdu(RdpgfxClientContext *context, const RDPGFX_SURFACE_TO_CACHE_PDU *surfaceToCache)
{
    printf("%s:\n", __FUNCTION__);
    return CHANNEL_RC_OK;
}

static
UINT handleCacheToSurfacePdu(RdpgfxClientContext *context, const RDPGFX_CACHE_TO_SURFACE_PDU *cacheToSurface)
{
    printf("%s:\n", __FUNCTION__);
    return CHANNEL_RC_OK;
}

static
UINT handleCacheImportReplyPdu(RdpgfxClientContext *context, const RDPGFX_CACHE_IMPORT_REPLY_PDU *cacheImportReply)
{
    printf("%s:\n", __FUNCTION__);
    return CHANNEL_RC_OK;
}

static
UINT handleEvictCacheEntryPdu(RdpgfxClientContext *context, const RDPGFX_EVICT_CACHE_ENTRY_PDU *evictCacheEntry)
{
    printf("%s:\n", __FUNCTION__);
    return CHANNEL_RC_OK;
}

static
UINT handleMapSurfaceToOutputPdu(RdpgfxClientContext *context, const RDPGFX_MAP_SURFACE_TO_OUTPUT_PDU *surfaceToOutput)
{
    printf("%s:\n", __FUNCTION__);
    return CHANNEL_RC_OK;
}

static
UINT handleMapSurfaceToScaledOutputPdu(RdpgfxClientContext *context, const RDPGFX_MAP_SURFACE_TO_SCALED_OUTPUT_PDU *surfaceToOutput)
{
    printf("%s:\n", __FUNCTION__);
    return CHANNEL_RC_OK;
}

static
UINT handleMapSurfaceToWindowPdu(RdpgfxClientContext *context, const RDPGFX_MAP_SURFACE_TO_WINDOW_PDU *surfaceToWindow)
{
    printf("%s:\n", __FUNCTION__);
    return CHANNEL_RC_OK;
}

static
UINT handleMapSurfaceToScaledWindowPdu(RdpgfxClientContext *context, const RDPGFX_MAP_SURFACE_TO_SCALED_WINDOW_PDU *surfaceToWindow)
{
    printf("%s:\n", __FUNCTION__);
    return CHANNEL_RC_OK;
}

static
UINT handleUpdateSurfacesPdu(RdpgfxClientContext *context)
{
    printf("%s:\n", __FUNCTION__);
    return CHANNEL_RC_OK;
}

void frgGfxHandlers(RdpgfxClientContext *gfx)
{
    printf("%s:\n", __FUNCTION__);
    gfx->ResetGraphics = handleResetGraphicsPdu;
    gfx->StartFrame = handleStartFramePdu;
    gfx->EndFrame = handleEndFramePdu;
    gfx->SurfaceCommand = handleSurfaceCommandPdu;
    gfx->DeleteEncodingContext = handleDeleteEncodingContextPdu;
    gfx->CreateSurface = handleCreateSurfacePdu;
    gfx->DeleteSurface = handleDeleteSurfacePdu;
    gfx->SolidFill = handleSolidFillPdu;
    gfx->SurfaceToSurface = handleSurfaceToSurfacePdu;
    gfx->SurfaceToCache = handleSurfaceToCachePdu;
    gfx->CacheToSurface = handleCacheToSurfacePdu;
    gfx->CacheImportReply = handleCacheImportReplyPdu;
    gfx->EvictCacheEntry = handleEvictCacheEntryPdu;
    gfx->MapSurfaceToOutput = handleMapSurfaceToOutputPdu;
    gfx->MapSurfaceToWindow = handleMapSurfaceToWindowPdu;
    gfx->MapSurfaceToScaledOutput = handleMapSurfaceToScaledOutputPdu;
    gfx->MapSurfaceToScaledWindow = handleMapSurfaceToScaledWindowPdu;
    gfx->UpdateSurfaces = handleUpdateSurfacesPdu;
}