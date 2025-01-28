@echo off
dotnet publish -f net___DOTNET_VERSION___-android -c Release /p:RuntimeIdentifier=android-arm64

