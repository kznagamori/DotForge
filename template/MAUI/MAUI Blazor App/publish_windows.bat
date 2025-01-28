@echo off
dotnet publish -f net___DOTNET_VERSION___-windows___WINDOWS_SDK_VERSION___ -c Release -p:RuntimeIdentifierOverride=win10-x64 -p:WindowsPackageType=None -p:WindowsAppSDKSelfContained=true

