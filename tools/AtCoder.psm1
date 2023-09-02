using namespace System.Collections.Generic;
if (-not (Test-Path "$PSScriptRoot/config.json")) {
    throw "Requied: $PSScriptRoot/config.json"
}

$defaultLangId = 5042 # AOT じゃない方は 5003
$config = (Get-Content "$PSScriptRoot/config.json" | ConvertFrom-Json)

function loadingDll {
    Add-Type -AssemblyName System.Windows.Forms
    Add-Type -Path "$($config.AtCoderStreakPath)/AtCoderStreak.dll"
    Add-Type -Path "$($config.AtCoderStreakPath)/AngleSharp.dll"
}
loadingDll
$streak = [AtCoderStreak.Program]::GetDefault()


#region Parse AtCoder
function Get-Parsed-AtCoder {
    $Cookie = (Get-Content $config.CookieFile)
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

    $inoutXmlPath = $config.Project.InOutPath
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
    [string]ToDefineInit() {
        return "int $($this.name) = cr;"
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
    [string]ToDefineInit() {
        return "int[] $($this.name) = cr.Repeat($($this.length));"
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
    [string]ToDefineInit() {
        return "var $($this.name) = cr.Repeat($($this.length1)).Select(cr => cr.Repeat($($this.length2)).Int);"
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
    [string]ToDefineInit() {
        return "var grid = cr.Repeat($($this.length)).Ascii;"
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
    [string]ToDefineInit() {
        return "var $($this.varname) = cr.Repeat($($this.length)).Select<$($this.typename)>(cr => ($(@('cr')*$this.names.Length -join ',')));"
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
            ForEach-Object Value |
            ForEach-Object { $_.Trim() })
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
            $m2 = $ml[2]
            if ($line.Length -gt 1) {
                $line[$line.Length - 1] -match '^(.+?)_(.+)$' | Out-Null
                $ml2 = $Matches
                $mm2 = $ml2[2]
                if ($ml[1] -eq $ml2[1]) {
                    # 1行すべて同じ文字

                    # 2次元
                    if ($mm2 -match '^\{(\D)(\D)\}$') {
                        [ATArray2]::new($ml2[1], $Matches[1], $Matches[2])
                    }
                    elseif ($mm2 -match '^\{(\D),\s*(\D)\}$') {
                        [ATArray2]::new($ml2[1], $Matches[1], $Matches[2])
                    }
                    elseif ($mm2 -match '^\{.+,\s*.+\}$') {
                    }
                    # 1次元
                    elseif ($mm2 -match '\{(.+)\}') {
                        [ATArray]::new($ml2[1], $Matches[1])
                    }
                    else {
                        [ATArray]::new($ml2[1], $mm2)
                    }
                }
                elseif ($m2 -notmatch '^\d+$') {
                    # 非数値の添字が来るまで回す
                    $ms = ($line | ForEach-Object { $_ -match '^(.+?)_(.+)$' | Out-Null; $Matches })
                    if ($ms.Length -eq [HashSet[string]]::new([string[]]($ms | ForEach-Object { $_[1] })).Count -and [HashSet[string]]::new([string[]]($ms | ForEach-Object { $_[2] })).Count -eq 1) {
                        # 1次元タプル配列っぽい
                        if ($m2 -match '\{(\D.*)\}') {
                            $l = $Matches[1]
                            [ATTuples]::new(($ms | Foreach-Object { $_[1] }) , $l)
                        }
                        elseif (-not $m2.Contains('_')) {
                            [ATTuples]::new(($ms | Foreach-Object { $_[1] }) , $m2)
                        }
                    }
                }
            }
            elseif ($m2 -notmatch '^\d+$') {
                if ($m2.Contains('{')) {
                    # 要素が1つなのに2次元っぽいならたぶんグリッド
                    $sp = ($m2 -split ' ')
                    $spl = $sp[$sp.Length - 1]
                    if ($spl -match '^.+_\{(\D)\D\}$') {
                        [ATGrid]::new($Matches[1])
                    }
                }
                else {
                    # 添字ありだが1行に1つなら、たぶん配列ということにする
                    [ATArray]::new($ml[1], $m2)
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
    elseif ($text -match '10\^\{9\} ?\+ ?7') {
        return "1000000007"
    }
    return ""
}
function Update-Input {
    param(
        [Parameter(Mandatory = $true)]
        [AngleSharp.Html.Dom.IHtmlDocument]
        $document,
        [Parameter(Mandatory = $false)]
        [bool]
        $StaticField = $false
    )
    $vars = (Get-Parsed-Input $document)
    if (-not $vars) {
        $vars = @()
    }
    $modInt = (Get-ModInt $document)

    $indent = "    "
    $mainPath = $config.Project.ProgramPath
    $main = (Get-Content $mainPath -Raw)
    if ($modInt) {
        $main = ($main -replace '(using ModInt[\S]*) ?= ?([^<]+)+[^;]+;', ('$1 = $2' + "<AtCoder.Mod$modInt>;"))
    }
    $main = ($main -replace 'const bool __ManyTestCases = true', 'const bool __ManyTestCases = false')
    ($main -replace 'ConsoleOutput\? Calc\((.*)\)[\s\S]*', ('ConsoleOutput? Calc($1)')) > $mainPath
    "$indent{" >> $mainPath
    $vars | ForEach-Object { $indent * 2 + ($StaticField ? $_.ToInit() : $_.ToDefineInit()) } >> $mainPath
    "$indent${indent}return null;" >> $mainPath
    "$indent}" >> $mainPath
    if ($StaticField) {
        $vars | ForEach-Object { $indent + "public static " + $_.ToDefine() } >> $mainPath
    }
    "}" >> $mainPath
}

function ParseAtCoder {
    param(
        [Parameter(Mandatory = $true)]
        [string]
        $Url,
        [Parameter(Mandatory = $false)]
        [switch]
        $StaticField
    )

    Set-Variable -Name "lastAtCoderUrl" -Value $Url -Scope Script
    $document = (Get-Parsed-AtCoder)
    Update-InOut (Get-InOut $document)
    Update-Input $document -StaticField $StaticField
}
Set-Alias at "ParseAtCoder"
#endregion Parse AtCoder

function streak {
    param (
        [string]$Url,
        [Parameter(Mandatory = $false)][System.IO.FileInfo]$File,
        [Parameter(Mandatory = $false)][int]$langId = $defaultLangId, # AOT じゃない方は 5003
        [Parameter(Mandatory = $false)][int]$priority = 0
    )
    if (-not $File.Exists) {
        $file = (Get-ChildItem $config.Project.CombinedPath)
    }
    if ($file.Exists) {
        $filePath = $file.FullName
        if (-not $Url) {
            $Url = $lastAtCoderUrl
        }
        if (-not $Url) {
            throw "url is Empty"
        }
        $streak.AddInternal($Url, "$langId", $filePath, $priority) | Out-Null
    }
    else {
        throw "$filePath doesn't Exist"
    }
}

function Get-Source {
    param (
        [Parameter(Mandatory = $false)][switch]$PathThru
    )
    $result = $streak.GetSources() | Sort-Object @{Expression = "priority"; Descending = $false }, @{Expression = "id"; Descending = $true }

    if ($PathThru) {
        return $result
    }
    else {
        $result | Select-Object -ExcludeProperty SourceCode
    }
}

function Remove-Source {
    $ids = [int[]]$args
    $streak.DeleteInternal($ids)
}
function Restore-Source {
    param (
        [Parameter(Mandatory = $false, Position = 0)][int]$id = -1,
        [Parameter(Mandatory = $false)][string]$filePath = $null,
        [Parameter(Mandatory = $false)][string]$url = $null
    )
    if (-not $filePath) {
        $filePath = $config.Project.CombinedPath
    }

    if ($id -ge 0) {
        $saved = $streak.RestoreInternal($id, $null)
    }
    else {
        if (-not $url) {
            $url = $lastAtCoderUrl
        }
        if (-not $url) {
            throw "url and id is Empty"
        }
        
        $saved = $streak.RestoreInternal(-1, $url)
    }
    if ($saved) {
        $saved.SourceCode | Out-File -FilePath $filePath -NoNewline
    }
    else {
        throw "Not found"
    }
}

function submit-streak {
    param (
        [int]$Parallel = 6 # デフォルト 6並列
    )
    $streak.SubmitInternal([AtCoderStreak.Service.SourceOrder]::None, $true, $Parallel, $config.CookieFile).Result | Out-Null
}

function submit {
    param (
        [string]$url,
        [string]$filePath,
        [int]$langId = $defaultLangId,
        [switch]$SilentResult
    )
    if (-not $filePath) {
        $filePath = $config.Project.CombinedPath
    }
    if (-not $url) {
        $url = $lastAtCoderUrl
        Write-Host -ForegroundColor DarkGreen "[Submit]: $url"
    }
    if (-not $url) {
        throw "url is Empty"
    }
    
    $streak.SubmitFileInternal((Get-Content -Raw $filePath), $url, "$langId", "$($config.CookieFile)").Result | Out-Null
    if (-not $SilentResult) {
        Start-Process ($url -replace 'tasks/.*', 'submissions/me')
    }
}

function Restore-MainProgram {
    $prog = "$PSScriptRoot/../Competitive/Program.cs"
    git update-index --no-assume-unchanged $prog
    git restore $prog
}
function Add-MainProgram {
    $prog = "$PSScriptRoot/../Competitive/Program.cs"
    git add $prog
    git update-index --assume-unchanged $prog
}

Export-ModuleMember  -Function @(
    "streak",
    "ParseAtCoder",
    "Get-Source",
    "Remove-Source",
    "Restore-Source",
    "Restore-MainProgram",
    "Add-MainProgram",
    "submit-streak",
    "submit"
) -Alias "at"
