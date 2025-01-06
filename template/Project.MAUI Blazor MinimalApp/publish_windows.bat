@echo off
dotnet publish -f ___DOTNET_VERSION___-___WINDOWS_SDK_VERSION___ -c Release -p:RuntimeIdentifierOverride=win10-x64 -p:WindowsPackageType=None -p:WindowsAppSDKSelfContained=true

