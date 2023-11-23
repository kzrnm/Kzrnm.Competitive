---
title: 最長増加部分列(LIS)
documentation_of: ./LongestIncreasingSubsequence.cs
---

## 概要

ある数列の連続とは限らない部分列 $b$ が単調増加になっており、他の単調増加な部分列で $b$ より長いものが存在しないとき、$b$ をその数列の **最長増加部分列** または **LIS(Longest Increasing Subsequence)** と呼びます。

動的計画法により $O(N \log N)$ で求められます。

### 使い方

- `Lis<T>(T[] s, bool strict = true)`: `strict` の値で狭義/広義単調増加のどちらにするか変えられます。デフォルトは狭義単調増加です。