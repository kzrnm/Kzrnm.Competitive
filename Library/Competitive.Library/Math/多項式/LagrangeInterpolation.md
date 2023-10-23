---
title: ラグランジュ補間
documentation_of: ./LagrangeInterpolation.cs
---

## 概要

$N$ 次多項式 $f(x) = a_0 + a_1 x + a_2 x^2 \cdots + a_{n} x^n$ についてのラグランジュ補間を行います。

### 使い方

- `Coefficient((x, y) plots)`: $N+1$ 個の $x_k, y_k$ の入力について $y_k = f(x_k)$ をみたす $N$ 次多項式 $f(x)$ を返します。計算量: $O(N^2)$
- `Eval(y, x, combination)`: $k = \{ 0, 1, 2, \ldots, N \}$ について $y_k = f(k)$ となる $N$ 次多項式 に $x$ を代入した値を返します。計算量: $O(N)$
  - ※: 複数回呼び出すときは初期化済みの二項係数 `combination` を渡すようにします。