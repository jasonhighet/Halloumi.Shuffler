@echo off

for /f "usebackq tokens=*" %%i in (`"%PROGRAMFILES(X86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do set MSBUILD=%%i

if not defined MSBUILD (
    echo Could not find MSBuild via vswhere. Is Visual Studio installed?
    pause
    exit /b 1
)

"%MSBUILD%" Halloumi.Shuffler.sln /p:Platform=x86 /p:Configuration=Release /m /nologo
if %errorlevel% neq 0 (
    echo Build failed.
    pause
    exit /b %errorlevel%
)

echo Build complete.
pause
