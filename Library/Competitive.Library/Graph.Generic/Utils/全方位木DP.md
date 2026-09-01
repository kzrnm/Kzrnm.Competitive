---
title: 全方位木DP
documentation_of: ./全方位木DP.cs
---

## 概要

全方位木DP を実装する。

- `Identity`: 動的計画法の単位元。部分木の根の値として使われる。
- `T Propagate(T x, int parent, TEdge edge)`: 子要素から親要素へ伝播させる。部分木(画像の赤) に辺を追加するイメージ。(画像の緑)
- `T Merge(T x, T y)`: 追加中の部分木 `x` (画像の赤) にその子である部分木を追加する (画像の緑)。

### 図解

![説明画像]({{ "rerooting.svg" | absolute_url }})

1. `Propagate` で親へマージの準備
2. `Merge` で `Propagate` してきた子の部分木を取り込む

と繰り返して、部分木の根を含むような含まないような微妙な状態をマージしていく。

`Propagate` では親への辺までの情報で完結して親ノードの情報は含まないはず。`Merge` では `x` と `y` は等価なはず。