@echo off

set appversion=1.0.1.0
set framework=v4.0.30319

"%SystemDrive%\Windows\Microsoft.NET\Framework\%framework%\MSBuild.exe" build\Build.proj /t:Build /p:build_number=%appversion% /v:m
echo Done

if (%1)==(NOPAUSE) goto :eof
pause
