@echo off
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Start-V5.ps1" -Mode NewGame -DashCornerFix %*
if errorlevel 1 pause
