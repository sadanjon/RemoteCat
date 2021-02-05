# Prerequisites:
# run install_freerdp.ps1

$ErrorActionPreference = "Stop"
$ProjectDir = Resolve-Path (Join-Path (Split-Path $MyInvocation.MyCommand.Source) ..)

Push-Location $ProjectDir

$FreeRDPInstallDir = Join-Path $ProjectDir "installs/FreeRDP"

Set-Location ./FreeRDPGlue

New-Item -ItemType "directory" -Force -Name "build" | Out-Null
Set-Location build
cmake .. "-DFREERDP_INSTALL_DIR=$FreeRDPInstallDir"

Pop-Location
