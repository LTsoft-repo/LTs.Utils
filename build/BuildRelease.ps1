function Pause
{
    # Prompt the user to press any key before exiting
    Write-Host ""
    Write-Host "Press any key to continue..."
    $null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')
}

# Enable error handling
$ErrorActionPreference = "Stop"

# Find solution file
$solutionFile = Get-ChildItem -Path "./../src" -Filter "*.sln" -File | Select-Object -ExpandProperty FullName

if (-not $solutionFile) {
    Write-Host "Solution file not found!" -ForegroundColor Red
    exit 1
}

Write-Host -NoNewline "Detected solution file: " -ForegroundColor Green
Write-Host $solutionFile

# Detect NuGet projects
$projectFiles = Get-ChildItem -Path "./../src" -Recurse -Filter "*.csproj" -File
$nugetProjects = @()

foreach ($projectFile in $projectFiles) {
    $projectXml = (Select-Xml -Path $projectFile.FullName -XPath /).Node | Where-Object { $_ }
    $packageId = $projectXml.Project.PropertyGroup.PackageId | Where-Object { $_ }
    $packageVersion = $projectXml.Project.PropertyGroup.Version | Where-Object { $_ }

    if ($packageId) {
        $nugetProjects += [PSCustomObject]@{`
            id = $packageId;`
            project = $projectFile.FullName;`
            version = $packageVersion`
        }
    }
}

if (-not $nugetProjects) {
    Write-Host "No NuGet projects found!" -ForegroundColor Red
    exit 1
}

Write-Host "Detected NuGets:" -ForegroundColor Green
foreach ($nugetProject in $nugetProjects) {
    Write-Host "  - $($nugetProject.id) ($($nugetProject.version))"
}

# Cleanup previous packages
if (Test-Path .\NuGet) {
    Write-Host "Deleting old packages..."
    Get-ChildItem .\NuGet\*.nupkg | foreach ($_) { remove-item $_.fullname }
}

# Build solution
Write-Host "Cleaning solution..."
& .\..\CleanSolution.ps1 -NoPause

Write-Host ""
Write-Host "Building solution..." -ForegroundColor Blue
dotnet build $solutionFile -c Release
Write-Host "Solution built!" -ForegroundColor Green

# Build NuGet packages
foreach ($nugetProject in $nugetProjects) {
    Write-Host ""
    Write-Host "Creating package: $($nugetProject.id)" -ForegroundColor Blue
    Write-Host "Project file:  $($nugetProject.project)" -ForegroundColor DarkGray
    Write-Host "Package version: $($nugetProject.version)" -ForegroundColor DarkGray

    $projectFile = $nugetProject.project
    $packageName = $nugetProject.id
    $packageVersion = $nugetProject.version
	
    $publishedVersion = (Find-Package $packageName -Source https://api.nuget.org/v3/index.json -ProviderName NuGet -erroraction 'silentlycontinue').Version ?? "package not published"
    Write-Host "Last published version: $publishedVersion" -ForegroundColor DarkGray

    if ($packageVersion -eq $publishedVersion ) {
        Write-Host "Already published. SKIPPING." -ForegroundColor Yellow
    }
    else {
        Write-Host "Building package..."
        dotnet pack $projectFile -o .\NuGet -c=Release
    }
}

Write-Host ""
Write-Host "Build completed!" -ForegroundColor Green

Pause