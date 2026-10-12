@echo off
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Start-V4.ps1" -Mode BeforeBoss -Boss base -AV1 %*
if errorlevel 1 pause
