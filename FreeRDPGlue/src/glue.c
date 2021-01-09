#include "glue.h"

#include <stdio.h>

#include <freerdp/client/cmdline.h>
#include <freerdp/client/channels.h>
#include <freerdp/cache/cache.h>

#include <freerdp/client/cliprdr.h>
#include <freerdp/client/encomsp.h>
#include <winpr/winpr.h>
#include <winpr/crt.h>
// #include <winpr/synch.h>

#include "rdpgfx.h"
#include "cliprdr.h"
#include "context_update.h"

#define EVENT_HANDLERS_COUNT 64

static
BOOL frgGlobalInit(void)
{

#ifdef _WIN32
    WSADATA wsaData;

    WSAStartup(0x101, &wsaData);
#endif

    freerdp_register_addin_provider(freerdp_channels_load_static_addin_entry, 0);
    return TRUE;
}

static
void frgGlobalUninit(void)
{
#ifdef _WIN32
    WSACleanup();
#endif
}

/**
 * Callback set in the rdp_freerdp structure, and used to get the user's password,
 * if required to establish the connection.
 *
 * This function is actually called in `credssp_ntlmssp_client_init()`
 * @see `rdp_server_accept_nego()` and `rdp_check_fds()`
 *
 * @param instance pointer to the rdp_freerdp structure that contains the connection settings
 * @param username unused
 * @param password on return: pointer to a character string that will be filled by the password
 * entered by the user. Note that this character string will be allocated inside the function, and
 * needs to be deallocated by the caller using free(), even in case this function fails.
 * @param domain unused
 *
 * @return TRUE if a password was successfully entered. See freerdp_passphrase_read() for more
 * details.
 */
static
BOOL frgAuthenticate(freerdp *instance, char **username, char **password, char **domain)
{
    FrgContext *frgContext = (FrgContext *)instance->context;
    FrgOnAuthenticateResult result = { 0 };

    if (frgContext->entryPoints.onAuthenticate) 
    {
        result = frgContext->entryPoints.onAuthenticate(*username, *password, *domain);
        
        if (result.passwordEntered) 
        {
            *username = result.username;
            *password = result.password;
            *domain = result.domain;
        }

        return result.passwordEntered;
    }

    return FALSE;
}

/**
 * Callback set in the rdp_freerdp structure, and used to make a certificate validation when the
 * connection requires it.
 *
 * This function will actually be called by `tls_verify_certificate()`.
 *
 * @see `rdp_client_connect()` and `tls_connect()`
 *
 * @param instance     pointer to the rdp_freerdp structure that contains the connection settings
 * @param host         The host currently connecting to
 * @param port         The port currently connecting to
 * @param common_name  The common name of the certificate, should match host or an alias of it
 * @param subject      The subject of the certificate
 * @param issuer       The certificate issuer name
 * @param fingerprint  The fingerprint of the certificate
 * @param flags        See VERIFY_CERT_FLAG_* for possible values.
 *
 * @return 1 if the certificate is trusted, 2 if temporary trusted, 0 otherwise.
 */

int frgVerifyCertificateEx(freerdp *instance, const BYTE *data, size_t length,
    const char *hostname, UINT16 port, DWORD flags)
{
    FrgContext *frgContext = (FrgContext*)instance->context;

    if (frgContext->entryPoints.onVerifyCertificate)
    {
        return frgContext->entryPoints.onVerifyCertificate(data, length, hostname, port, flags);
    }
    else
    {
        return 1;
    }
}

/**
 * Callback to report failed logon.
 * Use `freerdp_get_logon_error_info_data(data)` and `freerdp_get_logon_error_info_type(type)` do
 * get human readable representations of the given parameters.
 *
 * Return 1 (as far as I know)
 */
static
int frgLogonErrorInfo(freerdp *instance, UINT32 data, UINT32 type)
{
    return 1;
}

static
void frgOnChannelConnectedEventHandler(void *context, ChannelConnectedEventArgs *e)
{
    FrgContext *_context = (FrgContext *)context;

    if (strcmp(e->name, RDPEI_DVC_CHANNEL_NAME) == 0)
    {
        _context->rdpei = (RdpeiClientContext *)e->pInterface;
    }
    else if (strcmp(e->name, RDPGFX_DVC_CHANNEL_NAME) == 0)
    {
        _context->gfx = (RdpgfxClientContext *)e->pInterface;
        frgGfxHandlers(_context->gfx);
        // gdi_graphics_pipeline_init(tf->context.gdi, (RdpgfxClientContext *)e->pInterface);
    }
    else if (strcmp(e->name, RAIL_SVC_CHANNEL_NAME) == 0)
    {
    }
    else if (strcmp(e->name, CLIPRDR_SVC_CHANNEL_NAME) == 0)
    {
        rdpGlueCliprdrInit(&_context->context, (CliprdrClientContext *)e->pInterface);
    }
    else if (strcmp(e->name, ENCOMSP_SVC_CHANNEL_NAME) == 0)
    {
        // tf_encomsp_init(tf, (EncomspClientContext *)e->pInterface);
    }
}

