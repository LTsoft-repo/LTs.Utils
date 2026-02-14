# Use:
#   .\CleanSolution.ps1
#   .\CleanSolution.ps1 -NoPause

param (
    [switch]$NoPause
)

function Pause
{
    # Prompt the user to press any key before exiting
    Write-Host ""
    Write-Host "Press any key to continue..."
    $null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')
}

# Determine the base directory of the script
$scriptDirectory = $PSScriptRoot

# Define the folders to remove and ignore
$foldersToRemove = 'bin','obj'
[string[]]$foldersToIgnore = 'node_module'

# Clean up the specified folders
Get-ChildItem $scriptDirectory -Include $foldersToRemove -Recurse | Where-Object { $_.FullName -inotmatch "\\$($foldersToIgnore -join '|')\\" } | Remove-Item -Force -Recurse

Write-Host "Clean up complete!" -ForegroundColor Green

if (-not $NoPause) {
    Pause
}