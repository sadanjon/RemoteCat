#include "cliprdr.h"

static 
UINT sendClientCapabilities(CliprdrClientContext *cliprdr)
{
	CLIPRDR_CAPABILITIES capabilities;
	CLIPRDR_GENERAL_CAPABILITY_SET generalCapabilitySet;

	if (!cliprdr)
		return ERROR_INTERNAL_ERROR;

	capabilities.cCapabilitiesSets = 1;
	capabilities.capabilitySets = (CLIPRDR_CAPABILITY_SET *)&(generalCapabilitySet);
	generalCapabilitySet.capabilitySetType = CB_CAPSTYPE_GENERAL;
	generalCapabilitySet.capabilitySetLength = 12;
	generalCapabilitySet.version = CB_CAPS_VERSION_2;
	generalCapabilitySet.generalFlags = CB_USE_LONG_FORMAT_NAMES | CB_STREAM_FILECLIP_ENABLED | CB_FILECLIP_NO_FILE_PATHS;
	return cliprdr->ClientCapabilities(cliprdr, &capabilities);
}


static 
UINT sendFormatList(CliprdrClientContext *cliprdr)
{
	UINT rc;
	int count = 0;
	UINT32 index;
	UINT32 numFormats = 0;
	UINT32 formatId = 0;
	char formatName[1024];
	CLIPRDR_FORMAT formats[1] = { 0 };
	CLIPRDR_FORMAT_LIST formatList = { 0 };

	if (!cliprdr)
		return ERROR_INTERNAL_ERROR;

	formats[0].formatId = CF_UNICODETEXT;
	formats[0].formatName = 0;

	formatList.numFormats = 1;
	formatList.formats = formats;
	formatList.msgType = CB_FORMAT_LIST;

	return cliprdr->ClientFormatList(cliprdr, &formatList);
}

static 
UINT onMonitorReady(CliprdrClientContext *cliprdr, const CLIPRDR_MONITOR_READY *monitorReady)
{
	UINT rc;

	if (!cliprdr || !monitorReady)
		return ERROR_INTERNAL_ERROR;

	rc = sendClientCapabilities(cliprdr);

	if (rc != CHANNEL_RC_OK)
		return rc;

	return sendFormatList(cliprdr);
}

BOOL rdpGlueCliprdrInit(rdpContext *context, CliprdrClientContext *cliprdr)
{
	if (!context || !cliprdr)
	{
		return FALSE;
	}

	cliprdr->MonitorReady = onMonitorReady;
	cliprdr->custom = context;

	return TRUE;

}
