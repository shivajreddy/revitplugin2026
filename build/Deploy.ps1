#Requires -Version 5.1
<#
.SYNOPSIS
    Deploy the Revit plugin to the local Revit Addins folder.

.PARAMETER Configuration
    Build configuration: Debug or Release (default: Release)

.PARAMETER RevitVersion
    Target Revit version year (default: 2026)

.PARAMETER TargetPath
    Override the destination Addins folder path.

.EXAMPLE
    .\Deploy.ps1
    # Builds Release and deploys to %APPDATA%\Autodesk\Revit\Addins\2026

.EXAMPLE
    .\Deploy.ps1 -Configuration Debug
    # Deploys the Debug build (use during active development)
#>
param(
    [string]$Configuration = "Release",
    [string]$RevitVersion = "2026",
    [string]$TargetPath = "$env:APPDATA\Autodesk\Revit\Addins\$RevitVersion"
)

$ErrorActionPreference = "Stop"
$repoRoot = Split-Path -Parent $PSScriptRoot
$srcDir = Join-Path $repoRoot "src\RevitPlugin2026\bin\$Configuration"

Write-Host "Building $Configuration|x64..." -ForegroundColor Cyan
Push-Location $repoRoot
try {
    dotnet build RevitPlugin2026.sln `
        --configuration $Configuration `
        -p:Platform=x64 `
        --no-restore
    if ($LASTEXITCODE -ne 0) { throw "Build failed." }
}
finally {
    Pop-Location
}

Write-Host "Deploying to: $TargetPath" -ForegroundColor Cyan

$pluginDir = Join-Path $TargetPath "RevitPlugin2026"
if (-not (Test-Path $pluginDir)) {
    New-Item -ItemType Directory -Path $pluginDir | Out-Null
}

# Copy plugin DLL and PDB
Copy-Item "$srcDir\RevitPlugin2026.dll" $pluginDir -Force
Copy-Item "$srcDir\RevitPlugin2026.pdb" $pluginDir -Force -ErrorAction SilentlyContinue

# Copy Serilog and other runtime dependencies (NOT Revit API DLLs — Revit provides those)
Get-ChildItem "$srcDir\*.dll" |
    Where-Object { $_.Name -notmatch "^RevitAPI" } |
    Where-Object { $_.Name -ne "RevitPlugin2026.dll" } |
    Copy-Item -Destination $pluginDir -Force

# Addin manifest goes in the Addins root, not the plugin subfolder
$addinSrc = Join-Path $repoRoot "src\RevitPlugin2026\RevitPlugin2026.addin"
Copy-Item $addinSrc $TargetPath -Force

Write-Host "Done. Restart Revit to load the updated plugin." -ForegroundColor Green
