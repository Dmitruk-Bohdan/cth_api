@echo off
setlocal enabledelayedexpansion

echo Running SQL scripts in 'C:\custom\uni\CTHelper\App\CTHelper\sql' via Docker container: app-db-1...

set CONTAINER_NAME=app-db-1

for /f "delims=" %%f in ('dir /b *.sql ^| sort') do (
    echo Executing: %%f
    docker exec -i %CONTAINER_NAME% psql -U cthelper -d cthelper < "%%f"
    if errorlevel 1 (
        echo [ERROR] Failed to execute: %%f
        pause
        exit /b 1
    )
    echo [OK] Completed: %%f
)

echo All SQL scripts executed successfully on database 'cthelper'!
pause