using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kzrnm.Competitive.Testing.Util
{
    [Verify] // verification-helper: PROBLEM https://judge.yosupo.jp/problem/aplusb
    public class ZahyoCompressTests
    {
        [Fact]
        public void Compress()
        {
            var c = ZahyoCompress.Create("compress").Compress();
            var (decN, decO) = c;
            decN.Should().BeSameAs(c.NewTable);
            decO.Should().BeSameAs(c.Original);

            c.NewTable.Should().Equal(new Dictionary<char, int>
            {
                { 'c',0 },
                { 'o',3 },
                { 'm',2 },
                { 'p',4 },
                { 'r',5 },
                { 'e',1 },
                { 's',6 },
            });
            c.Original.Should().Equal('c', 'e', 'm', 'o', 'p', 'r', 's');
            c.Replace("compress").Should().Equal(0, 3, 2, 4, 5, 1, 6, 6);

            c.Compress(new ReverseComparerClass<char>());
            c.NewTable.Should().Equal(new Dictionary<char, int>
            {
                { 'c',6 },
                { 'o',3 },
                { 'm',4 },
                { 'p',2 },
                { 'r',1 },
                { 'e',5 },
                { 's',0 },
            });
            c.Original.Should().Equal('s', 'r', 'p', 'o', 'm', 'e', 'c');
            c.Replace("compress").Should().Equal(6, 3, 4, 2, 1, 5, 0, 0);

            c.Add('i');
            c.Add('r');
            c.Add('k');
            c.Compress();
            c.NewTable.Should().Equal(new Dictionary<char, int>
            {
                { 'c',0 },
                { 'o',5 },
                { 'm',4 },
                { 'p',6 },
                { 'r',7 },
                { 'e',1 },
                { 's',8 },
                { 'i',2 },
                { 'k',3 },
            });
            c.Original.Should().Equal('c', 'e', 'i', 'k', 'm', 'o', 'p', 'r', 's');
        }

        [Fact]
        public void CompressedArray()
        {
            ZahyoCompress.CompressedArray("compress".AsSpan()).Should().Equal(0, 3, 2, 4, 5, 1, 6, 6);
            ZahyoCompress.CompressedArray(new[] { 3, 5, 6, 41, 6 }).Should().Equal(0, 1, 2, 3, 2);
        }
    }
}
