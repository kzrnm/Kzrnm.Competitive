---
title: ビット操作
documentation_of: //Library/Competitive.Library/Bit/Bit.cs
---

## 概要

数値型へのビット操作のを定義します。

### 使い方

- `ToBitString(this T num, int padLeft = sizeof(T) * 8)`: 2進数表記の文字列にします。デフォルトの `padLeft` は数値のサイズです。
- `On(this T num, int index)`: `index` のビットが立っているかを返します。
- `Bits(this T num)`: 立っているビットを列挙します。