---
title: 線形漸化式
documentation_of: ./LinearRecurrence.cs
---

## 概要

数列 $a_n$ について $n=k$ のときの値 $a_k$ を求めるアルゴリズムです。

### 使い方

- `BostanMori(Q, P, k)`: 高々 $k$ 次の多項式 $P(x), Q(x)$ について $\dfrac{P(x)}{Q(x)}$ の $x^n$ の係数を求めます。計算量: $O(k \log k \log n)$
- `Kitamasa(a, c, n)`: $a_{n+k} = c_0 a_{n+k-1} + \cdots + c_{k-1} a_{n}$ である $k$ 項間漸化式について、$a_n$ を求めます。`a` は最初の $k$ 項を渡します。計算量: $O(k \log k \log n)$