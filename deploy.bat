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

set DEPLOY_DIR=%LOCALAPPDATA%\Halloumi\Shuffler
if not exist "%DEPLOY_DIR%" mkdir "%DEPLOY_DIR%"
robocopy "Halloumi.Shuffler\bin\x86\Release\net48" "%DEPLOY_DIR%" /E /IS /IT /NFL /NDL /NJH /NJS

echo Deploy complete.
pause
