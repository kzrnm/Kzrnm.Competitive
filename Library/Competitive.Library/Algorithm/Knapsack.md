---
title: ナップサック問題
documentation_of: ./Knapsack.cs
---

## 概要

各種のナップサック問題を解きます。

重さ $w_i$, 価値 $v_i$ の品物を、重さの和が $W$ 以下になるように選んだときの価値の和の最大値を返します。

実装上は $w_i, v_i$ はタプルで渡します。

### SmallWeight

重さの制限値 $W$ が小さい時のナップサック問題を解きます。

- 戻り値: $r_i$ が「重さの和が $i$ となるときの価値の和の最大値」である配列 $r$
- 計算量: $O(NW)$


### SmallValue

価値の合計 $\sum v_i$ が小さい時のナップサック問題を解きます。

- 戻り値: $r_i$ が「価値の和が $i$ となるときの重さの和の最小値」である配列 $r$
- 計算量: $O(N \sum v_i)$