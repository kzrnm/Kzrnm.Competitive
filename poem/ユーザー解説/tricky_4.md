値を抜き取るのは正規表現で `Takahashi is the (\d+)` や`(\S+) overtakes (\S+)` などとすると簡単です。

追い抜きごとに各選手の前か後ろの選手が確定するため、選手の状態を連結リストで保持すると良いです。

あとはレースの状況をシミュレーションして矛盾があるかを判定すれば答えが求まります。

### 出力

英語のひっかけがあります。  
「下一桁が1,2,3なら`st`,`nd`,`rd` そうでなければ `th` をつける」としてしまうと WA です。  
下二桁が `11`,`12`,`13` の場合は `th` です。

- eleventh: 11th
- twelfth: 12th
- thirteenth: 13th
- hundred and eleventh: 111th
- hundred and twelfth: 112th
- hundred and thirteenth: 113th

### 矛盾があると判定するケース

#### `名前1` と `名前2` が等しい
同名の選手はいないという制約があるので、自分で自分を追い越してしまうことになります。

#### $N$ 人より多く名前が出てくる
参加者が $N$ 人なので名前の種類も高橋くんを含めて $N$ 以下になるはずです。


#### 追い抜かれた選手の後ろの選手が確定しているのに追い抜いた選手ではない
#### 追い抜いた選手の前の選手が確定しているのに追い抜かれた選手ではない

選手 $A$ が選手 $B$ を追い抜いたとします。

その後、選手 $C$ が選手 $A$ を追い抜いたら矛盾です。なぜならば、選手 $A$ の後ろは選手 $B$ で確定しているためです。  
同様に、選手 $B$ が選手 $C$ を追い抜いたら矛盾です。なぜならば、選手 $B$ の前は選手 $A$ で確定しているためです。

このように選手間に別の選手が湧いて出てきている場合は矛盾です。

#### 追い抜かれた選手の前を辿ると追い抜かれた選手になる

前から順に $A \rightarrow B \rightarrow C$ という関係が確定している集団を考えます。

この状態で、選手 $A$ が選手 $C$ を追い抜いたら矛盾です。連結リストにループがある状態になっています。

Union-Find を用いて閉路検出すれば良いです。

#### 高橋くんがいる集団の先頭の順位が $0$ 位(またはそれより前)になる
#### 高橋くんがいる集団の末尾の順位が $N+1$ 位(またはそれより後ろ)になる。

レースをシミュレーションすれば、最終的な高橋くんの順位は確定できます。

また、レース終了時には最終的に高橋くんがいる集団が形成されています。よって、連結リストを何回辿れるかカウントすれば高橋くんのいる集団の先頭/末尾の順位も確定できます。

その順位がありえない値になっていたら矛盾です。

#### 高橋くんがいない集団が収まらない

$N=5$ で $A \rightarrow B,$ $C \rightarrow D$, $Takahashi$ の3集団があるとします。

高橋くんの順位が $2$ 位や $4$ 位だと高橋くん以外の集団がうまく当てはめられません。

当てはめられるかどうかを判定する必要があります。

##### 当てはめられるかの判定

高橋くんがいる集団の先頭の順位 $p_1$、末尾の順位を $p_2$、高橋くんがいる集団の人数を $M$ とします。また、名前が出てきた人数を $C$ とします。

$s_1 = p_1 - 1, s_2 = N - p_2$ で残りの枠のサイズがわかります。

高橋くんの前後であるかの区別は不要なので、以下では $s_1 \lt s_2$ であるとします。

$s_2 \ge C - M$ ならば $s_2$ に全員当てはめられるので矛盾はありません。  
そうでなければ、動的計画法で当てはめられるかを判定します。

- $dp_i(0\le i \le s_1) = \{高橋くんがいない集団をいくつか選んで合計で i 人にできるかどうか\}$
  - $dp_i$ が真のときに、$k$ 人の集団を加えれば $dp_{i+k}$ も真になる。

という動的計画法で判定できます。

$dp_i$ が真で、$C-M-i \le s_2$ ならば、高橋くんがいない集団を高橋くんの前後に分けて当てはめることができます。

そのような $dp_i$ がなければ当てはめられないので矛盾となります。

### 検証用サンプルデータ

```
5
13 11
Takahashi is the 3th place
10 overtakes 1
10 overtakes 2
10 overtakes 3
10 overtakes Takahashi
10 overtakes 4
10 overtakes 5
7 overtakes 6
6 overtakes 7
6 overtakes 8
6 overtakes 9
11 overtakes 12

13 9
Takahashi is the 4th place
A overtakes B
A overtakes C
A overtakes D
A overtakes E
A overtakes F
Z overtakes Y
Z overtakes X
Z overtakes W
S overtakes T

13 9
Takahashi is the 4th place
A overtakes B
A overtakes C
A overtakes D
A overtakes E
A overtakes F
A overtakes Y
Z overtakes X
Z overtakes W
S overtakes T

3 2
Takahashi is the 2nd place
Takahashi overtakes Aoki
HechoSamurai overtakes Aoki

18446744073709551615 1
Takahashi is the 18446744073709551615st place
A overtakes B
```