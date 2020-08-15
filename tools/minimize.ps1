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
function Main {

    Compress-Main "$SolutionRoot\AtCoderProject\Main\Program.cs"

    $targetFiles = @(
        'AtCoderLib\Collection\Deque.cs',
        'AtCoderLib\Collection\PriorityQueue.cs',
        'AtCoderLib\Collection\Set.cs',
        'AtCoderLib\Collection\SortedCollection.cs',
        'AtCoderLib\Collection\Trie.cs',
        'AtCoderLib\グラフ\重み付き\重み付きグラフ.cs',
        'AtCoderLib\グラフ\重み付き\流量とコストを持つグラフ.cs',
        'AtCoderLib\グラフ\UnionFind.cs',
        'AtCoderLib\グラフ\最小共通祖先.cs',
        'AtCoderLib\グラフ\重みなしグラフ.cs',
        'AtCoderLib\整数\Bit.cs',
        'AtCoderLib\整数\Mod.cs',
        'AtCoderLib\整数\素数.cs',
        'AtCoderLib\範囲演算\SegmentTree.cs',
        'AtCoderLib\範囲演算\特殊なBIT\BinaryIndexedTree2D.cs',
        'AtCoderLib\範囲演算\特殊なBIT\RangeBinaryIndexedTree.cs',
        'AtCoderLib\範囲演算\累積和.cs',
        'AtCoderLib\範囲演算\累積和2D.cs',
        'AtCoderLib\文字列\SuffixArray.cs'
    )
    $methodTargetFiles = @(
        'AtCoderLib\Collection\座標圧縮.cs',
        'AtCoderLib\Collection\順列を求める.cs',
        'AtCoderLib\グラフ\重み付き\GraphUtil.cs'
    ) 
    $targetFiles | ForEach-Object { Compress-CSharp "$SolutionRoot\$_" } 
    $methodTargetFiles | ForEach-Object { Compress-CSharp "$SolutionRoot\$_" -MethodOnly } 
    dotnet-format.exe -w "$PSScriptRoot\.." --files (($methodTargetFiles + $targetFiles) -join ',')
}

Main
