@echo off
setlocal

rem Prerequisites:
rem run install_freerdp.cmd

pushd %~dp0
cd ..
set PROJECT_DIR=%CD%
set FREERDP_INSTALL_DIR=%PROJECT_DIR%\installs\FreeRDP

cd FreeRDPGlue

if not exist build ( mkdir build )

cd build
cmake .. -DFREERDP_INSTALL_DIR=%FREERDP_INSTALL_DIR%

popd