using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.IO
{
    public static class TupleReaderExtension
    {
        [凾(256)]
        public static (int, int)[] Int0Int0<R>(this RepeatReader<R> rr) where R : ConsoleReader
            => rr.Select(cr => (cr.Int0(), cr.Int0()));
        [凾(256)]
        public static (int, int)[] Int0Int<R>(this RepeatReader<R> rr) where R : ConsoleReader
                => rr.Select(cr => (cr.Int0(), cr.Int()));
        [凾(256)]
        public static (int, int)[] IntInt<R>(this RepeatReader<R> rr) where R : ConsoleReader
                => rr.Select(cr => (cr.Int(), cr.Int()));
    }
}