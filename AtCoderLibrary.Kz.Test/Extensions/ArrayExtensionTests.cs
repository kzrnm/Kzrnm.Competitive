using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace AtCoder
{
    public class ArrayExtensionTests
    {
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
    }
}