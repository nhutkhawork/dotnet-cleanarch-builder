@echo off

:: Variables
set STARTUP_PROJECT=../EasyBooking.API/
set PROJECT=../EasyBooking.EFCore/
set CONTEXT=AppDbContext
set OUTPUT_PATH=../EasyBooking.EFCore/Migrations

:: Prompt user for migration name
set /p MIGRATION_NAME=Enter the migration name: 

:: Check if migration name is empty
if "%MIGRATION_NAME%"=="" (
    echo Migration name cannot be empty. Exiting...
    exit /b 1
)

:: Run the dotnet ef command
dotnet ef --startup-project %STARTUP_PROJECT% --project %PROJECT% migrations add %MIGRATION_NAME% --context %CONTEXT% -o %OUTPUT_PATH%

:: Check if the command was successful
if %ERRORLEVEL% equ 0 (
    echo Migration "%MIGRATION_NAME%" created successfully.
) else (
    echo Failed to create migration "%MIGRATION_NAME%".
)