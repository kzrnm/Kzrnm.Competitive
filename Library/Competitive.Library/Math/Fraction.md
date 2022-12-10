---
title: 有理数型
documentation_of: ./Fraction.cs
---

## 概要

有理数を保持する構造体です。`default(Fraction) = 0/1` を満たすように内部では分母を実際の値から `-1` しています。

生成時に最大公約数を計算しているので計算量に注意。