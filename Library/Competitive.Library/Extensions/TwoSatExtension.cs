using AtCoder;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    /// <summary>
    /// AddClause は (x_i=¬f)∧(x_j=¬g) を削除する操作と同等なのでうまく扱う。
    /// </summary>
    public static class __TwoSatExtension
    {
        /// <summary>
        /// (x_<paramref name="i"/>=<paramref name="f"/>)∨(x_<paramref name="j"/>=<paramref name="g"/>) というクローズを追加します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="i"/>&lt;n, 0≤<paramref name="j"/>&lt;n</para>
        /// <para>計算量: ならし O(1)</para>
        /// </remarks>
        [凾(256)] public static void Or(this TwoSat t, int i, bool f, int j, bool g) => t.AddClause(i, f, j, g);
        /// <summary>
        /// (x_<paramref name="i"/>=<paramref name="f"/>)∧(x_<paramref name="j"/>=<paramref name="g"/>) というクローズを追加します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="i"/>&lt;n, 0≤<paramref name="j"/>&lt;n</para>
        /// <para>計算量: ならし O(1)</para>
        /// </remarks>
        [凾(256)]
        public static void And(this TwoSat t, int i, bool f, int j, bool g)
        {
            t.AddClause(i, f, j, g);
            t.AddClause(i, f, j, !g);
            t.AddClause(i, !f, j, g);
        }
        /// <summary>
        /// (x_<paramref name="i"/> = x_<paramref name="j"/>) というクローズを追加します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="i"/>&lt;n, 0≤<paramref name="j"/>&lt;n</para>
        /// <para>計算量: ならし O(1)</para>
        /// </remarks>
        [凾(256)]
        public static void Same(this TwoSat t, int i, int j)
        {
            t.AddClause(i, true, j, false);
            t.AddClause(i, false, j, true);
        }
        /// <summary>
        /// (x_<paramref name="i"/> != x_<paramref name="j"/>) というクローズを追加します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="i"/>&lt;n, 0≤<paramref name="j"/>&lt;n</para>
        /// <para>計算量: ならし O(1)</para>
        /// </remarks>
        [凾(256)]
        public static void NotSame(this TwoSat t, int i, int j)
        {
            t.AddClause(i, true, j, true);
            t.AddClause(i, false, j, false);
        }
        /// <summary>
        /// (x_<paramref name="i"/>=<paramref name="f"/>)→(x_<paramref name="j"/>=<paramref name="g"/>) というクローズを追加します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="i"/>&lt;n, 0≤<paramref name="j"/>&lt;n</para>
        /// <para>計算量: ならし O(1)</para>
        /// </remarks>
        [凾(256)] public static void IfThen(this TwoSat t, int i, bool f, int j, bool g) => t.AddClause(i, !f, j, g);
        /// <summary>
        /// (x_<paramref name="i"/>=<paramref name="f"/>) というクローズを追加します。
        /// </summary>
        /// <remarks>
        /// <para>制約: 0≤<paramref name="i"/>&lt;n</para>
        /// <para>計算量: ならし O(1)</para>
        /// </remarks>
        [凾(256)] public static void Set(this TwoSat t, int i, bool f) => t.AddClause(i, f, i, f);
    }
}