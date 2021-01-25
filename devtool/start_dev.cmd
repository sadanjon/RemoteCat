@echo off

pushd %~dp0
cd ..

set FREERDPGLUE_OUTPUT_DIR=%CD%\FreeRDPGlue\build\src\Debug
set PATH=%FREERDPGLUE_OUTPUT_DIR%;%PATH%
set RDP_BRIDGE_PATH=%CD%\RemoteAccessGateway\RdpBridge\bin\Debug\netcoreapp3.1\RdpBridge.dll

start RemoteAccessGateway\RemoteAccessGateway.sln
code .

popd