namespace SourceExpander.Testing
{
    using AtCoder;
    using AtCoder.Extension;
    using Kzrnm.Competitive;
    using Kzrnm.Competitive.IO;
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;
    using System.Numerics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Intrinsics;
    using System.Text;
    using System.Text.RegularExpressions;
    using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;
    using static System.Math;
    using static Kzrnm.Competitive.BitOperationsEx;
    using static Kzrnm.Competitive.Global;
    using static Kzrnm.Competitive.IterTools;
    using static Kzrnm.Competitive.MathLibEx;
    using static Kzrnm.Competitive.NumberTheoreticTransform;
    using static Kzrnm.Competitive.__BinarySearchEx;
    using static Program;
    using BitArray = System.Collections.BitArray;
    using ModInt = AtCoder.StaticModInt<SourceExpander.Testing.AtCoder.Mod998244353>;
    using MontgomeryModInt = Kzrnm.Competitive.MontgomeryModInt<SourceExpander.Testing.AtCoder.Mod998244353>;
    using ModIntFactor = Kzrnm.Competitive.ModIntFactor<SourceExpander.Testing.AtCoder.StaticModInt<SourceExpander.Testing.AtCoder.Mod998244353>>;
    using MontgomeryModIntFactor = Kzrnm.Competitive.ModIntFactor<Kzrnm.Competitive.MontgomeryModInt<SourceExpander.Testing.AtCoder.Mod998244353>>;

    partial class Program
    {
        const bool __ManyTestCases = false;
        static void Main() => new Program(new(), new()).Run();
        [凾(256)]
        private ConsoleOutput? Calc()
        {
            var m1 = new ModInt[5];
            var m2 = new MontgomeryModInt[5];
            m1[0] = new ModIntFactor(10).Combination(5, 1);
            m2[0] = new MontgomeryModIntFactor(10).Combination(5, 1);

            var bt = new BitArray(4);
            BinarySearch<Op>(0, 1);
            Convolution(m2, m2);
            int gcd = Gcd(451532, 45104);
            CombinationsWithReplacement(m1, 4);
            long[][] la = NewArray(2, 4, 0L);
            int msb = Msb(421);
            uint max = Max(1u, 2u);

            Regex.Replace("foo", "(foo|bar)", "");

            new StringBuilder().Append(12);

            return la[0].LowerBound(4L);
        }
        public static Deque<int> deque;
    }
    struct Op : IOk<int>
    {
        public bool Ok(int value) => true;
    }
    class Other
    {
        public static int F = deque.First;
    }
}