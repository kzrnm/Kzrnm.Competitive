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
    try {
        [string]$html = (Invoke-WebRequest -Uri $Url -Headers @{"Cookie" = $Cookie; }).Content
        return [AngleSharp.Html.Parser.HtmlParser]::new().ParseDocument($html)
    }
    catch {
        Write-Error $_.Exception.Message
        throw
    }
}

function Get-InOut {
    param(
        [Parameter(Mandatory = $true)]
        [AngleSharp.Html.Dom.IHtmlDocument]
        $document
    )
    
    $parts = $document.GetElementById("task-statement").
    GetElementsByClassName("part");
    $inputList = [List[string]]::new()
    $outputList = [List[string]]::new()

    foreach ($part in $parts) {
        $h3 = $part.GetElementsByTagName("h3")[0];
        $list = $null
    
        if ($h3.TextContent -match "入力例 *(\d+)") {
            $list = $inputList;
        }
        elseif ($h3.TextContent -match "出力例 *(\d+)") {
            $list = $outputList;
        }

        if ($null -ne $list) {
            $list.Add($part.GetElementsByTagName("pre")[0].TextContent);
        }
    }

    return [System.Linq.Enumerable]::Zip($inputList, $outputList)
}

function Update-InOut {
    param(
        [Parameter(Mandatory = $true)]
        [ValueTuple[string, string][]]
        $inouts
    )

    $inoutXmlPath = "$PSScriptRoot\..\Competitive.Runner\InOut.resx"
    $writer = [System.Resources.ResXResourceWriter]::new($inoutXmlPath)
    try {
        for ($i = 0; $i -lt 6; $i++) {
            if ($i -lt $inouts.Length) {
                $in = $inouts[$i].Item1
                $out = $inouts[$i].Item2
            }
            else {
                $in = $out = ''
            }
            [char]$c = [int][char]'A' + $i
            $writer.AddResource("${c}_IN", $in)
            $writer.AddResource("${c}_OUT", $out)
        }
    }
    finally {
        $writer.Close()
    }
}

class ATVariable {
    [string]$name
    ATVariable([string]$name) {
        $this.name = $name
    }

    [string]ToInit() {
        return "$($this.name) = cr;"
    }
    [string]ToDefine() {
        return "int $($this.name);"
    }
}
class ATArray {
    [string]$name
    [string]$length
    ATArray([string]$name, [string]$length) {
        $this.name = $name
        $this.length = $length
    }

    [string]ToInit() {
        return "$($this.name) = cr.Repeat($($this.length));"
    }
    [string]ToDefine() {
        return "int[] $($this.name);"
    }
}
class ATArray2 {
    [string]$name
    [string]$length1
    [string]$length2
    ATArray2([string]$name, [string]$length1, [string]$length2) {
        $this.name = $name
        $this.length1 = $length1
        $this.length2 = $length2
    }

    [string]ToInit() {
        return "$($this.name) = cr.Repeat($($this.length1)).Select(cr => cr.Repeat($($this.length2)).Int);"
    }
    [string]ToDefine() {
        return "int[][] $($this.name);"
    }
}
class ATGrid {
    [string]$length
    ATGrid([string]$length) {
        $this.length = $length
    }

    [string]ToInit() {
        return "grid = cr.Repeat($($this.length));"
    }
    [string]ToDefine() {
        return "string[] grid;"
    }
}
class ATTuples {
    [string[]]$names
    [string]$length
    [string]$typename
    [string]$varname
    ATTuples([string[]]$names, [string]$length) {
        $this.names = $names
        $this.length = $length
        $this.varname = $this.names -join ''
        $this.typename = "(" + (($this.names | ForEach-Object { "int $_" }) -join "," ) + ")"
    }

