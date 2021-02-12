using AtCoder;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive
{
    public struct IntToStaticModCastOperator<T> : ICastOperator<int, StaticModInt<T>>
           where T : struct, IStaticMod
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StaticModInt<T> Cast(int y) => new StaticModInt<T>(y);
    }
}
