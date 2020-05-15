$SolutionRoot = "$PSScriptRoot\.."
function Compress-Main {
    param (
        [System.IO.FileInfo]$filepath
    )

    $lines = (Get-Content $filepath.FullName |
        ForEach-Object { $_.Trim() } |
        Where-Object { $_.Length -ne 0 } 
    ).GetEnumerator()
    $sb = [System.Text.StringBuilder]::new()
    
    while ($lines.MoveNext() -and ($lines.Current.StartsWith('using') -or $lines.Current -eq '#region いつもの')) {
        $sb.AppendLine($lines.Current) | Out-Null
    }

    $sb.AppendLine() | Out-Null

    do {
        $line = $lines.Current
        $sb.Append($line) | Out-Null
        if ($line.StartsWith('#') -or $line.StartsWith('//')) {
            $sb.Append("`r`n") | Out-Null
        }
        else {
            $sb.Append(" ") | Out-Null
        }
    } while ($lines.MoveNext() -and ($lines.Current -ne '#endregion'))

    if ($lines.Current -eq '#endregion') {
        $sb.AppendLine("`r`n#endregion") | Out-Null
        while ($lines.MoveNext()) {
            $sb.AppendLine($lines.Current) | Out-Null
        }
    }
    $sb.ToString() | Out-File $filepath
}
function Compress-CSharp {
    param (
        [System.IO.FileInfo]$filepath,
        [switch]$MethodOnly
    )

    $lines = (Get-Content $filepath.FullName |
        ForEach-Object { $_.Trim() } |
        Where-Object { $_.Length -ne 0 } |
        Where-Object { -not $_.StartsWith('#') }
    ).GetEnumerator()
    $sb = [System.Text.StringBuilder]::new()
    
    while ($lines.MoveNext() -and ($lines.Current.StartsWith('using') -or $lines.Current -eq '#region いつもの')) {
        $sb.AppendLine($lines.Current) | Out-Null
    }

    1..3 | ForEach-Object { $sb.AppendLine() } | Out-Null

    do {
        $line = $lines.Current
        $si = $line.IndexOf('//')
        if ($si -eq 0) { continue }
        elseif ($si -gt 0) {
            $line = $line.Substring(0, $si)
        }

        $sb.Append($line) | Out-Null
        $sb.Append(" ") | Out-Null
    } while ($lines.MoveNext())

    if ($MethodOnly) { $sb.Insert($sb.Length - 2, "`r`n") | Out-Null }
    $sb.ToString() | Out-File $filepath
}

Compress-Main "$SolutionRoot\AtCoderProject\Main\Program.cs"

@(
    'AtCoderLib\Collection\PriorityQueue.cs',
    'AtCoderLib\Collection\Set.cs',
    'AtCoderLib\Collection\SortedCollection.cs',
    'AtCoderLib\Util\Bit.cs',
    'AtCoderLib\グラフ\重み付き\重み付きグラフ.cs',
    'AtCoderLib\グラフ\UnionFind.cs',
    'AtCoderLib\グラフ\最小共通祖先.cs',
    'AtCoderLib\グラフ\重みなしグラフ.cs',
    'AtCoderLib\整数\Mod.cs',
    'AtCoderLib\整数\有理数.cs',
    'AtCoderLib\範囲演算\累積和.cs',
    'AtCoderLib\範囲演算\累積和2D.cs',
    'AtCoderLib\文字列\SuffixArray.cs'
) | ForEach-Object { Compress-CSharp "$SolutionRoot\$_" } 
@(
    'AtCoderLib\Collection\座標圧縮.cs',
    'AtCoderLib\Collection\順列を求める.cs',
    'AtCoderLib\グラフ\重み付き\ShortestPath.cs'
) | ForEach-Object { Compress-CSharp "$SolutionRoot\$_" -MethodOnly } 
dotnet-format.exe -w "$PSScriptRoot\.."
