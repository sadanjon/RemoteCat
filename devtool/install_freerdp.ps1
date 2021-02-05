# Prerequisites:
# Install OpenSSL from http://slproweb.com/products/Win32OpenSSL.html, or `choco install openssl`
# Add env var 'OPENSSL_ROOT_DIR' that points to the installed location: C:\Program Files\OpenSSL-Win64

$ErrorActionPreference = "Stop"
$ProjectDir = Resolve-Path (Join-Path (Split-Path $MyInvocation.MyCommand.Source) ..)
Push-Location $ProjectDir

New-Item -ItemType "directory" -Force -Name "installs" | Out-Null
New-Item -ItemType "directory" -Force -Name "FreeRDP/build" | Out-Null

Set-Location .\FreeRDP\build

$FreeRDPInstallDir = Join-Path $ProjectDir "installs/FreeRDP"
cmake .. "-DCHANNEL_URBDRC=OFF" "-DCMAKE_INSTALL_PREFIX=$FreeRDPInstallDir"
cmake --build . --config Debug
cmake --install . --config Debug

Pop-Location
