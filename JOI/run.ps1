param (
    [Parameter(Mandatory = $true, Position = 0)]
    [string]
    $Directory,

    [Parameter()]
    [switch]
    $Time,

    [Parameter()]
    [switch]
    $Diff,

    [Parameter()]
    [switch]
    $Release,

    [Parameter()]
    [switch]
    $Build
)
$target = 'Debug'
if ($Release) {
    $target = 'Release'
    $Build = $true
}
if ($Build) {
    dotnet build -c $target ../ --nologo -v q
}

$files = (Get-ChildItem $Directory | Where-Object { $_.Name.Contains("-in") -or $_.Name.Contains(".in") })

foreach ($file in $files) {
    $mc = Measure-Command { 
        $commandRes = (& ../bin/$target/netcoreapp3.1/Competitive.exe $file).Trim([char]65279)
    }

    if ($Diff) {
        Write-Host "$file`t" -ForegroundColor DarkBlue -NoNewline
        $dir = $file.DirectoryName
        $outName = $file.Name.Replace("-in", "-out").Replace(".in", ".out")
        $outPath = Join-Path $dir $outName
        $d = ($commandRes | C:\Software\vim\diff.exe -u $outPath -)
        if ($d) {
            Write-Host ""
            Write-Output $d
        }
    }
    else {
        Write-Host "$commandRes`t" -NoNewline
    }

    if ($Time) {
        Write-Host "$($mc.TotalMilliseconds) ms" -ForegroundColor DarkGreen
    }
    else {
        Write-Host ""
    }
}