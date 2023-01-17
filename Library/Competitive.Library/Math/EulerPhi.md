---
title: オイラーの $\phi$ 関数
documentation_of: ./EulerPhi.cs
---

## 概要

$n$ 以下の自然数で $n$ と互いに素なものの個数 $\phi(n)$。

$\phi(n)=n\displaystyle\prod_{i=1}^k(1-\dfrac{1}{p_i})$ (ただし $p_i$ は $n$ の素因数)

### 使い方

- `Solve(int n)`: $\phi(n)$ の値を返します。計算量: $O(\sqrt n)$
- `Table(int n)`: インデックス $k$ に $\phi(k)$ の値が入った長さ $n+1$ の配列を返します。計算量: $O(n \log \log n)$