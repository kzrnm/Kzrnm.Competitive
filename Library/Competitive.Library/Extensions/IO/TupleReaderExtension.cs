namespace Kzrnm.Competitive.IO
{
    public static class TupleReaderExtension
    {
        public static (int, int)[] Int0Int0<R>(this RepeatReader<R> rr) where R : ConsoleReader
            => rr.Select(cr => (cr.Int0(), cr.Int0()));
        public static (int, int)[] Int0Int<R>(this RepeatReader<R> rr) where R : ConsoleReader
                => rr.Select(cr => (cr.Int0(), cr.Int()));
        public static (int, int)[] IntInt<R>(this RepeatReader<R> rr) where R : ConsoleReader
                => rr.Select(cr => (cr.Int(), cr.Int()));
    }
}