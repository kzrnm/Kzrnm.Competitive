[![test](https://github.com/kzrnm/Kzrnm.Competitive/workflows/test/badge.svg)](https://github.com/kzrnm/Kzrnm.Competitive/actions)
[![NuGet version (Kzrnm.Competitive)](https://img.shields.io/nuget/v/Kzrnm.Competitive.svg?style=flat-square)](https://www.nuget.org/packages/Kzrnm.Competitive/)
[![GitHub Pages](https://img.shields.io/static/v1?label=GitHub+Pages&message=Kzrnm.Competitife+&color=brightgreen&logo=github)](https://kzrnm.github.io/Kzrnm.Competitive/)


C# での競技プログラミング用のライブラリ・実行環境を整備したリポジトリです。

本家の `oj-verify` は C# に対応していないので [改造版](https://github.com/kzrnm/verification-helper) を使用しています。

## 動作環境

.NET Core 3.1, C# 8.0
AtCoder に準ずる。

## 使い方

リポジトリ内で使うつもりで作ったライブラリですが、NuGet からインストールもできます。[SourceExpander](https://github.com/kzrnm/SourceExpander) と一緒に使ってください。

```sh
dotnet add package Kzrnm.Competitive
dotnet add package SourceExpander
```