using System;
using System.Collections.Generic;

namespace Kzrnm.Competitive.Testing.Algorithm
{
    public class ZahyoCompressTests
    {
        [Fact]
        public void Compress()
        {
            var c = ZahyoCompress.Create("compress").Compress();
            var (decN, decO) = c;
            decN.ShouldBeSameAs(c.NewTable);
            decO.ShouldBeSameAs(c.Original);

            c.NewTable.ShouldSatisfyAllConditions([
                t => t.Count.ShouldBe(7),
                t => t.ShouldContainKeyAndValue('c', 0),
                t => t.ShouldContainKeyAndValue('o', 3),
                t => t.ShouldContainKeyAndValue('m', 2),
                t => t.ShouldContainKeyAndValue('p', 4),
                t => t.ShouldContainKeyAndValue('r', 5),
                t => t.ShouldContainKeyAndValue('e', 1),
                t => t.ShouldContainKeyAndValue('s', 6),
            ]);
            c.Original.ShouldBe(['c', 'e', 'm', 'o', 'p', 'r', 's']);
            c.Replace("compress").ShouldBe([0, 3, 2, 4, 5, 1, 6, 6]);

            c.Compress(new ReverseComparerStruct<char>());
            c.NewTable.ShouldSatisfyAllConditions([
                t => t.Count.ShouldBe(7),
                t => t.ShouldContainKeyAndValue('c', 6),
                t => t.ShouldContainKeyAndValue('o', 3),
                t => t.ShouldContainKeyAndValue('m', 4),
                t => t.ShouldContainKeyAndValue('p', 2),
                t => t.ShouldContainKeyAndValue('r', 1),
                t => t.ShouldContainKeyAndValue('e', 5),
                t => t.ShouldContainKeyAndValue('s', 0),
            ]);
            c.Original.ShouldBe(['s', 'r', 'p', 'o', 'm', 'e', 'c']);
            c.Replace("compress").ShouldBe([6, 3, 4, 2, 1, 5, 0, 0]);

            c.Add('i');
            c.Add('r');
            c.Add('k');
            c.Compress();
            c.NewTable.ShouldSatisfyAllConditions([
                t => t.Count.ShouldBe(9),
                t => t.ShouldContainKeyAndValue('c', 0),
                t => t.ShouldContainKeyAndValue('o', 5),
                t => t.ShouldContainKeyAndValue('m', 4),
                t => t.ShouldContainKeyAndValue('p', 6),
                t => t.ShouldContainKeyAndValue('r', 7),
                t => t.ShouldContainKeyAndValue('e', 1),
                t => t.ShouldContainKeyAndValue('s', 8),
                t => t.ShouldContainKeyAndValue('i', 2),
                t => t.ShouldContainKeyAndValue('k', 3),
            ]);
            c.Original.ShouldBe(['c', 'e', 'i', 'k', 'm', 'o', 'p', 'r', 's']);
        }

        [Fact]
        public void CompressedArray()
        {
            ZahyoCompress.CompressedArray("compress".AsSpan()).ShouldBe([0, 3, 2, 4, 5, 1, 6, 6]);
            ZahyoCompress.CompressedArray(new[] { 3, 5, 6, 41, 6 }).ShouldBe([0, 1, 2, 3, 2]);
        }
    }
}
