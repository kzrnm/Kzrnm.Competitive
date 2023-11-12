---
title: Expand Test
documentation_of: ./Expand.Test.csproj
display: no-index
---

## 概要

SourceExpander で埋め込んだソースを展開したときにコンパイルできるか確認します。

特に [Competitive.Library/+Runner/Program.cs]({{'/Competitive.Library/+Runner/Program.cs' | relative_url }}) は `partial` で定義して展開して完成するので変に書き換えるとコンパイルできなくなってしまいます。

## Program.cs

<div class="code">
<pre class="hljs" id="code-body-1"><code class="language-cs">namespace SourceExpander.Testing;
partial class Program
{
    const bool __ManyTestCases = false;
    static void Main() => new Program(new(), new()).Run();
    private SourceExpander.Testing.Kzrnm.Competitive.ConsoleOutput? Calc()
    {
        return 1;
    }
}
</code></pre>
    <div class="btn-area">
        <div class="btn-group"><button type="button" class="code-btn code-copy-btn hint--top hint--always hint--disable" aria-label="Copied!">Copy</button></div>
    </div>
</div>
