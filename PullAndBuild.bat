@echo off

if exist "%PROGRAMFILES%\Git\bin\git.exe" (
	"%PROGRAMFILES%\Git\bin\git.exe" pull origin master
) else (
	"%PROGRAMFILES(86)%\Git\bin\git.exe" pull origin master
)
"%PROGRAMFILES(X86)%\Microsoft Visual Studio 14.0\Common7\IDE\devenv" "Halloumi.Shuffler.sln" /Build "Release"
pause