---
title: ゼータ変換/メビウス変換
documentation_of: ./ZetaMoebiusTransform.cs
---

## 概要

- ゼータ変換: 集合 `S` に対して $\sum_{T \subseteq S} a(T)$ のようなものを求める。累積和の $2^N$ 版みたいなイメージ。
- メビウス変換: 逆ゼータ変換のこと。

### 計算量
 
$|a| = 2^N$ としたとき、計算量は $\mathrm{O}(3^N)$

### 使い方

- `SupersetZetaTransform`: 上位集合の総和をとるゼータ変換。
- `SupersetMoebiusTransform`: 上位集合の総和を分解するメビウス変換。
- `SubsetZetaTransform`: 部分集合の総和をとるゼータ変換。
- `SubsetMoebiusTransform`: 部分集合の総和を分解するメビウス変換。