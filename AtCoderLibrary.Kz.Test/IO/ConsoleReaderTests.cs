using FluentAssertions;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

#pragma warning disable CA1810 // Initialize reference type static fields inline

namespace AtCoder.IO
{
    public class ConsoleReaderTests
    {
        const int N = 500;
        readonly MemoryStream stream;
        private readonly ConsoleReader cr;
        static readonly string input;
        static ConsoleReaderTests()
        {
            var rnd = new Random(42);
            var sb = new StringBuilder();
            for (int i = 0; i < N; i++)
            {
                sb.Append(rnd.Next());
                sb.Append(' ');
                sb.Append(rnd.Next());
                sb.Append(' ');
                sb.Append(rnd.Next());
                sb.AppendLine();
            }
            input = sb.ToString();
        }
        public ConsoleReaderTests()
        {
            stream = new MemoryStream(new UTF8Encoding(false).GetBytes(input));
            cr = new ConsoleReader(stream, new UTF8Encoding(false));
        }

        #region Single
        [Fact(Timeout = 1000)]
        [Trait("Category", "Single")]
        public async Task Line() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < N; i++)
            {
                cr.Line.Should().Be($"{rnd.Next()} {rnd.Next()} {rnd.Next()}");
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Single")]
        public async Task Char() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            var head = rnd.Next().ToString() + rnd.Next().ToString();
            foreach (var c in head)
                cr.Char.Should().Be(c);
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Single")]
        public async Task Int() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < 3 * N; i++)
            {
                cr.Int.Should().Be(rnd.Next());
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Single")]
        public async Task IntImplicit() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < 3 * N; i++)
            {
                int r = cr;
                r.Should().Be(rnd.Next());
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Single")]
        public async Task Int0() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < 3 * N; i++)
            {
                cr.Int0.Should().Be(rnd.Next() - 1);
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Single")]
        public async Task Long() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < 3 * N; i++)
            {
                cr.Long.Should().Be(rnd.Next());
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Single")]
        public async Task LongImplicit() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < 3 * N; i++)
            {
                long r = cr;
                r.Should().Be(rnd.Next());
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Single")]
        public async Task Long0() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < 3 * N; i++)
            {
                cr.Long0.Should().Be(rnd.Next() - 1);
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Single")]
        public async Task Double() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < 3 * N; i++)
            {
                cr.Double.Should().Be(rnd.Next());
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Single")]
        public async Task DoubleImplicit() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < 3 * N; i++)
            {
                double r = cr;
                r.Should().Be(rnd.Next());
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Single")]
        public async Task DoublePeriod() => await Task.Run(() =>
        {
            var stream = new MemoryStream(new UTF8Encoding(false).GetBytes("   1232.25"));
            var cr = new ConsoleReader(stream, new UTF8Encoding(false));
            cr.Double.Should().Be(1232.25);
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Single")]
        public async Task Ascii() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < 3 * N; i++)
            {
                cr.Ascii.Should().Be(rnd.Next().ToString());
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Single")]
        public async Task AsciiImplicit() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < 3 * N; i++)
            {
                string r = cr;
                r.Should().Be(rnd.Next().ToString());
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Single")]
        public async Task String() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < 3 * N; i++)
            {
                cr.String.Should().Be(rnd.Next().ToString());
            }
        });
        #endregion

