---
title: itertools
documentation_of: ./IterTools.cs
---

## 概要

Python の itertools の移植です。

### 使い方

- `Permutations<T>(IEnumerable<T> collection)`: 要素を並び替えた順列を列挙します。
- `Combinations<T>(IEnumerable<T> collection, int k)`: `collection` から `k` 個取り出す部分列を列挙します。
- `CombinationsWithReplacement<T>(IEnumerable<T> collection, int k)`: `collection` から **重複を許して** `k` 個取り出す部分列を列挙します。


