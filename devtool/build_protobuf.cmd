@echo off
setlocal

rem Prerequisites:
rem Install Protobuf compiler 3.14

pushd %~dp0
cd ..

protoc -I=Protocol --csharp_out=RemoteAccessGateway\RdpBridge\Protocol Protocol\Rdp.proto

popd