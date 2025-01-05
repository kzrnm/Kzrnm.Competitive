using System.Runtime.CompilerServices;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Kzrnm.Competitive.IO
{
    public static class TupleReaderExtension
    {
        [凾(256)]
        public static (int, int)[] Int0Int0(this RepeatReader rr)
            => R(rr).Select(cr => (cr.Int0(), cr.Int0()));
        [凾(256)]
        public static (int, int)[] Int0Int(this RepeatReader rr)
                => R(rr).Select(cr => (cr.Int0(), cr.Int()));
        [凾(256)]
        public static (int, int)[] IntInt0(this RepeatReader rr)
                => R(rr).Select(cr => (cr.Int(), cr.Int0()));
        [凾(256)]
        public static (int, int)[] IntInt(this RepeatReader rr)
                => R(rr).Select(cr => (cr.Int(), cr.Int()));

        [凾(256)]
        static RepeatReader<ConsoleReader> R(RepeatReader rr)
            => Unsafe.As<RepeatReader<ConsoleReader>>(rr);
    }
}