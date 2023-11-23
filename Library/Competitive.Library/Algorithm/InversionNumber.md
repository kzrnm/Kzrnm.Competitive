---
title: 転倒数
documentation_of: ./InversionNumber.cs
---

## 概要

転倒数: $i &lt; j$ かつ $s_i &gt; s_j$ を満たす組み合わせの個数

0-based な `int[]` ならば、最大値が配列長 **未満** なら良いのだが、競プロ用途で1個余裕を持って **以下** まで対応しておきます。

`FenwickTree<T>` を使って $O(N \log N)$ で求められます。

### 使い方

- `Inversion<T>(T[])`: 座標圧縮して任意の `IComparable<T>` 型の転倒数を返します。
- `Inversion(int[])`: 転倒数を返します。渡される配列は座標圧縮されて最大値が配列長以下になっている必要があります。
