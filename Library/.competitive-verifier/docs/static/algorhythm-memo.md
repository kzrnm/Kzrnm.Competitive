# アルゴリズムのメモ

未実装のアルゴリズムをメモしておく

## ラグランジュの反転公式

母関数による数え上げで $x = G(x)$ の形式、つまり $x$ の逆関数に帰結できるときに使える公式。

$[x^n] F(x)$ は $F(x)$ の $x^n$ の係数を表すとして、

> $F(x)$ の逆関数 $G(x)$ について $[x^0] F(x) = [x^0] G(x) = 0$ かつ $[x] F(x) \ne 0, [x] G(x) \ne 0$ が成り立つとき
> 
> $$[x^n] F(x) = \frac{1}{n} [x^{n-1}] \left( \frac{x}{G(x)} \right)^n$$
> 
> が成り立つ。

[ABC222:H問題 Beautiful Binary Tree](https://atcoder.jp/contests/abc222/tasks/abc222_h)