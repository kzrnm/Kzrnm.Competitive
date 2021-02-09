using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kzrnm.Competitive.Extension
{
    public class MyLinqExtensionTests
    {
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
    }
}