using AtCoder;
using AtCoder.Operators;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    // competitive-verifier: TITLE IntToStaticModCastOperator
    public struct IntToStaticModCastOperator<T> : ICastOperator<int, StaticModInt<T>>
           where T : struct, IStaticMod
    {
        [凾(256)]
        public StaticModInt<T> Cast(int y) => new StaticModInt<T>(y);
    }
}
