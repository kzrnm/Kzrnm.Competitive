---
title: Mo's algorithm
documentation_of: ./MoAlgorithm.cs
---


## 概要

Mo のアルゴリズムです。

$[0, N)$ の区間についての $Q$ 個のクエリを $O( \alpha (N+Q) \sqrt{Q} )$ で解きます。

ただし、$[x, y)$ が計算済みのときに $[x, y-1), [x, y+1), [x-1, y), [x+1, y)$ を求める計算量が $O(\alpha)$ とします。


### 使い方

- `AddQuery(int from, int toExclusive)`: クエリを追加します。
- `Solve<T, TSt>(TSt st)`: 初期状態 `st` から各クエリの結果を返します。
- `SolveStrict<T, TSt>(TSt st)`: 初期状態 `st` から各クエリの結果を返します。
- `Solve(Action<int> add, Action<int> remove, Action<int> update)`: クエリを適用します。
- `SolveStrict(Action<int> addLeft, Action<int> addRight, Action<int> removeLeft, Action<int> removeRight, Action<int> update)`: クエリを適用します。
- `Solve<TOp>(TOp op)`: クエリを適用します。
- `SolveStrict<TOp>(TOp op)`: クエリを適用します。