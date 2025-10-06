# PowerShell script to install and manage Radarr/Sonarr MCP Server as a Windows Service
# Requires NSSM (Non-Sucking Service Manager) to be installed
# Download NSSM from: https://nssm.cc/download

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("install", "uninstall", "start", "stop", "restart", "status")]
    [string]$Action = "status",
    
    [Parameter(Mandatory=$false)]
    [string]$ServiceName = "RadarrSonarrMCP",
    
    [Parameter(Mandatory=$false)]
    [string]$ExecutablePath = "$PSScriptRoot\RadarrSonarrMcp\bin\Release\net8.0\win-x64\publish\RadarrSonarrMcp.exe"
)

# Check if running as Administrator
$IsAdmin = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if (-not $IsAdmin -and $Action -ne "status") {
    Write-Host "❌ This script requires Administrator privileges for service management." -ForegroundColor Red
    Write-Host "   Please run PowerShell as Administrator and try again." -ForegroundColor Yellow
    exit 1
}

# Check if NSSM is installed
$NssmPath = (Get-Command nssm -ErrorAction SilentlyContinue).Source

if (-not $NssmPath -and $Action -ne "status") {
    Write-Host "❌ NSSM (Non-Sucking Service Manager) is not installed or not in PATH." -ForegroundColor Red
    Write-Host ""
    Write-Host "To install NSSM:" -ForegroundColor Yellow
    Write-Host "1. Download from: https://nssm.cc/download" -ForegroundColor White
    Write-Host "2. Extract to a folder (e.g., C:\Tools\nssm)" -ForegroundColor White
    Write-Host "3. Add the folder to your PATH" -ForegroundColor White
    Write-Host ""
    Write-Host "Or install via Chocolatey:" -ForegroundColor Yellow
    Write-Host "   choco install nssm" -ForegroundColor White
    Write-Host ""
    exit 1
}

function Get-ServiceStatus {
    $service = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
    if ($service) {
        Write-Host "Service Status: " -NoNewline -ForegroundColor Cyan
        switch ($service.Status) {
            "Running" { Write-Host "Running ✅" -ForegroundColor Green }
            "Stopped" { Write-Host "Stopped ⏸️" -ForegroundColor Yellow }
            default { Write-Host $service.Status -ForegroundColor Yellow }
        }
        Write-Host "Startup Type: $($service.StartType)" -ForegroundColor Cyan
    } else {
        Write-Host "Service Status: Not Installed ❌" -ForegroundColor Red
    }
}

