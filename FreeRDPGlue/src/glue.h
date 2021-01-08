#pragma once

#include <freerdpglue_export.h>

#include <stdint.h>

#include <freerdp/freerdp.h>
#include <freerdp/client/rdpgfx.h>
#include <freerdp/client/rdpei.h>

typedef struct 
{
    char *username;
    char *password;
    char *domain;
    BOOL passwordEntered;
} FrgOnAuthenticateResult;

typedef FrgOnAuthenticateResult (*FrgOnAuthenticateFn)(char *username, char *password, char *domain);

typedef void (*FrgOnContextCreatedFn)(void *frgContext);

typedef uint32_t (*FrgOnVerifyCertificateFn)(const BYTE *data, 
                                           size_t length,
                                           const char *hostname, 
                                           UINT16 port, 
                                           DWORD flags);

typedef struct
{
    FrgOnContextCreatedFn onContextCreated;
    FrgOnVerifyCertificateFn onVerifyCertificate;
    FrgOnAuthenticateFn onAuthenticate;
} FrgEntryPoints;

typedef struct
{
    rdpContext context;

    BOOL disconnectRequested;
    FrgEntryPoints entryPoints;

    /* Channels */
    RdpgfxClientContext *gfx;
    RdpeiClientContext *rdpei;
    // EncomspClientContext *encomsp;
} FrgContext;

typedef struct
{
    int argc;
    char **argv;
    FrgEntryPoints entryPoints;
} FrgMainOptions;

int FREERDPGLUE_EXPORT frgMain(FrgMainOptions *options);
void FREERDPGLUE_EXPORT frgDisconnect(FrgContext *frgContext);
