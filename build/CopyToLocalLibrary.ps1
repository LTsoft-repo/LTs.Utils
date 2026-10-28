function Pause
{
    # Prompt the user to press any key before exiting
    Write-Host ""
    Write-Host "Press any key to continue..."
    $null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')
}

function Get-LocalNuGetsDirectory
{
    param(
        [string] $StartDirectory
    )

    $directory = $StartDirectory
    while ($directory) {
        foreach ($configName in @('nuget.config', 'NuGet.Config', 'NuGet.config')) {
            $configPath = Join-Path $directory $configName
            if (-not (Test-Path -LiteralPath $configPath)) {
                continue
            }

            [xml] $config = Get-Content -LiteralPath $configPath
            $localSource = $config.configuration.packageSources.add |
                Where-Object { $_.key -eq 'localNuGets' } |
                Select-Object -First 1

            if ($localSource -and $localSource.value) {
                $sourcePath = $localSource.value
                if (-not [System.IO.Path]::IsPathRooted($sourcePath)) {
                    $sourcePath = Join-Path $directory $sourcePath
                }

                return [System.IO.Path]::GetFullPath($sourcePath)
            }
        }

        $parent = Split-Path $directory -Parent
        if (-not $parent -or $parent -eq $directory) {
            break
        }

        $directory = $parent
    }

    throw "Could not find a 'localNuGets' package source in nuget.config. Walked up from $StartDirectory."
}

$ErrorActionPreference = 'Stop'

# Get the path to the NuGet folder relative to the script's location
$sourceDirectory = Join-Path -Path $PSScriptRoot -ChildPath 'NuGet'
$destinationDirectory = Get-LocalNuGetsDirectory -StartDirectory $PSScriptRoot
$packageCacheDirectory = Join-Path -Path $env:USERPROFILE -ChildPath '.nuget\packages'

Write-Host "Local NuGet feed: $destinationDirectory" -ForegroundColor Cyan

# Gets the list of packages to copy
$packageFiles = Get-ChildItem -Path $sourceDirectory -Filter '*.nupkg' -ErrorAction SilentlyContinue
if (-not $packageFiles) {
    Write-Host "No NuGet packages found in '$sourceDirectory'. Run BuildRelease.ps1 first." -ForegroundColor Yellow
    Pause
    exit 1
}

# The files in packageFiles has the name with the following format: "namepackage.x.x.x.nupkg", extract the package name
$packageNames = $packageFiles | ForEach-Object { $_.Name -replace '\.\d+\.\d+\.\d+.*\.nupkg$', '' }

# Removes each of the packages to copy from the destination directory
Write-Host "Removing packages from local library..."
$packageNames | ForEach-Object {
    $package = $_
    $existingPackages = Get-ChildItem -Path $destinationDirectory -Filter "$package.*.nupkg" -ErrorAction SilentlyContinue

    if ($existingPackages) {
        $existingPackages | ForEach-Object {
            Write-Host "- $($_.FullName)" -ForegroundColor DarkGray
            Remove-Item -LiteralPath $_.FullName -Force
        }
    } else {
        Write-Host "- WARNING: No existing package found for $package in $destinationDirectory." -ForegroundColor Yellow
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
    }
}

# Check if the source directory exists
Write-Host "Copying new packages..."
if (Test-Path $sourceDirectory) {
    if (-not (Test-Path $destinationDirectory)) {
        New-Item -ItemType Directory -Path $destinationDirectory -Force | Out-Null
    }

    Copy-Item -Path (Join-Path $sourceDirectory '*') -Destination $destinationDirectory -Force

    Write-Host "NuGet packages copied successfully to $destinationDirectory." -ForegroundColor Green
} else {
    Write-Host "Source directory '$sourceDirectory' not found." -ForegroundColor Yellow
}

Pause
