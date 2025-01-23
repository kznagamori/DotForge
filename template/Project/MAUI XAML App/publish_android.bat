@echo off
dotnet publish -f ___DOTNET_VERSION___-android -c Release /p:RuntimeIdentifier=android-arm64