static
void frgOnChannelDisconnectedEventHandler(void *context, ChannelDisconnectedEventArgs *e)
{
    FrgContext *_context = (FrgContext *)context;

    if (strcmp(e->name, RDPEI_DVC_CHANNEL_NAME) == 0)
    {
        _context->rdpei = NULL;
    }
    else if (strcmp(e->name, RDPGFX_DVC_CHANNEL_NAME) == 0)
    {
        _context->gfx = NULL;
        // gdi_graphics_pipeline_uninit(tf->context.gdi, (RdpgfxClientContext *)e->pInterface);
    }
    else if (strcmp(e->name, RAIL_SVC_CHANNEL_NAME) == 0)
    {
    }
    else if (strcmp(e->name, CLIPRDR_SVC_CHANNEL_NAME) == 0)
    {
    }
    else if (strcmp(e->name, ENCOMSP_SVC_CHANNEL_NAME) == 0)
    {
        // tf_encomsp_uninit(tf, (EncomspClientContext *)e->pInterface);
    }
}

static
BOOL frgPreConnect(freerdp *instance)
{
    rdpSettings *settings;
    settings = instance->settings;

    // Optional OS identifier sent to server
    settings->OsMajorType = OSMAJORTYPE_WINDOWS;
    settings->OsMinorType = OSMINORTYPE_WINDOWS_NT;


    // TODO: Find out what is `settings->OrderSupport`. Should I initialize it here?

     /* Register the channel listeners.
      * They are required to set up / tear down channels if they are loaded. */
    PubSub_SubscribeChannelConnected(instance->context->pubSub, frgOnChannelConnectedEventHandler);
    PubSub_SubscribeChannelDisconnected(instance->context->pubSub, frgOnChannelDisconnectedEventHandler);

    freerdp_set_param_uint32(instance->settings, FreeRDP_KeyboardLayout, -858993460 & 0x0000FFFF);

    // Parse instance->settings to load static and dynamic channels
    if (!freerdp_client_load_addins(instance->context->channels, instance->settings))
    {
        return FALSE;
    }


    return TRUE;
}

static
BOOL frgFrameMarker(rdpContext *context, const FRAME_MARKER_ORDER *frame_marker)
{
    printf("XXX: FRAME_MARKER\n");
    return TRUE;
}

static
BOOL frgEndPaint(rdpContext *context)
{
    printf("XXX: END_PAINT\n");
    return TRUE;
}

static
BOOL frgInitFreeRDPCache(freerdp *instance)
{
    instance->context->cache = cache_new(instance->settings);
    if (!instance->context->cache)
    {
        return FALSE;
    }
    return TRUE;
}

/* Called after a RDP connection was successfully established.
 * Settings might have changed during negociation of client / server feature
 * support.
 *
 * Set up local framebuffers and paing callbacks.
 * If required, register pointer callbacks to change the local mouse cursor
 * when hovering over the RDP window
 */
static
BOOL frgPostConnect(freerdp *instance)
{
    FrgContext *frgContext = (FrgContext*) instance->context;

    if (!frgInitFreeRDPCache(instance))
    {
        return FALSE;
    }

    if (!frgInitFreeRDPUpdate(instance))
    {
        return FALSE;
    }

    return TRUE;
}

/**
 * This function is called whether a session ends by failure or success.
 * Clean up everything allocated by pre_connect and post_connect.
 */
static
void frgPostDisconnect(freerdp *instance)
{
    FrgContext *context = 0;

    if (!instance)
        return;

    if (!instance->context)
        return;

    context = (FrgContext *)instance->context;

    PubSub_UnsubscribeChannelConnected(instance->context->pubSub, frgOnChannelConnectedEventHandler);
    PubSub_UnsubscribeChannelDisconnected(instance->context->pubSub, frgOnChannelDisconnectedEventHandler);

    // gdi_free(instance);

    /* TODO : Clean up custom stuff */
    WINPR_UNUSED(context);
}

static
BOOL frgClientNew(freerdp *instance, rdpContext *context)
{
    FrgContext *tf = (FrgContext *)context;
    WINPR_UNUSED(tf);

    if (!instance || !context)
    {
        return FALSE;
    }

    instance->PreConnect = frgPreConnect;
    instance->PostConnect = frgPostConnect;
    instance->PostDisconnect = frgPostDisconnect;
    instance->Authenticate = frgAuthenticate;
    instance->GatewayAuthenticate = frgAuthenticate;
    instance->VerifyX509Certificate = frgVerifyCertificateEx;
    instance->LogonErrorInfo = frgLogonErrorInfo;

    // Should the client set its display now?

    return TRUE;
}

