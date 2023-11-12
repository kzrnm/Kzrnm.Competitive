---
title: AND 畳み込み
documentation_of: ./AndConvolution.cs
---

## 概要

長さ $2^N$ の配列 $a$, $b$ について 

$$c_k = \sum_{i, j, i \And j = k} a_i b_j$$

となる $c$ を返します。

### 計算量
 
$ \|a \| = 2^N$ としたとき、計算量は $\mathrm{O}(3^N)$

### 使い方

- `Convolution`: AND 畳み込み