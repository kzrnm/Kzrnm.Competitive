---
title: ビット集合での畳み込みの実装
documentation_of: ./BitConv.cs
---

## 概要

長さ $2^N$ の配列 $a$, $b$ について 

$$c_k = \sum_{cond} a_i b_j$$

となる $c$ を返します。

$cond$ はサブクラスで定義した内容を使います。

### 計算量
 
$ \|a \| = 2^N$ としたとき、計算量は $\mathrm{O}(3^N)$

### 使い方

- `Conv`: 畳み込み