static
int setRdpClientEntryPoints(RDP_CLIENT_ENTRY_POINTS *pEntryPoints)
{
    ZeroMemory(pEntryPoints, sizeof(RDP_CLIENT_ENTRY_POINTS));
    pEntryPoints->Version = RDP_CLIENT_INTERFACE_VERSION;
    pEntryPoints->Size = sizeof(RDP_CLIENT_ENTRY_POINTS_V1);
    pEntryPoints->GlobalInit = frgGlobalInit;
    pEntryPoints->GlobalUninit = frgGlobalUninit;
    pEntryPoints->ContextSize = sizeof(FrgContext);
    pEntryPoints->ClientNew = frgClientNew;
    pEntryPoints->ClientFree = 0;
    pEntryPoints->ClientStart = 0;
    pEntryPoints->ClientStop = 0;
    return 0;
}

static
DWORD frgRun(freerdp *freeRdpInstance)
{
    rdpSettings *settings = freeRdpInstance->settings;
    DWORD nCount = 0;
    DWORD status = 0;
    DWORD result = 0;
    HANDLE handles[EVENT_HANDLERS_COUNT];
    BOOL rc = 0;
    FrgContext *frgContext = 0;

    rc = freerdp_connect(freeRdpInstance);

    if (settings->AuthenticationOnly)
    {
        result = freerdp_get_last_error(freeRdpInstance->context);
        freerdp_abort_connect(freeRdpInstance);
        printf("Authentication only, exit status: %s\n", freerdp_get_last_error_string(result));
        goto disconnect;
    }

    if (!rc)
    {
        result = freerdp_get_last_error(freeRdpInstance->context);
        printf("Connection failure: %s\n", freerdp_get_last_error_string(result));
        return result;
    }

    frgContext = (FrgContext*) freeRdpInstance->context;

    while (!freerdp_shall_disconnect(freeRdpInstance) && !frgContext->disconnectRequested)
    {
        nCount = freerdp_get_event_handles(freeRdpInstance->context, handles, EVENT_HANDLERS_COUNT);

        if (nCount == 0)
        {
            printf("%s: freerdp_get_event_handles failed\n", __FUNCTION__);
            break;
        }

        status = WaitForMultipleObjects(nCount, handles, FALSE, 100);

        if (status == WAIT_FAILED)
        {
            printf("%s: WaitForMultipleObjects failed with %" PRIu32 "\n", __FUNCTION__, status);
            break;
        }

        if (!freerdp_check_event_handles(freeRdpInstance->context))
        {
            if (freerdp_get_last_error(freeRdpInstance->context) == FREERDP_ERROR_SUCCESS)
            {
                printf("Failed to check FreeRDP event handles\n");
            }
            break;
        }
    }

disconnect:
    freerdp_disconnect(freeRdpInstance);
    return result;
}

void frgDisconnect(FrgContext *frgContext)
{
    frgContext->disconnectRequested = TRUE;
}

int frgMain(FrgMainOptions *options)
{
    RDP_CLIENT_ENTRY_POINTS clientEntryPoints = { 0 };
    rdpContext *freeRdpContext = 0;
    FrgContext *frgContext = 0;
    DWORD status = 0;

    printf("Hello FreeRDP: %s\n", freerdp_get_version_string());

    setRdpClientEntryPoints(&clientEntryPoints);
    freeRdpContext = freerdp_client_context_new(&clientEntryPoints);
    if (!freeRdpContext)
    {
        goto error;
    }

    // Setup FRG context
    frgContext = (FrgContext*)freeRdpContext;
    frgContext->disconnectRequested = FALSE;
    frgContext->entryPoints = options->entryPoints;

    if (frgContext->entryPoints.onContextCreated)
    {
        frgContext->entryPoints.onContextCreated(frgContext);
    }

    // Set FRG specific settings
    freeRdpContext->settings->ExternalCertificateManagement = TRUE;

    status = freerdp_client_settings_parse_command_line(freeRdpContext->settings, options->argc, options->argv, FALSE);
    status = freerdp_client_settings_command_line_status_print(freeRdpContext->settings, status, options->argc, options->argv);

    if (status)
    {
        goto error;
    }

    if (freerdp_client_start(freeRdpContext) != 0)
    {
        goto error;
    }

    status = frgRun(freeRdpContext->instance);

    if (freerdp_client_stop(freeRdpContext) != 0)
    {
        goto error;
    }

error:
    return status;
}
