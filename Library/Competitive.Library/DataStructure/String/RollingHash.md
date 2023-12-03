---
title: Rolling Hash
documentation_of:
- ./RollingHash.cs
- ./RollingHashBase.cs
---

## 概要

- $h_0 = 0$
- $h_1 = s_0$
- $h_2 = s_1 + s_0 B$
- $h_3 = s_2 + s_1 B + s_0 B^2$
- $h_4 = s_3 + s_2 B + s_1 B^2 + s_0 B^3$

となるような

$$
h_i = \sum_{k=0}^{i-1} s_i B^{i-k-1}
$$

と表されるハッシュを保持します。

$$
\begin{eqnarray}
h_{l..r}
&=& h_r - B^{r-l} h_l \\
&=& (s_{r-1} + s_{r-2} B + \ldots + s_1 B^{r-2} + s_0 B^{r-1}) - B^{r-l} (s_{l-1} + s_{l-2} B + \ldots + s_1 B^{l-2} + s_0 B^{l-1}) \\
&=& (s_{r-1} + s_{r-2} B + \ldots + s_1 B^{r-2} + s_0 B^{r-1}) - (s_{l-1} B^{r-l} + s_{l-2} B^{r-l+1} + \ldots + s_1 B^{r-2} + s_0 B^{r-1}) \\
&=& s_{r-1} + s_{r-2} B + \ldots + s_{l+1} B^{r-l-2} + s_l B^{r-l-1}
\end{eqnarray}
$$

となって、任意の範囲のハッシュを取得できます。