function Install-MCPService {
    # Check if executable exists
    if (-not (Test-Path $ExecutablePath)) {
        Write-Host "❌ Executable not found at: $ExecutablePath" -ForegroundColor Red
        Write-Host "   Please run .\publish.ps1 first to build the application." -ForegroundColor Yellow
        exit 1
    }
    
    $WorkingDir = Split-Path -Parent $ExecutablePath
    
    # Check if config.json exists
    if (-not (Test-Path "$WorkingDir\config.json")) {
        Write-Host "⚠️  Warning: config.json not found at: $WorkingDir" -ForegroundColor Yellow
        Write-Host "   The service may fail to start without a valid config.json" -ForegroundColor Yellow
    }
    
    # Check if service already exists
    $ExistingService = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
    if ($ExistingService) {
        Write-Host "⚠️  Service '$ServiceName' already exists!" -ForegroundColor Yellow
        $Response = Read-Host "Do you want to uninstall and reinstall? (Y/N)"
        if ($Response -eq 'Y' -or $Response -eq 'y') {
            Uninstall-MCPService
        } else {
            Write-Host "Installation cancelled." -ForegroundColor Yellow
            exit 0
        }
    }
    
    Write-Host "Installing service '$ServiceName'..." -ForegroundColor Green
    
    # Install service
    & nssm install $ServiceName $ExecutablePath
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Failed to install service!" -ForegroundColor Red
        exit 1
    }
    
    # Configure service
    & nssm set $ServiceName AppDirectory $WorkingDir
    & nssm set $ServiceName DisplayName "Radarr/Sonarr MCP Server"
    & nssm set $ServiceName Description "Model Context Protocol server for Radarr and Sonarr integration with AI assistants"
    & nssm set $ServiceName Start SERVICE_AUTO_START
    
    # Configure logging
    $LogDir = "$WorkingDir\logs"
    if (-not (Test-Path $LogDir)) {
        New-Item -ItemType Directory -Path $LogDir -Force | Out-Null
    }
    
    & nssm set $ServiceName AppStdout "$LogDir\service-stdout.log"
    & nssm set $ServiceName AppStderr "$LogDir\service-stderr.log"
    & nssm set $ServiceName AppRotateFiles 1
    & nssm set $ServiceName AppRotateOnline 1
    & nssm set $ServiceName AppRotateBytes 1048576  # 1MB
    
    Write-Host "✅ Service installed successfully!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Service Details:" -ForegroundColor Cyan
    Write-Host "  Name: $ServiceName" -ForegroundColor White
    Write-Host "  Executable: $ExecutablePath" -ForegroundColor White
    Write-Host "  Working Directory: $WorkingDir" -ForegroundColor White
    Write-Host "  Logs: $LogDir" -ForegroundColor White
    Write-Host ""
    
    $Response = Read-Host "Start the service now? (Y/N)"
    if ($Response -eq 'Y' -or $Response -eq 'y') {
        Start-MCPService
    }
}

function Uninstall-MCPService {
    $service = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
    
    if (-not $service) {
        Write-Host "❌ Service '$ServiceName' is not installed." -ForegroundColor Red
        exit 1
    }
    
    # Stop service if running
    if ($service.Status -eq 'Running') {
        Write-Host "Stopping service..." -ForegroundColor Yellow
        & nssm stop $ServiceName
        Start-Sleep -Seconds 2
    }
    
    Write-Host "Uninstalling service '$ServiceName'..." -ForegroundColor Yellow
    & nssm remove $ServiceName confirm
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Service uninstalled successfully!" -ForegroundColor Green
    } else {
        Write-Host "❌ Failed to uninstall service!" -ForegroundColor Red
        exit 1
    }
}

function Start-MCPService {
    Write-Host "Starting service '$ServiceName'..." -ForegroundColor Green
    & nssm start $ServiceName
    
    Start-Sleep -Seconds 2
    Get-ServiceStatus
}

function Stop-MCPService {
    Write-Host "Stopping service '$ServiceName'..." -ForegroundColor Yellow
    & nssm stop $ServiceName
    
    Start-Sleep -Seconds 2
    Get-ServiceStatus
}

function Restart-MCPService {
    Write-Host "Restarting service '$ServiceName'..." -ForegroundColor Yellow
    & nssm restart $ServiceName
    
    Start-Sleep -Seconds 2
    Get-ServiceStatus
}

# Main script execution
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Radarr/Sonarr MCP Server - Service Manager" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

switch ($Action) {
    "install" {
        Install-MCPService
    }
    "uninstall" {
        Uninstall-MCPService
    }
    "start" {
        Start-MCPService
    }
    "stop" {
        Stop-MCPService
    }
    "restart" {
        Restart-MCPService
    }
    "status" {
        Get-ServiceStatus
    }
}

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Available Commands:" -ForegroundColor Cyan
Write-Host "  .\manage-service.ps1 install   - Install the service" -ForegroundColor White
Write-Host "  .\manage-service.ps1 uninstall - Uninstall the service" -ForegroundColor White
Write-Host "  .\manage-service.ps1 start     - Start the service" -ForegroundColor White
Write-Host "  .\manage-service.ps1 stop      - Stop the service" -ForegroundColor White
Write-Host "  .\manage-service.ps1 restart   - Restart the service" -ForegroundColor White
Write-Host "  .\manage-service.ps1 status    - Check service status" -ForegroundColor White
Write-Host ""
