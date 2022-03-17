using AtCoder;

namespace Kzrnm.Competitive
{

    /// <summary>
    /// 長さ N の配列に対し、
    /// <list type="bullet">
    /// <item>
    /// <description>要素の区間変更</description>
    /// </item>
    /// <item>
    /// <description>区間の要素の総和</description>
    /// </item>
    /// </list>
    /// <para>を O(log⁡N) で求めることが出来るデータ構造です。</para>
    /// </summary>
    public class StaticModIntFenwickTreeRange<T> : FenwickTreeRange<StaticModInt<T>, StaticModIntOperator<T>, IntToStaticModCastOperator<T>>
        where T : struct, IStaticMod
    {


        /// <summary>
        /// 長さ <paramref name="n"/> の配列aを持つ <see cref="StaticModIntFenwickTreeRange{T}"/> クラスの新しいインスタンスを作ります。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        /// <param name="n">配列の長さ</param>
        public StaticModIntFenwickTreeRange(int n) : base(n) { }
    }
}