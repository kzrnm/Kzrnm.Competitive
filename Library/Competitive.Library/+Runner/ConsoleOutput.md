---
title: 出力用のラッパー
documentation_of: ./ConsoleOutput.cs
---

## 概要

`Utf8ConsoleWriter` は呼び出した型ごとに高速に標準出力に書き込める。

しかし、`Program.Calc` の返り値としてジェネリクスを使うのは難しい(返り値で判定することになるのでラムダ式を駆使する羽目になる)。

そのラッパーとして `ConsoleOutput` への暗黙の型変換によって標準出力への書き込みを行うことにした。

これによって `Program.Calc` の返り値がそのまま標準出力に書き込まれる。