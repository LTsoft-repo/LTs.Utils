function Pause
{
    # Prompt the user to press any key before exiting
    Write-Host ""
    Write-Host "Press any key to continue..."
    $null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')
}

$ErrorActionPreference = "Stop"
$timeOut = 20
$maxRetries = 5
$delaySeconds = 1

$nugetSource = "githubLTsoft"

$packages = Get-ChildItem .\NuGet\*.nupkg
if ($packages.Count -eq 0) {
    Write-Host "No NuGet packages found in the specified directory." -ForegroundColor Yellow
    Pause
    exit
}

Write-Host ( "Packages found: " + $packages.Count ) -ForegroundColor Green

# dotnet nuget push $package --source $nugetSource --timeout $timeOut --skip-duplicate > $null
# Write-Host "    Result: $($LASTEXITCODE)" -ForegroundColor Cyan


# Publish each package found in the directory
Get-ChildItem .\NuGet\*.nupkg | ForEach-Object ($_) `
{
    $package = $_
    Write-Host ( "Publishing package: " + $package.Name ) -ForegroundColor Blue

    $attempt = 0
    $success = $false

    while ($attempt -lt $maxRetries -and -not $success) {
        $attempt++
        dotnet nuget push $package --source $nugetSource --timeout $timeOut --skip-duplicate

        if ($LASTEXITCODE -eq 0) {
            Write-Host "Push succeeded on attempt #$attempt." -ForegroundColor Green
            $success = $true
        }
        else {
            Write-Warning "Push failed (exit code $LASTEXITCODE)."
            if ($attempt -lt $maxRetries) {
                Write-Host "  Waiting $delaySeconds seconds before retry…" -ForegroundColor DarkYellow
                Start-Sleep -Seconds $delaySeconds
            }
        }
    }

    if (-not $success) {
        Write-Error "All $maxRetries attempts failed for package $($package.Name)."
        exit $LASTEXITCODE
    }

    Write-Host ""
}

Write-Host ""
Write-Host "Packages published!" -ForegroundColor Green

Pause