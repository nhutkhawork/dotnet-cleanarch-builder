@echo off

:: Variables
set STARTUP_PROJECT=../EasyBooking.API/
set CONTEXT=AppDbContext

:: Run the dotnet ef database update command
dotnet ef --startup-project %STARTUP_PROJECT% database update --context %CONTEXT%

:: Check if the command was successful
if %ERRORLEVEL% equ 0 (
    echo Database updated successfully for context "%CONTEXT%".
) else (
    echo Failed to update the database for context "%CONTEXT%".
)