        #region Split
        [Fact(Timeout = 1000)]
        [Trait("Category", "Split")]
        public async Task SplitInt() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < N; i++)
            {
                cr.Split.Int.Should().Equal(new int[] { rnd.Next(), rnd.Next(), rnd.Next() });
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Split")]
        public async Task SplitIntImplicit() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < N; i++)
            {
                int[] r = cr.Split;
                r.Should().Equal(new int[] { rnd.Next(), rnd.Next(), rnd.Next() });
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Split")]
        public async Task SplitInt0() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < N; i++)
            {
                cr.Split.Int0.Should().Equal(new int[] { rnd.Next() - 1, rnd.Next() - 1, rnd.Next() - 1 });
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Split")]
        public async Task SplitLong() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < N; i++)
            {
                cr.Split.Long.Should().Equal(new long[] { rnd.Next(), rnd.Next(), rnd.Next() });
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Split")]
        public async Task SplitLongImplicit() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < N; i++)
            {
                long[] r = cr.Split;
                r.Should().Equal(new long[] { rnd.Next(), rnd.Next(), rnd.Next() });
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Split")]
        public async Task SplitLong0() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < N; i++)
            {
                cr.Split.Long0.Should().Equal(new long[] { rnd.Next() - 1, rnd.Next() - 1, rnd.Next() - 1 });
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Split")]
        public async Task SplitDouble() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < N; i++)
            {
                cr.Split.Double.Should().Equal(new double[] { rnd.Next(), rnd.Next(), rnd.Next() });
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Split")]
        public async Task SplitDoubleImplicit() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < N; i++)
            {
                double[] r = cr.Split;
                r.Should().Equal(new double[] { rnd.Next(), rnd.Next(), rnd.Next() });
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Split")]
        public async Task SplitAscii() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < N; i++)
            {
                cr.Split.Ascii.Should().Equal(new string[] { rnd.Next().ToString(), rnd.Next().ToString(), rnd.Next().ToString() });
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Split")]
        public async Task SplitAsciiImplicit() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < N; i++)
            {
                string[] r = cr.Split;
                r.Should().Equal(new string[] { rnd.Next().ToString(), rnd.Next().ToString(), rnd.Next().ToString() });
            }
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Split")]
        public async Task SplitString() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < N; i++)
            {
                cr.Split.String.Should().Equal(new string[] { rnd.Next().ToString(), rnd.Next().ToString(), rnd.Next().ToString() });
            }
        });
        #endregion


        #region Repeat
        [Fact(Timeout = 1000)]
        [Trait("Category", "Repeat")]
        public async Task RepeatInt() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            var r = cr.Repeat(3 * N).Int;
            r.Should().Equal(Enumerable.Repeat(new Random(42), 3 * N).Select(rnd => rnd.Next()));
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Repeat")]
        public async Task RepeatIntImplicit() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            int[] r = cr.Repeat(3 * N);
            r.Should().Equal(Enumerable.Repeat(new Random(42), 3 * N).Select(rnd => rnd.Next()));
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Repeat")]
        public async Task RepeatInt0() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            cr.Repeat(3 * N).Int0.Should().Equal(Enumerable.Repeat(new Random(42), 3 * N).Select(rnd => rnd.Next() - 1));
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Repeat")]
        public async Task RepeatLong() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            cr.Repeat(3 * N).Long.Should().Equal(Enumerable.Repeat(new Random(42), 3 * N).Select(rnd => rnd.Next()));
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Repeat")]
        public async Task RepeatLongImplicit() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            long[] r = cr.Repeat(3 * N);
            r.Should().Equal(Enumerable.Repeat(new Random(42), 3 * N).Select(rnd => (long)rnd.Next()));
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Repeat")]
        public async Task RepeatLong0() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            cr.Repeat(3 * N).Long0.Should().Equal(Enumerable.Repeat(new Random(42), 3 * N).Select(rnd => (long)rnd.Next() - 1));
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Repeat")]
        public async Task RepeatDouble() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            cr.Repeat(3 * N).Double.Should().Equal(Enumerable.Repeat(new Random(42), 3 * N).Select(rnd => rnd.Next()));
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Repeat")]
        public async Task RepeatDoubleImplicit() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            double[] r = cr.Repeat(3 * N);
            r.Should().Equal(Enumerable.Repeat(new Random(42), 3 * N).Select(rnd => rnd.Next()));
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Repeat")]
        public async Task RepeatAscii() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            cr.Repeat(3 * N).Ascii.Should().Equal(Enumerable.Repeat(new Random(42), 3 * N).Select(rnd => rnd.Next().ToString()));
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Repeat")]
        public async Task RepeatAsciiImplicit() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            string[] r = cr.Repeat(3 * N);
            r.Should().Equal(Enumerable.Repeat(new Random(42), 3 * N).Select(rnd => rnd.Next().ToString()));
        });

        [Fact(Timeout = 1000)]
        [Trait("Category", "Repeat")]
        public async Task RepeatString() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            cr.Repeat(3 * N).String.Should().Equal(Enumerable.Repeat(new Random(42), 3 * N).Select(rnd => rnd.Next().ToString()));
        });
        #endregion

        [Fact(Timeout = 1000)]
        [Trait("Category", "Mix")]
        public async Task Mix() => await Task.Run(() =>
        {
            var rnd = new Random(42);
            for (int i = 0; i < 3 * N; i++)
            {
                switch (i % 6)
                {
                    case 0:
                        cr.Int.Should().Be(rnd.Next());
                        break;
                    case 1:
                        cr.Int0.Should().Be(rnd.Next() - 1);
                        break;
                    case 2:
                        cr.Long.Should().Be(rnd.Next());
                        break;
                    case 3:
                        cr.Long0.Should().Be(rnd.Next() - 1);
                        break;
                    case 4:
                        cr.Ascii.Should().Be(rnd.Next().ToString());
                        break;
                    default:
                        cr.String.Should().Be(rnd.Next().ToString());
                        break;
                }
            }
        });
    }
}
