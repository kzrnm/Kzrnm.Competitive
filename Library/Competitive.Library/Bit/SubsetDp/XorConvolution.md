---
title: XOR 畳み込み
documentation_of: ./XorConvolution.cs
---

## 概要

長さ $2^N$ の配列 $a$, $b$ について 

$$c_k = \sum_{i, j, i \oplus j = k} a_i b_j$$

となる $c$ を返します。

### 計算量
 
$|a| = 2^N$ としたとき、計算量は $\mathrm{O}(3^N)$

### 使い方

- `Convolution`: XOR 畳み込み
- `WalshHadamardTransform`: 高速ウォルシュ・アダマール変換