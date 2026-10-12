@echo off
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Start-V4.ps1" -Mode BeforeBoss -Boss b3 -AV1 -PhaseTwo %*
if errorlevel 1 pause
