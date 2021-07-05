## Go の構文解析器を使用する解法

Go は標準でGoの構文解析ライブラリを備えています。

演算子の優先順位も問題文と同様なのでそのまま使い回すことができます。

JOIでは使用できませんが、言語によっては構文解析器を自前で備えていたりするので構文解析できることもあります。

提出例: https://atcoder.jp/contests/joi2020yo2/submissions/24011479

## 構文解析

入力された文字列はGoの構文としてもほぼ正しいです。ただし、`?` は変数名に使えないので適当な文字(仮に `Q` とする)に置換しておきます。
この文字列を `parser.ParseExpr` に渡すと構文解析された結果が返ります。

```Go
e = strings.ReplaceAll(e, "?", "Q")
expr, _ := parser.ParseExpr(e)
```

あとは解析された構文の種類ごとに処理を書いてあげればOKです。

```go
func solveExpr(expr ast.Expr) RSP {
	switch e := expr.(type) {
	case *ast.BinaryExpr:
		x := solveExpr(e.X)
		y := solveExpr(e.Y)
		switch e.Op {
		case token.ADD:
			return x.Plus(y)
		case token.SUB:
			return x.Minus(y)
		case token.MUL:
			return x.Prod(y)
		}
	case *ast.Ident:
		switch e.Name {
		case "R":
			return RSP{R: 1}
		case "S":
			return RSP{S: 1}
		case "P":
			return RSP{P: 1}
		default:
			return RSP{R: 1, S: 1, P: 1}
		}
	case *ast.ParenExpr:
		return solveExpr(e.X)
	}
	panic(expr)
}
```

## 括弧の掃除

上記の解法でほとんどの場合はACできます。

しかし、余計な括弧が大量についているとネストが深くなりすぎて処理しきれなくなってTLEするので掃除しておきます。

$$
pp_i  =  \left\{
\begin{array}{ll}
対応する閉じ括弧 \verb|)|のインデックス & e_i が\verb|(| \\
対応する開き括弧 \verb|(|のインデックス & e_i が \verb|)| \\
-INF & e_i が括弧ではない
\end{array}
\right.
$$

と定義しておくと、下記のいずれかを満たすときにその括弧を無視することができます。

- $e_i が\verb|(| かつ pp_{i+1} +  1 = pp_i$
- $e_i が\verb|)| かつ pp_{i-1}  -  1 = pp_i$

```go
func trimParen(e string) string {
	pp := make([]int, len(e))
	st := make([]int, 0, len(e))
	for i, v := range e {
		switch v {
		case '(':
			st = append(st, i)
		case ')':
			s := st[len(st)-1]
			pp[s] = i
			pp[i] = s
			st = st[:len(st)-1]
		default:
			pp[i] = -10000000
		}
	}
	buf := make([]byte, 0, len(e))
	for i, p := range pp {
		switch e[i] {
		case '(':
			if pp[i+1]+1 == p {
				continue
			}
		case ')':
			if pp[i-1]-1 == p {
				continue
			}
		}
		buf = append(buf, e[i])
	}
	return string(buf)
}
```