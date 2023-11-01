---
title: 特性多項式
documentation_of: ./CharacteristricPolynomial.cs
---

## 概要

$n$ 次の正方行列 $A$ とある $n$ 次の列ベクトル $\boldsymbol{x}$ の積がスカラー $\lambda$ と $\boldsymbol{x}$ の積と一致する、つまり

$$A \boldsymbol{x} = \lambda \boldsymbol{x}$$

となるとき、$\lambda$ を $A$ の **固有値** 、$\boldsymbol{x}$  を **固有ベクトル** とよぶ。


$I$ を単位行列、$\det$ は行列式として、$\lambda$ は

$$ \det (\lambda I - M)=0$$

をみたす。

これは $\lambda$ について $n$ 次方程式になっている。これを **特性方程式** (または固有方程式) とよぶ。

また、

$$\det (x I - M) = \sum_{i = 0}^n p_i x^i$$

を **特性多項式** (または固有多項式) とよぶ。


### 使い方

- `CharacteristricPolynomial(bool normalize)`: 特性多項式の係数 $p_i$ を返します。計算量: $O(n^3)$