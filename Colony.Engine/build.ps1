[CmdletBinding()]
param(
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Debug",

    [switch]$SkipTests,

    [switch]$Clean,

    [switch]$LaunchUnity
)

$ErrorActionPreference = "Stop"

# ------------------------------------------------------------
# Configuration
# ------------------------------------------------------------

$Solution = Join-Path $PSScriptRoot "Colony.Engine.sln"

$UnityPlugins = Join-Path $PSScriptRoot "..\Colony.Unity\Assets\Plugins\Colony"

#Add projects to this array to have their assemblies copied to the Unity plugin folder
$Projects = @(
    "Colony.Engine"
)

# ------------------------------------------------------------
# Helper Functions
# ------------------------------------------------------------

function Write-Title($Message)
{
    Write-Host ""
    Write-Host "====================================================" -ForegroundColor Cyan
    Write-Host " $Message" -ForegroundColor Cyan
    Write-Host "====================================================" -ForegroundColor Cyan
}

function Write-Step($Message)
{
    Write-Host ""
    Write-Host "> $Message" -ForegroundColor Yellow
}

function Write-Success($Message)
{
    Write-Host "[OK] $Message" -ForegroundColor Green
}

function Write-Failure($Message)
{
    Write-Host "[FAIL] $Message" -ForegroundColor Red
}

function Invoke-Step
{
    param(
        [string]$Name,
        [scriptblock]$Action
    )

    Write-Step $Name

    try
    {
        & $Action

        if ($LASTEXITCODE -ne 0)
        {
            throw "Command failed."
        }

        Write-Success $Name
    }
    catch
    {
        Write-Failure $Name
        throw
    }
}

# ------------------------------------------------------------
# Header
# ------------------------------------------------------------

Clear-Host

Write-Title "Colony Build System"

Write-Host "Configuration : $Configuration"
Write-Host "Solution      : $Solution"
Write-Host "Unity         : $UnityPlugins"

$Stopwatch = [System.Diagnostics.Stopwatch]::StartNew()

# ------------------------------------------------------------
# Clean
# ------------------------------------------------------------

if ($Clean)
{
    Invoke-Step "Cleaning solution" {

        dotnet clean `
            $Solution `
            --configuration $Configuration

    }
}

# ------------------------------------------------------------
# Restore
# ------------------------------------------------------------

Invoke-Step "Restoring packages" {

    dotnet restore $Solution

}

# ------------------------------------------------------------
# Build
# ------------------------------------------------------------

Invoke-Step "Building solution" {

    dotnet build `
        $Solution `
        --configuration $Configuration `
        --no-restore

}

# ------------------------------------------------------------
# Tests
# ------------------------------------------------------------

if (-not $SkipTests)
{
    Invoke-Step "Running tests" {

        dotnet test `
            $Solution `
            --configuration $Configuration `
            --no-build

    }
}

# ------------------------------------------------------------
# Prepare Unity
# ------------------------------------------------------------

Invoke-Step "Preparing Unity plugin folder" {

    if (!(Test-Path $UnityPlugins))
    {
        New-Item `
            -ItemType Directory `
            -Path $UnityPlugins | Out-Null
    }

    Remove-Item "$UnityPlugins\*.dll" `
        -Force `
        -ErrorAction SilentlyContinue

    Remove-Item "$UnityPlugins\*.pdb" `
        -Force `
        -ErrorAction SilentlyContinue

}

# ------------------------------------------------------------
# Copy Assemblies
# ------------------------------------------------------------

Invoke-Step "Copying assemblies" {

    foreach ($Project in $Projects)
    {
        $Output = Join-Path `
            $PSScriptRoot `
            "$Project\bin\$Configuration"

        Get-ChildItem `
            $Output `
            -Filter *.dll `
            -Recurse |
                Copy-Item `
            -Destination $UnityPlugins `
            -Force

        Get-ChildItem `
            $Output `
            -Filter *.pdb `
            -Recurse |
                Copy-Item `
            -Destination $UnityPlugins `
            -Force
    }

}

# ------------------------------------------------------------
# Launch Unity
# ------------------------------------------------------------

if ($LaunchUnity)
{
    Invoke-Step "Launching Unity" {

        Start-Process `
            "..\Unity\Colony.Unity"

    }
}

# ------------------------------------------------------------
# Finish
# ------------------------------------------------------------

$Stopwatch.Stop()

Write-Host ""
Write-Host "====================================================" -ForegroundColor Green
Write-Host " BUILD SUCCEEDED" -ForegroundColor Green
Write-Host "====================================================" -ForegroundColor Green

Write-Host ""
Write-Host ("Elapsed : {0:N2} sec" -f $Stopwatch.Elapsed.TotalSeconds)
Write-Host ""