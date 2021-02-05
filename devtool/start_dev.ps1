$ErrorActionPreference = "Stop"
$ProjectDir = Resolve-Path (Join-Path (Split-Path $MyInvocation.MyCommand.Source) ..)

Push-Location $ProjectDir

$FreeRDPGlueOutputDir = Join-Path $ProjectDir "FreeRDPGlue/build/src/Debug"
$Env:PATH = @($FreeRDPGlueOutputDir, $Env:PATH) | Join-String -Separator ([IO.Path]::PathSeparator) 
$Env:RDP_BRIDGE_PATH = Join-Path $ProjectDir "RemoteAccessGateway/RdpBridge/bin/Debug/netcoreapp3.1/RdpBridge.dll"

Start-Process RemoteAccessGateway/RemoteAccessGateway.sln
code .

Pop-Location