# Prerequisites:
# Install Protobuf compiler 3.14 `choco install protoc`

$ErrorActionPreference = "Stop"
$ProjectDir = Resolve-Path (Join-Path (Split-Path $MyInvocation.MyCommand.Source) ..)
Push-Location $ProjectDir

protoc -I=Protocol --csharp_out=RemoteAccessGateway\RdpBridge\Protocol Protocol\Rdp.proto Protocol\Common.proto

Pop-Location
