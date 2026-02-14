function Pause
{
    # Prompt the user to press any key before exiting
    Write-Host ""
    Write-Host "Press any key to continue..."
    $null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')
}

# Check if the current user has administrative privileges
$adminCheck = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

# If not running as administrator, relaunch the script with elevated privileges
if (-not $adminCheck) {
    Write-Host "This script requires administrative privileges." -ForegroundColor Red
    Write-Host ""
    Write-Host "Relaunching the script with elevated privileges..." -ForegroundColor Yellow
    Start-Sleep -Seconds 5

    $scriptPath = $MyInvocation.MyCommand.Path
    Start-Process pwsh.exe -Verb RunAs -ArgumentList "-NoProfile -ExecutionPolicy Bypass -File `"$scriptPath`""
    Exit
}

# Get the path to the NuGet folder relative to the script's location
$sourceDirectory = Join-Path -Path $PSScriptRoot -ChildPath "Nuget"
$destinationDirectory = Join-Path -Path $env:ProgramFiles -ChildPath "dotnet\library-packs"
$packageCacheDirectory = Join-Path -Path $env:USERPROFILE -ChildPath ".nuget\packages"

# Gets the list of packages to copy
$packageFiles = Get-ChildItem -Path $PSScriptRoot\NuGet
# The files in packageFiles has the name with the following format: "namepackage.x.x.x.nupkg", extract the package name
$packageNames = $packageFiles | ForEach-Object { $_.Name -replace '\.\d+\.\d+\.\d+\.nupkg', '' }

# Removes each of the packages to copy from the destination directory
Write-Host "Removing packages from local library..."
$packageNames | ForEach-Object {
    $package = $_
    $packagePath = Join-Path -Path $destinationDirectory -ChildPath "$package.*.*"

    if (Test-Path -Path $packagePath) {
        Write-Host "- $packagePath" -ForegroundColor DarkGray
        Remove-Item -Path $packagePath -Force -ErrorAction SilentlyContinue
    } else {
        Write-Host "- WARNING: $packagePath does not exist." -ForegroundColor Yellow
        # Write-Warning "WARNING: $packagePath does not exist."
    }
}

# Removes the packages to copy from the cache directory
Write-Host "Removing packages from the cache directory..."
$packageNames | ForEach-Object {
    $package = $_
    $packagePath = Join-Path -Path $packageCacheDirectory -ChildPath ($package.ToLower())

    if (Test-Path -Path $packagePath) {
        Write-Host "- $packagePath" -ForegroundColor DarkGray
        Remove-Item -Path $packagePath -Recurse -Force
    } else {
        Write-Host "- WARNING: $packagePath does not exist." -ForegroundColor Yellow
        # Write-Warning "WARNING: $packagePath does not exist."
    }
}

# Check if the source directory exists
Write-Host "Copying new packages..."
if (Test-Path $sourceDirectory) {
    # Check if the destination directory exists, if not, create it
    if (-not (Test-Path $destinationDirectory)) {
        New-Item -ItemType Directory -Path $destinationDirectory -Force
    }

    # Copy the NuGet packages to the destination directory, overriding any existing ones
    Copy-Item -Path $sourceDirectory\* -Destination $destinationDirectory -Force -Recurse

    Write-Host "NuGet packages copied successfully to $destinationDirectory." -ForegroundColor Green
} else {
    Write-Host "Source directory '$sourceDirectory' not found." -ForegroundColor Yellow
}

Pause