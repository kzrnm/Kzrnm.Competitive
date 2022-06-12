---
title: 実行スレッドの調整
documentation_of: //Library/Competitive.Library/Util/LargeThread.cs
---

## 概要

再帰が深いプログラムではスタックオーバーフローが発生してしまうのだが、.NET は標準のスタックサイズが 1MB しかない。
せいぜい 10000 くらい再帰すると危うくなってくるので、スタックを一気に確保できるようにする。

### 使い方

- `LargeStack(System.Threading.ThreadStart func, int stackMegaByte = 128)`: `stackMegaByte` MB だけ確保して`func`を実行する。`func`の戻り値をそのまま返す。`System.Threading.ThreadStart` は引数・戻り値なしのデリゲート(`Action` と同じ)。
- `LargeStack<T>(Func<T> func, int stackMegaByte = 128)`: `stackMegaByte` MB だけ確保して`func`を実行する。`func`の戻り値をそのまま返す。