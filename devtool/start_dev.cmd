@echo off

pushd %~dp0
cd ..
set FREERDPGLUE_OUTPUT_DIR=%CD%\FreeRDPGlue\build\src\Debug
set PATH=%FREERDPGLUE_OUTPUT_DIR%;%PATH%
set FREERDPGLUE_OUTPUT_DIR=

start RemoteAccessGateway\RemoteAccessGateway.sln
code .

popd