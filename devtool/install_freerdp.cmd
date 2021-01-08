@echo off
setlocal

rem Prerequisites:
rem Install OpenSSL from http://slproweb.com/products/Win32OpenSSL.html
rem Add env var 'OPENSSL_ROOT_DIR' that points to the installed location: C:\Program Files\OpenSSL-Win64

pushd %~dp0
cd ..
set PROJECT_DIR=%CD%

if not exist installs ( mkdir installs )

cd FreeRDP

if not exist build ( mkdir build )
cd build

cmake .. -DCHANNEL_URBDRC=OFF -DCMAKE_INSTALL_PREFIX=%PROJECT_DIR%\installs\FreeRDP
cmake --build . --config Debug
cmake --install . --config Debug
popd