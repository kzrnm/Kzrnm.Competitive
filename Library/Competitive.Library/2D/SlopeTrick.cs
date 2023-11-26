using AtCoder.Internal;
using System.Numerics;
using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // https://maspypy.com/slope-trick-1-%E8%A7%A3%E8%AA%AC%E7%B7%A8

    /// <inheritdoc cref="SlopeTrick{T}"/>
    public class SlopeTrick : SlopeTrick<long>
    {
        /// <summary>
        /// <para><paramref name="p"/> と <paramref name="q"/> を破壊的にマージします。</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N log N)</para>
        /// <para><paramref name="p"/>, <paramref name="q"/> のうち Count が大きい方のインスタンスにマージします。</para>
        /// </remarks>
        [凾(256)]
        public static SlopeTrick Merge(SlopeTrick p, SlopeTrick q)
            => Unsafe.As<SlopeTrick>(SlopeTrick<long>.Merge(p, q));

    }
    /// <summary>
    /// 左から傾きが 1 ずつ変化していく下に凸なグラフ ＼_／
    /// </summary>
    public class SlopeTrick<T> where T : INumber<T>, IMinMaxValue<T>
    {
        public static T INF => T.MaxValue / (T.One + T.One);

        /// <summary>
        /// 左側の傾き
        /// </summary>
        PriorityQueueOp<T, ReverseComparerStruct<T>> left = new();

        /// <summary>
        /// 右側の傾き
        /// </summary>
        PriorityQueueOp<T, DefaultComparerStruct<T>> right = new();

        /// <summary>
        /// 左右にまとめて加算される値
        /// </summary>
        T addL, addR;

        /// <summary>
        /// 関数の最小値
        /// </summary>
        public T Min { get; private set; }

        [凾(256)] void EnqueueLeft(T v) => left.Enqueue(v - addL);
        [凾(256)] void EnqueueRight(T v) => right.Enqueue(v - addR);
        [凾(256)] T EnqueueDequeueLeft(T v) => left.EnqueueDequeue(v - addL) + addL;
        [凾(256)] T EnqueueDequeueRight(T v) => right.EnqueueDequeue(v - addR) + addR;
        [凾(256)] T PeekLeft() => left.Count > 0 ? left.Peek + addL : -INF;
        [凾(256)] T PeekRight() => right.Count > 0 ? right.Peek + addR : INF;

        public int Count => left.Count + right.Count;

        /// <summary>
        /// 関数の最小値を取る x の範囲
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        [凾(256)]
        public (T Left, T Right) MinRange() => (PeekLeft(), PeekRight());

        /// <summary>
        /// <para>関数 f(x) に定数 <paramref name="a"/> を加算します。 f(x) = f(x) + <paramref name="a"/> </para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        [凾(256)]
        public void AddAll(T a) => Min += a;

        /// <summary>
        /// <para>関数 f(x) に ＼_ 型の関数を加算します。</para>
        /// <para><paramref name="a"/> で傾きが変わります。</para>
        /// <para>f(x) = f(x) + Max(<paramref name="a"/> - x, 0)</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log N)</para>
        /// </remarks>
        [凾(256)]
        public void AddLeftUpper(T a)
        {
            Min += T.Max(T.Zero, a - PeekRight()); // 右に食い込むと底上げされる
            EnqueueLeft(EnqueueDequeueRight(a));
        }

        /// <summary>
        /// <para>関数 f(x) に _／ 型の関数を加算します。</para>
        /// <para><paramref name="a"/> で傾きが変わります。</para>
        /// <para>f(x) = f(x) + Max(x - <paramref name="a"/>, 0)</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log N)</para>
        /// </remarks>
        [凾(256)]
        public void AddRightUpper(T a)
        {
            Min += T.Max(T.Zero, PeekLeft() - a); // 左に食い込むと底上げされる
            EnqueueRight(EnqueueDequeueLeft(a));
        }

        /// <summary>
        /// <para>関数 f(x) に ＼／ 型の関数を加算します。</para>
        /// <para><paramref name="a"/> で傾きが変わります。</para>
        /// <para>f(x) = f(x) + |x - <paramref name="a"/>|</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(log N)</para>
        /// </remarks>
        [凾(256)]
        public void AddAbs(T a)
        {
            AddLeftUpper(a);
            AddRightUpper(a);
        }

        /// <summary>
        /// <para>関数 f(x) を ＼_ 型にします。</para>
        /// <para>Max(x - a, 0) の成分を消します。</para>
        /// <para>f(x) = x &lt; MinRange → f(x), MinRange ≦ x → Min</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        [凾(256)]
        public void ClearRight() => right.Clear();

        /// <summary>
        /// <para>関数 f(x) を _／ 型にします。</para>
        /// <para>Max(a - x, 0) の成分を消します。</para>
        /// <para>f(x) = x &lt; MinRange → Min, MinRange ≦ x → f(x)</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        [凾(256)]
        public void ClearLeft() => left.Clear();

        /// <summary>
        /// <para>関数 f(x) を <paramref name="a"/> だけ右にシフトします。</para>
        /// <para>f(x) = f(x - <paramref name="a"/>)</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        [凾(256)]
        public void Shift(T a) => Shift(a, a);

        /// <summary>
        /// <para>関数 f(x) の左側を <paramref name="left"/>, 右側を <paramref name="right"/> だけ右にシフトします。</para>
        /// <para>＼_／ → ＼___／</para>
        /// <para>f(x) =</para>
        /// <para>x &lt; MinRange+<paramref name="left"/> → f(x - <paramref name="left"/>)</para>
        /// <para>MinRange+<paramref name="left"/> ≦ x ≦ MinRange+<paramref name="left"/>+<paramref name="right"/> → Min</para>
        /// <para>MinRange+<paramref name="left"/>+<paramref name="right"/> &lt; x → f(x - <paramref name="right"/>)</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        [凾(256)]
        public void Shift(T left, T right)
        {
            addL += left;
            addR += right;
        }

        /// <summary>
        /// <para>f(<paramref name="x"/>) を返します。</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N)</para>
        /// </remarks>
        [凾(256)]
        public T Eval(T x)
        {
            T r = Min;
            foreach (var a in left.Unorderd())
                r += T.Max(T.Zero, a + addL - x);
            foreach (var a in right.Unorderd())
                r += T.Max(T.Zero, x - (a + addR));
            return r;
        }

        /// <summary>
        /// <para>関数 f(x) の中身を破棄します。</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(1)</para>
        /// </remarks>
        [凾(256)]
        public void Clear()
        {
            Min = T.Zero;
            addL = T.Zero;
            addR = T.Zero;
            left.Clear();
            right.Clear();
        }

        /// <summary>
        /// <para><paramref name="p"/> と <paramref name="q"/> を破壊的にマージします。</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N log N)</para>
        /// <para><paramref name="p"/>, <paramref name="q"/> のうち Count が大きい方のインスタンスにマージします。</para>
        /// </remarks>
        [凾(256)]
        public static SlopeTrick<T> Merge(SlopeTrick<T> p, SlopeTrick<T> q)
        {
            if (p.Count < q.Count) (p, q) = (q, p);
            p.Merge(q);
            return p;
        }

        /// <summary>
        /// <para><paramref name="q"/> をマージします。</para>
        /// </summary>
        /// <remarks>
        /// <para>計算量: O(N log N)</para>
        /// </remarks>
        [凾(256)]
        public void Merge(SlopeTrick<T> q)
        {
            Min += q.Min;
            foreach (var a in q.left.Unorderd())
                AddLeftUpper(a + q.addL);
            foreach (var a in q.right.Unorderd())
                AddRightUpper(a + q.addR);
        }
    }
}