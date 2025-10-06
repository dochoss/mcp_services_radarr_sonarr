# PowerShell script to publish a self-contained executable for Windows
# This creates a standalone .exe that includes the .NET runtime

param(
    [string]$Configuration = "Release",
    [string]$Runtime = "win-x64"
)

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Radarr/Sonarr MCP Server - Publish Script" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Navigate to project directory
$ProjectPath = "$PSScriptRoot\RadarrSonarrMcp"
$PublishPath = "$ProjectPath\bin\$Configuration\net8.0\$Runtime\publish"

Write-Host "Configuration: $Configuration" -ForegroundColor Yellow
Write-Host "Runtime: $Runtime" -ForegroundColor Yellow
Write-Host "Project Path: $ProjectPath" -ForegroundColor Yellow
Write-Host "Publish Path: $PublishPath" -ForegroundColor Yellow
Write-Host ""

# Clean previous build
Write-Host "Cleaning previous build..." -ForegroundColor Green
if (Test-Path $PublishPath) {
    Remove-Item -Path $PublishPath -Recurse -Force
}

# Publish the application
Write-Host "Publishing application..." -ForegroundColor Green
dotnet publish $ProjectPath `
    --configuration $Configuration `
    --runtime $Runtime `
    --self-contained true `
    --output $PublishPath `
    /p:PublishSingleFile=true `
    /p:PublishTrimmed=false `
    /p:IncludeNativeLibrariesForSelfExtract=true

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed!" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "✅ Build successful!" -ForegroundColor Green
Write-Host ""

# Copy config.json if it exists
$SourceConfig = "$ProjectPath\config.json"
$DestConfig = "$PublishPath\config.json"

if (Test-Path $SourceConfig) {
    Write-Host "Copying config.json to publish folder..." -ForegroundColor Green
    Copy-Item -Path $SourceConfig -Destination $DestConfig -Force
    Write-Host "✅ Config file copied!" -ForegroundColor Green
} else {
    Write-Host "⚠️  Warning: config.json not found at $SourceConfig" -ForegroundColor Yellow
    Write-Host "   You'll need to create one in the publish folder before running." -ForegroundColor Yellow
    
    # Copy example config instead
    $ExampleConfig = "$ProjectPath\config.example.json"
    if (Test-Path $ExampleConfig) {
        Copy-Item -Path $ExampleConfig -Destination "$PublishPath\config.example.json" -Force
        Write-Host "   Copied config.example.json as reference." -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Published Files Location:" -ForegroundColor Cyan
Write-Host $PublishPath -ForegroundColor White
Write-Host ""
Write-Host "Executable:" -ForegroundColor Cyan
Write-Host "$PublishPath\RadarrSonarrMcp.exe" -ForegroundColor White
Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Next Steps:" -ForegroundColor Cyan
Write-Host "1. Ensure config.json is in the publish folder" -ForegroundColor White
Write-Host "2. Test the executable: cd `"$PublishPath`"; .\RadarrSonarrMcp.exe" -ForegroundColor White
Write-Host "3. Configure Claude Desktop (stdio, see WINDOWS_SETUP.md)" -ForegroundColor White
Write-Host ""
Write-Host "For Claude Desktop, use this path:" -ForegroundColor Yellow
Write-Host ($PublishPath + "\RadarrSonarrMcp.exe").Replace('\', '\\') -ForegroundColor Green
Write-Host ""

# Offer to open the publish folder
$Response = Read-Host "Open publish folder in Explorer? (Y/N)"
if ($Response -eq 'Y' -or $Response -eq 'y') {
    explorer.exe $PublishPath
}