    [string]ToInit() {
        return "$($this.varname) = cr.Repeat($($this.length)).Select<$($this.typename)>(cr => ($(@('cr')*$this.names.Length -join ',')));"
    }
    [string]ToDefine() {
        return "$($this.typename)[] $($this.varname);"
    }
}
function Get-Parsed-Input {
    param(
        [Parameter(Mandatory = $true)]
        [AngleSharp.Html.Dom.IHtmlDocument]
        $document
    )
    
    $lines = $document.GetElementById("task-statement").
    GetElementsByClassName("part") |
    Where-Object { $_.GetElementsByTagName("h3")[0].TextContent -like "入*力" } |
    ForEach-Object { $_.GetElementsByTagName('pre')[0].InnerHtml -split "`n" } 
    for ($i = 0; $i -lt $lines.Length; $i++) {
        $line = [string[]](([Xml]"<root>$($lines[$i])</root>").GetElementsByTagName('var') | 
            ForEach-Object ChildNodes |
            ForEach-Object Value)
        if (-not $line) { continue }
        elseif ($line.Count -eq 1) {
            $line = [string[]]($line[0] -split " *\\hspace\{[^}]*\} *" | Where-Object { $_ })
        }
        $first = $line[0]
        if ($first -eq ':') { }
        elseif ($first -eq '.') { }
        elseif ($first -eq '\vdots') { }
        elseif ( $first -match '^(.+?)_(.+)$' ) {
            $ml = $Matches
            if ($line.Length -gt 1) {
                $line[$line.Length - 1] -match '^(.+?)_(.+)$' | Out-Null
                $ml2 = $Matches
                if ($ml[1] -eq $ml2[1]) {
                    # 1行すべて同じ文字

                    # 2次元
                    if ($ml2[2] -match '^\{(\D)(\D)\}$') {
                        [ATArray2]::new($ml2[1], $Matches[1], $Matches[2])
                    }
                    elseif ($ml2[2] -match '^\{(\D),(\D)\}$') {
                        [ATArray2]::new($ml2[1], $Matches[1], $Matches[2])
                    }
                    # 1次元
                    elseif ($ml2[2] -match '\{(.+)\}') {
                        [ATArray]::new($ml2[1], $Matches[1])
                    }
                    else {
                        [ATArray]::new($ml2[1], $ml2[2])
                    }
                } 
                elseif ($ml[2] -notmatch '^\d+$') {
                    # 非数値の添字が来るまで回す
                    $ms = ($line | ForEach-Object { $_ -match '^(.+?)_(.+)$' | Out-Null; $Matches })
                    if ($ms.Length -eq [HashSet[string]]::new([string[]]($ms | ForEach-Object { $_[1] })).Count -and [HashSet[string]]::new([string[]]($ms | ForEach-Object { $_[2] })).Count -eq 1) {
                        # 1次元タプル配列っぽい
                        if ($ml[2] -match '\{(\D.*)\}') {
                            $l = $Matches[1]
                            [ATTuples]::new(($ms | Foreach-Object { $_[1] }) , $l)
                        }
                        elseif (-not $ml[2].Contains('_')) {
                            [ATTuples]::new(($ms | Foreach-Object { $_[1] }) , $ml[2])
                        }
                    }
                }
            }
            elseif ($ml[2] -notmatch '^\d+$') {
                if ($ml[2].Contains('{')) {
                    # 要素が1つなのに2次元っぽいならたぶんグリッド
                    $sp = ($ml[2] -split ' ')
                    $spl = $sp[$sp.Length - 1]
                    if ($spl -match '^.+_\{(\D)\D\}$') {
                        [ATGrid]::new($Matches[1])
                    }
                }
                else {
                    # 添字ありだが1行に1つなら、たぶん配列ということにする
                    [ATArray]::new($ml[1], $ml[2]) 
                }
            }
        }
        else {
            # 添字なし
            $line | ForEach-Object { [ATVariable]::new($_) } 
        }
    }
}
function Get-ModInt {
    param(
        [Parameter(Mandatory = $true)]
        [AngleSharp.Html.Dom.IHtmlDocument]
        $document
    )
    $text = $document.Body.TextContent
    if ($text -match '998,?244,?353') {
        return '998244353'
    }
    elseif ($text -match '1,?000,?000,?007') {
        return "1000000007"
    }
    elseif ($text -match '10\^9 ?\+ ?7') {
        return "1000000007"
    }
    return ""
}
function Update-Input {
    param(
        [Parameter(Mandatory = $true)]
        [AngleSharp.Html.Dom.IHtmlDocument]
        $document
    )
    $vars = (Get-Parsed-Input $document)
    if (-not $vars) {
        $vars = @()
    }
    $modInt = (Get-ModInt $document)

    $indent = "    "
    $mainPath = "$PSScriptRoot\..\Competitive\Program.cs"
    $main = (Get-Content $mainPath -Raw)
    if ($modInt) {
        $main = ($main -replace 'using ModInt = [^;]+;', "using ModInt = AtCoder.StaticModInt<AtCoder.Mod$modInt>;")
    }
    ($main -replace 'private object Calc\(\)[\s\S]*', ("private object Calc()")) > $mainPath
    "$indent{" >> $mainPath
    $vars | ForEach-Object { $indent * 2 + $_.ToInit() } >> $mainPath
    "$indent${indent}return null;" >> $mainPath
    "$indent}" >> $mainPath
    $vars | ForEach-Object { $indent + "public static " + $_.ToDefine() } >> $mainPath
    "}" >> $mainPath
}

function Main {
    Set-Variable -Name "lastAtCoderUrl" -Value $Url -Scope Global
    $document = (Get-Parsed-AtCoder)
    Update-InOut (Get-InOut $document)
    Update-Input $document
    # dotnet-format.exe -f "$PSScriptRoot\..\Competitive"
}

Main
