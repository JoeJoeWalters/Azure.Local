@echo off
setlocal EnableExtensions EnableDelayedExpansion

:: Retarget to this script directory first, then move to repo root.
cd /D "%~dp0"
cd ..

set "OPENAPI_DIR=%CD%\openapi"
set "OPENAPI_JSON=%OPENAPI_DIR%\openapi.v1.json"
set "OPENAPI_HTML=%OPENAPI_DIR%\openapi.v1.html"
set "API_URL=http://127.0.0.1:5067/openapi/v1.json"

if exist "%OPENAPI_DIR%" rd /S /Q "%OPENAPI_DIR%"
mkdir "%OPENAPI_DIR%"

echo Starting Azure.Local.ApiService in Development mode...
powershell -NoProfile -Command "$wd = Get-Location; $p = Start-Process dotnet -ArgumentList 'run --project .\\src\\Azure.Local.ApiService\\Azure.Local.ApiService.csproj --no-build --urls http://127.0.0.1:5067' -WorkingDirectory $wd -PassThru; Set-Content -Path '%OPENAPI_DIR%\\apiservice.pid' -Value $p.Id"
set "API_PID="
if exist "%OPENAPI_DIR%\apiservice.pid" (
  set /p API_PID=<"%OPENAPI_DIR%\apiservice.pid"
)

echo Waiting for OpenAPI endpoint and downloading spec...
set "DOWNLOADED=0"
for /L %%I in (1,1,60) do (
  curl --silent --show-error --fail "%API_URL%" -o "%OPENAPI_JSON%" >nul 2>&1
  if not errorlevel 1 (
    set "DOWNLOADED=1"
    goto :downloaded
  )
  powershell -NoProfile -Command "Start-Sleep -Seconds 1" >nul
)

:downloaded
if "%DOWNLOADED%" neq "1" (
  echo Failed to download OpenAPI spec from %API_URL%.
  if defined API_PID (
    powershell -NoProfile -Command "Stop-Process -Id %API_PID% -ErrorAction SilentlyContinue"
  )
  exit /b 1
)

echo Validating OpenAPI version is 3.1.x...
powershell -NoProfile -Command "$doc = Get-Content -Raw '%OPENAPI_JSON%' | ConvertFrom-Json; if (-not ($doc.openapi -like '3.1*')) { throw ('Expected OpenAPI 3.1.x but got ' + $doc.openapi) }"
if errorlevel 1 (
  if defined API_PID (
    powershell -NoProfile -Command "Stop-Process -Id %API_PID% -ErrorAction SilentlyContinue"
  )
  exit /b 1
)

echo Generating HTML documentation with Redoc...
call npx --yes @redocly/cli@latest build-docs "%OPENAPI_JSON%" --output "%OPENAPI_HTML%"
if errorlevel 1 (
  if defined API_PID (
    powershell -NoProfile -Command "Stop-Process -Id %API_PID% -ErrorAction SilentlyContinue"
  )
  exit /b 1
)

if defined API_PID (
  powershell -NoProfile -Command "Stop-Process -Id %API_PID% -ErrorAction SilentlyContinue"
)

echo OpenAPI assets generated:
echo - %OPENAPI_JSON%
echo - %OPENAPI_HTML%
endlocal
