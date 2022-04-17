using System.Collections.Generic;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive
{
    public static class __CollectionTupleExtension_Queue
    {
        [凾(256)]
        public static bool TryDequeue<T1, T2>(this Queue<(T1, T2)> stack, out T1 item1, out T2 item2)
        {
            var ok = stack.TryDequeue(out var tuple);
            (item1, item2) = tuple;
            return ok;
        }
        [凾(256)]
        public static bool TryDequeue<T1, T2, T3>(this Queue<(T1, T2, T3)> stack, out T1 item1, out T2 item2, out T3 item3)
        {
            var ok = stack.TryDequeue(out var tuple);
            (item1, item2, item3) = tuple;
            return ok;
        }
        [凾(256)]
        public static bool TryPeek<T1, T2>(this Queue<(T1, T2)> stack, out T1 item1, out T2 item2)
        {
            var ok = stack.TryPeek(out var tuple);
            (item1, item2) = tuple;
            return ok;
        }
        [凾(256)]
        public static bool TryPeek<T1, T2, T3>(this Queue<(T1, T2, T3)> stack, out T1 item1, out T2 item2, out T3 item3)
        {
            var ok = stack.TryPeek(out var tuple);
            (item1, item2, item3) = tuple;
            return ok;
        }
    }
}
