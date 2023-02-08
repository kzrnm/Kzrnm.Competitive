namespace Kzrnm.Competitive
{
    public enum CirclePosition
    {
        /// <summary>
        /// 小さい方の円が大きい方の円の内側にある
        /// </summary>
        Inner,
        /// <summary>
        /// 内接している
        /// </summary>
        Inscribed,
        /// <summary>
        /// 交わっている
        /// </summary>
        Intersected,
        /// <summary>
        /// 外接している
        /// </summary>
        Circumscribed,
        /// <summary>
        /// 共有領域を持たない
        /// </summary>
        Separated,
    }
}
