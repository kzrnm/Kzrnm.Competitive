using AtCoder.Algebra;
using System.Collections.Generic;

namespace AtCoder
{
    /// <summary>
    /// コンパイル時に決定する mod を表します。
    /// </summary>
    /// <example>
    /// <code>
    /// public readonly struct Mod1000000009 : IStaticMod
    /// {
    ///     public uint Mod => 1000000009;
    ///     public bool IsPrime => true;
    /// }
    /// </code>
    /// </example>
    public interface IStaticMod
    {
        /// <summary>
        /// mod を取得します。
        /// </summary>
        uint Mod { get; }

        /// <summary>
        /// mod が素数であるか識別します。
        /// </summary>
        bool IsPrime { get; }
    }

#pragma warning disable CA1815
    public readonly struct Mod1000000007 : IStaticMod
    {
        public uint Mod => 1000000007;
        public bool IsPrime => true;
    }

    public readonly struct Mod998244353 : IStaticMod
    {
        public uint Mod => 998244353;
        public bool IsPrime => true;
    }

    /// <summary>
    /// 実行時に決定する mod の ID を表します。
    /// </summary>
    /// <example>
    /// <code>
    /// public readonly struct ModID123 : IDynamicModID { }
    /// </code>
    /// </example>
    public interface IDynamicModID { }

    public readonly struct ModID0 : IDynamicModID { }
    public readonly struct ModID1 : IDynamicModID { }
    public readonly struct ModID2 : IDynamicModID { }

    public readonly struct StaticModIntOperator<T> :
        IAddOperator<StaticModInt<T>>,
        ISubtractOperator<StaticModInt<T>>,
        IMultiplyOperator<StaticModInt<T>>,
        IDivideOperator<StaticModInt<T>>,
        IIncrementOperator<StaticModInt<T>>,
        IDecrementOperator<StaticModInt<T>>,
        INegateOperator<StaticModInt<T>>,
        IEqualityComparer<StaticModInt<T>>
        where T : struct, IStaticMod
    {
        public StaticModInt<T> Add(StaticModInt<T> x, StaticModInt<T> y) => x + y;
        public StaticModInt<T> Subtract(StaticModInt<T> x, StaticModInt<T> y) => x - y;
        public StaticModInt<T> Multiply(StaticModInt<T> x, StaticModInt<T> y) => x * y;
        public StaticModInt<T> Divide(StaticModInt<T> x, StaticModInt<T> y) => x / y;
        public StaticModInt<T> Negate(StaticModInt<T> x) => -x;
        public StaticModInt<T> Increment(StaticModInt<T> x) => ++x;
        public StaticModInt<T> Decrement(StaticModInt<T> x) => --x;
        public bool Equals(StaticModInt<T> x, StaticModInt<T> y) => x == y;
        public int GetHashCode(StaticModInt<T> obj) => obj.GetHashCode();
    }
#pragma warning restore CA1815
}
