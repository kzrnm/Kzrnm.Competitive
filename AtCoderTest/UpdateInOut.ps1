param([Parameter(Mandatory = $true)][string]$Url, [Parameter()][string]$Cookie, [Parameter()][string]$CookieFile)

$ErrorActionPreference="Stop"

[Reflection.Assembly]::LoadFrom("$PSScriptRoot\AngleSharp.dll") | Out-Null

if (-not $Cookie) { 
    if (-not $CookieFile) {
        $CookieFile = "$PSScriptRoot\cookie.txt"
    }
    $Cookie = Get-Content $CookieFile
}

[string]$html = (Invoke-WebRequest -Uri $Url -Headers @{"Cookie" = $Cookie; }).Content

$document = [AngleSharp.Html.Parser.HtmlParser]::new().ParseDocument($html)
$taskStatement = $document.GetElementById("task-statement");
$ja = $taskStatement.GetElementsByClassName("lang-ja")[0];
$parts = $ja.GetElementsByClassName("part");
$inputList = [System.Collections.Generic.List[string]]::new()
$outputList = [System.Collections.Generic.List[string]]::new()

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

$inoutXmlPath = "$PSScriptRoot\InOut.resx"
$inoutXml = [Xml](Get-Content $inoutXmlPath)
$datas = ($inoutXml.GetElementsByTagName("data") | Sort-Object -property @{Expression = { $_.Attributes["name"].Value } })

foreach ($data in $datas) {
    if ($data -isnot [System.Xml.XmlElement]) { continue }
    $name = $data.Attributes["name"].Value;
    if ($name -match "(.)_IN") {
        $c = [char]$Matches[1]
        $i = $c - [char]'A'
        $v = $inputList[$i]
        if (-not $v) { $v = "" }
    }
    elseif ($name -match "(.)_OUT") {
        $c = [char]$Matches[1]
        $i = $c - [char]'A'
        $v = $outputList[$i]
        if (-not $v) { $v = "" }
    }
    else { continue }
    $valueNode = $data.GetElementsByTagName("value")[0];
    $valueNode.InnerText = $v.TrimEnd()
}

$inoutXml.Save($inoutXmlPath)