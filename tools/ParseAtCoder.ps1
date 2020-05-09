using namespace System.Collections.Generic;

param(
    [Parameter(Mandatory = $true)]
    [string]
    $Url
)

$ErrorActionPreference = "Stop"
Add-Type -Path "$PSScriptRoot\DLL\AngleSharp.dll"
Add-Type -AssemblyName System.Windows.Forms
function Get-Parsed-AtCoder {
    $CookieFile = "$PSScriptRoot\cookie.txt"
    $Cookie = Get-Content $CookieFile

    [string]$html = (Invoke-WebRequest -Uri $Url -Headers @{"Cookie" = $Cookie; }).Content

    return [AngleSharp.Html.Parser.HtmlParser]::new().ParseDocument($html)
}

function Get-Parsed-InOut {
    param(
        [Parameter(Mandatory = $true)]
        [AngleSharp.Html.Dom.IHtmlDocument]
        $document
    )
    
    $parts = $document.GetElementById("task-statement").
    GetElementsByClassName("lang-ja")[0].
    GetElementsByClassName("part");
    $inputList = [List[string]]::new()
    $outputList = [List[string]]::new()

    foreach ($part in $parts) {
        $h3 = $part.GetElementsByTagName("h3")[0];
        $list = $null
    
        if ($h3.TextContent -match "入力例 (\d+)") {
            $list = $inputList;
        }
        elseif ($h3.TextContent -match "出力例 (\d+)") {
            $list = $outputList;
        }

        if ($null -ne $list) {
            $list.Add($part.GetElementsByTagName("pre")[0].TextContent);
        }
    }

    [System.Linq.Enumerable]::Zip($inputList, $outputList)
}

function Update-InOut {
    param(
        [Parameter(Mandatory = $true)]
        [ValueTuple[string, string][]]
        $inouts
    )
    

    $inoutXmlPath = "$PSScriptRoot\..\AtCoderProject\Tests\InOut.resx"
    $writer = [System.Resources.ResXResourceWriter]::new($inoutXmlPath)
    try {
        $i = 0
        $inouts | ForEach-Object {
            $in = $_.Item1
            $out = $_.Item2
            [char]$c = [int][char]'A' + $i
            $writer.AddResource("${c}_IN", $in)
            $writer.AddResource("${c}_OUT", $out)
            $i++
        }
    }
    finally {
        $writer.Close()
    }
}
function Wait {
    Start-Sleep -Seconds 2    
}

$document = (Get-Parsed-AtCoder)
Wait &
Update-InOut (Get-Parsed-InOut $document) &