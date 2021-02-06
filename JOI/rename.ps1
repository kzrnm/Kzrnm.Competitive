$ErrorActionPreference="Stop"
function Rename {
    param (
        [System.IO.FileInfo]$file
    )
    $dir = $file.Directory
    $newName = ($file.Name + "." + $dir.Name)
    Rename-Item -Path $file -NewName $newName
    Move-Item ($file.DirectoryName + "/$newName") -Destination $dir.Parent
}

$files = (Get-ChildItem -Directory "$PSScriptRoot\*\*" | Get-ChildItem -File -Recurse)
$files | ForEach-Object {
    Rename $_
}
wsl.exe bash -c "find -type d | xargs rmdir"