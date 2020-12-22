cd %~dp0
XCopy /y /i components\bin\debug\*.nupkg "%userprofile%\LocalNuget\"
"%userprofile%\LocalNuget\sync.bat"
pause
