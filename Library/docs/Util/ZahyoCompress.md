---
title: 座標圧縮
documentation_of: //Library/Competitive.Library/Util/ZahyoCompress.cs
---

## 概要

値を座標圧縮します。

`IComparable<T>` でない型も `IComparer<T>` を渡して比較可能です。

### 使い方

- `ZahyoCompress.Create<T>(T[] orig)`: 配列や `IEnumerable<T>` などを座標圧縮した状態で初期化します。
- `new ZahyoCompress<T>()`: 空で初期化します。
- `ZahyoCompress.CompressedArray<T>`: 座標圧縮後の値に置換した配列を取得する
- `Add(T item)`: 座標圧縮する値を追加します。
- `Compress()`: デフォルトの `CompareTo` をもとに座標圧縮します。
- `Compress(IComparer<T> comparer)`: `comparer` をもとに座標圧縮します。
- `Replace(T[] orig)`: 引数の値を座標圧縮後の値に置換した配列を返します。
- `NewTable`: 座標圧縮後の値を返す `Dictinoary<T, int>` です。
- `Original`: 座標圧縮後の値に対応するもとの値を保持する配列です。