[![verify](https://github.com/kzrnm/Kzrnm.Competitive/workflows/verify/badge.svg)](https://github.com/kzrnm/Kzrnm.Competitive/actions)
[![NuGet version (Kzrnm.Competitive)](https://img.shields.io/nuget/v/Kzrnm.Competitive.svg?style=flat-square)](https://www.nuget.org/packages/Kzrnm.Competitive/)
[![GitHub Pages](https://img.shields.io/static/v1?label=GitHub+Pages&message=Kzrnm.Competitife+&color=brightgreen&logo=github)](https://kzrnm.github.io/Kzrnm.Competitive/)

## 使い方

このリポジトリ内で使うつもりで作ったライブラリですが、NuGet からインストールもできます。[SourceExpander](https://github.com/kzrnm/SourceExpander) と一緒に使ってください。

```sh
dotnet add package Kzrnm.Competitive
dotnet add package SourceExpander
```

## LICENSE

基本的には MIT license とします。

ただし、`Competitive.Library/` 以下のファイルを競技プログラミングの用途で提出コードに含める場合はライセンス表記なしの使用可です(下記の例外を除く)。

### LICENSE の例外

- `Competitive.Library/Collection/Set`
- `Competitive.Library/Number.GenericMath`

上記については、MIT license で公開されている .NET のコードを元にしているので競技プログラミング用途でも MIT license としてください。
[SourceExpander](https://github.com/kzrnm/SourceExpander) で埋め込む場合はライセンス表記が `string` として残るので確実です。