[CmdletBinding()]
param(
    [Parameter(Mandatory=$True)]
    [string]$SolutionPath
)

$orchardWebPath = Get-Item (Join-Path $SolutionPath "\src\Orchard.Web")
    
if ((Get-ChildItem $orchardWebPath -Directory | ? { $_.Name -in "Themes", "Modules" }).Length -ne 2)
{
    "The target directory doesn't seem to be an Orchard solution directory."
    exit 1;
}

"Modules", "Themes" | % {

    $projectFolderName = $_
    $projectFolderPath = Join-Path $orchardWebPath $projectFolderName

    Get-ChildItem $projectFolderName -Directory | % {
        
        $sourceFullName = $_.FullName
        $targetFullName = Join-Path $projectFolderPath $_.Name
        
        cmd /c mklink /J "$targetFullName" "$sourceFullName"
    }
}