@echo off
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Start-V5.ps1" -Mode NewGame -BodyHeight 1.3 %*
if errorlevel 1 pause
