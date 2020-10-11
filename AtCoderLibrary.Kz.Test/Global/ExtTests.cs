using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace AtCoder
{
    public class ExtTests
    {
        [Fact]
        public void UpdateMax()
        {
            int a = 0;
            a.UpdateMax(10).Should().BeTrue();
            a.Should().Be(10);
            a.UpdateMax(0).Should().BeFalse();
            a.Should().Be(10);

            DateTime d = new DateTime(2000, 1, 1);
            d.UpdateMax(new DateTime(2001, 1, 1)).Should().BeTrue();
            d.Should().Be(new DateTime(2001, 1, 1));
            d.UpdateMax(new DateTime(2000, 12, 1)).Should().BeFalse();
            d.Should().Be(new DateTime(2001, 1, 1));
        }

        [Fact]
        public void UpdateMin()
        {
            int a = 0;
            a.UpdateMin(-10).Should().BeTrue();
            a.Should().Be(-10);
            a.UpdateMin(0).Should().BeFalse();
            a.Should().Be(-10);

            DateTime d = new DateTime(2000, 1, 1);
            d.UpdateMin(new DateTime(1999, 1, 1)).Should().BeTrue();
            d.Should().Be(new DateTime(1999, 1, 1));
            d.UpdateMin(new DateTime(2000, 12, 1)).Should().BeFalse();
            d.Should().Be(new DateTime(1999, 1, 1));
        }

        [Fact]
        public void Fill()
        {
            new string[100].Fill("🦈").Should().Equal(Enumerable.Repeat("🦈", 100));
        }

        [Fact]
        public void Sort()
        {
            var arr = Enumerable.Repeat(new Random(), 2000).Select(r => r.Next()).ToArray();
            MemoryMarshal.Cast<int, long>(arr).ToArray().Sort().Should().BeInAscendingOrder(Comparer<long>.Default);
        }

        [Fact]
        public void SortString()
        {
            new string[] {
                "zzz14144",
                "aBc",
                "AB",
                "a",
                "dsjkf50000",
                "BCD443",
                "aaa31",
            }.Sort().Should().Equal(new string[] {
                "AB",
                "BCD443",
                "a",
                "aBc",
                "aaa31",
                "dsjkf50000",
                "zzz14144",
            });
        }

        [Fact]
        public void SortExpression()
        {
            new string[] {
                "zzz14144",
                "aBc",
                "AB",
                "a",
                "dsjkf50000",
                "BCD443",
                "aaa31",
            }.Sort(s => s.Length).Should().Equal(new string[] {
                "a",
                "AB",
                "aBc",
                "aaa31",
                "BCD443",
                "zzz14144",
                "dsjkf50000",
            });
        }

        [Fact]
        public void SortComparison()
        {
            new string[] {
                "zzz14144",
                "aBc",
                "AB",
                "a",
                "dsjkf50000",
                "BCD443",
                "aaa31",
            }.Sort((s1, s2) => s1.Length.CompareTo(s2.Length)).Should().Equal(new string[] {
                "a",
                "AB",
                "aBc",
                "aaa31",
                "BCD443",
                "zzz14144",
                "dsjkf50000",
            });
        }

        [Fact]
        public void SortComparer()
        {
            new string[] {
                "zzz14144",
                "aBc",
                "AB",
                "a",
                "dsjkf50000",
                "BCD443",
                "aaa31",
            }.Sort(StringComparer.OrdinalIgnoreCase).Should().Equal(new string[] {
                "a",
                "aaa31",
                "AB",
                "aBc",
                "BCD443",
                "dsjkf50000",
                "zzz14144",
            });
        }

        [Fact]
        public void Reverse()
        {
            new string[] {
                "zzz14144",
                "aBc",
                "AB",
                "a",
                "dsjkf50000",
                "BCD443",
                "aaa31",
            }.Reverse().Should().Equal(new string[] {
                "aaa31",
                "BCD443",
                "dsjkf50000",
                "a",
                "AB",
                "aBc",
                "zzz14144",
            });
        }

        [Fact]
        public void MaxBy()
        {
            new long[] {
                43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
            }.MaxBy().Should().Be((17, 3401434));
        }

        [Fact]
        public void MaxByArrayFunc()
        {
            new (int, long)[] {
                (0,43),(0,24),(0,8373),(0,4),(0,98),(0,7),(0,43),(0,28),(0,9470),(0,71),(0,431),(0,45),(0,23014),(0,345),(0,23614),(0,1503),(0,7),(0,3401434),(0,120),(0,42314),(0,3123)
            }.MaxBy(tup => tup.Item2).Should().Be((17, (0, 3401434)));
        }

        [Fact]
        public void MaxByFunc()
        {
            new List<(int, long)> {
                (0,43),(0,24),(0,8373),(0,4),(0,98),(0,7),(0,43),(0,28),(0,9470),(0,71),(0,431),(0,45),(0,23014),(0,345),(0,23614),(0,1503),(0,7),(0,3401434),(0,120),(0,42314),(0,3123)
            }.MaxBy(tup => tup.Item2).Should().Be(((0, 3401434), 3401434));
        }


        [Fact]
        public void MinBy()
        {
            new long[] {
                43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
            }.MinBy().Should().Be((3, 4));
        }

        [Fact]
        public void MinByArrayFunc()
        {
            new (int, long)[] {
                (0,43),(0,24),(0,8373),(0,4),(0,98),(0,7),(0,43),(0,28),(0,9470),(0,71),(0,431),(0,45),(0,23014),(0,345),(0,23614),(0,1503),(0,7),(0,3401434),(0,120),(0,42314),(0,3123)
            }.MinBy(tup => tup.Item2).Should().Be((3, (0, 4)));
        }

        [Fact]
        public void MinByFunc()
        {
            new List<(int, long)> {
                (0,43),(0,24),(0,8373),(0,4),(0,98),(0,7),(0,43),(0,28),(0,9470),(0,71),(0,431),(0,45),(0,23014),(0,345),(0,23614),(0,1503),(0,7),(0,3401434),(0,120),(0,42314),(0,3123)
            }.MinBy(tup => tup.Item2).Should().Be(((0, 4), 4));
        }

        [Fact]
        public void GroupCount()
        {
            new[] {
                StringComparison.OrdinalIgnoreCase,
                StringComparison.OrdinalIgnoreCase,
                StringComparison.InvariantCulture,
                StringComparison.InvariantCultureIgnoreCase,
                StringComparison.CurrentCulture,
                StringComparison.CurrentCulture,
                StringComparison.CurrentCulture,
                StringComparison.OrdinalIgnoreCase,
                StringComparison.Ordinal,
                StringComparison.OrdinalIgnoreCase,
                StringComparison.InvariantCulture,
            }.GroupCount().Should().BeEquivalentTo(new Dictionary<StringComparison, int>
            {
                { StringComparison.Ordinal,1 },
                { StringComparison.OrdinalIgnoreCase,4 },
                { StringComparison.CurrentCulture,3 },
                { StringComparison.InvariantCulture,2 },
                { StringComparison.InvariantCultureIgnoreCase,1 },
            });
        }

        [Fact]
        public void GroupCountFunc()
        {
            new long[] {
                43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
            }.GroupCount(i => i % 7).Should().BeEquivalentTo(new Dictionary<long, int>
            {
                { 0, 4 },
                { 1, 7 },
                { 2, 1 },
                { 3, 3 },
                { 4, 2 },
                { 5, 2 },
                { 6, 2 },
            });
        }

        [Fact]
        public void ListAsSpan()
        {
            new List<long> {
                43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
            }.AsSpan()[18..].ToArray().Should().Equal(new long[] { 120, 42314, 3123 });
        }

        [Fact]
        public void ArrayGet()
        {
            var arr = new long[] {
                43,24,8373,4,98,7,43,28,9470,71,431,45,23014,345,23614,1503,7,3401434,120,42314,3123
            };
            arr.Get(1).Should().Be(24);
            arr.Get(1) = 25;
            arr[1].Should().Be(25);

            arr.Get(-1).Should().Be(3123);
            arr.Get(-1) = -2;
            arr[^1].Should().Be(-2);
        }

        [Fact]
        public void DicGet()
        {
            var dic = new Dictionary<string, int> {
                {"foo",1 },
                {"bar",-1 },
                {"😀",10 },
                {"でぃ",0 },
            };
            dic.Get("foo").Should().Be(1);
            dic.Get("bar").Should().Be(-1);
            dic.Get("😀").Should().Be(10);
            dic.Get("でぃ").Should().Be(0);
            dic.Get("invalid").Should().Be(0);
        }
    }
}