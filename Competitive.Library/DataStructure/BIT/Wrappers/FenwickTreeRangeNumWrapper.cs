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
    public class IntFenwickTreeRange : FenwickTreeRange<int, IntOperator, SameTypeCastOperator<int>>
    {


        /// <summary>
        /// 長さ <paramref name="n"/> の配列aを持つ <see cref="IntFenwickTreeRange"/> クラスの新しいインスタンスを作ります。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        /// <param name="n">配列の長さ</param>
        public IntFenwickTreeRange(int n) : base(n) { }
    }

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
    public class LongFenwickTreeRange : FenwickTreeRange<long, LongOperator, IntToLongCastOperator>
    {


        /// <summary>
        /// 長さ <paramref name="n"/> の配列aを持つ <see cref="LongFenwickTreeRange"/> クラスの新しいインスタンスを作ります。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="n"/>≤10^8</para>
        /// <para>計算量: O(<paramref name="n"/>)</para>
        /// </remarks>
        /// <param name="n">配列の長さ</param>
        public LongFenwickTreeRange(int n) : base(n) { }
    }
}