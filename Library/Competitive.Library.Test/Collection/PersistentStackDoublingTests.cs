using System.Linq;

namespace Kzrnm.Competitive.Testing.Collection
{
    public class PersistentStackDoublingTests
    {
        [Fact]
        public void Stack()
        {
            var ss = new PersistentStackDoubling<int>[100];
            ss[0] = PersistentStackDoubling<int>.Empty;
            ss[0].ShouldBeEmpty();
            for (int i = 1; i < ss.Length; i++)
            {
                ss[i] = ss[i - 1].Push(i);
                ss[i].ShouldBe(Enumerable.Range(1, i).Reverse());
                ss[i].Count.ShouldBe(i);
            }

            for (int i = ss.Length - 1; i >= 0; i--)
            {
                ss[i].ShouldBe(Enumerable.Range(1, i).Reverse());
                ss[i].Count.ShouldBe(i);

                if (i > 0)
                {
                    var other1 = ss[i].Pop();
                    other1.ShouldBe(Enumerable.Range(1, i - 1).Reverse());
                    other1.Count.ShouldBe(i - 1);
                }

                var add2 = ss[i].Push(i).Push(i);
                add2.Count.ShouldBe(i + 2);
                add2.ShouldBe(Enumerable.Range(1, i).Append(i).Append(i).OrderBy(i => i).Reverse());
            }

            ss[50].Push(-1).Push(-3).ShouldBe(new[] { -3, -1 }.Concat(Enumerable.Range(1, 50).Reverse()));
        }
        [Fact]
        public void Clear()
        {
            var s = PersistentStackDoubling<int>.Empty;
            s.ShouldBeEmpty();
            for (int i = 0; i < 100; i++)
            {
                s.Clear().ShouldBeSameAs(PersistentStackDoubling<int>.Empty);
                s = s.Push(i);
                s.Clear().ShouldBeSameAs(PersistentStackDoubling<int>.Empty);
            }
        }

        [Fact]
        public void Indexer()
        {
            var s = PersistentStackDoubling<int>.Empty;
            s.ShouldBeEmpty();
            for (int i = 0; i < 100; i++)
            {
                s = s.Push(i);
                for (int ix = 0; ix <= i; ix++)
                {
                    s[ix].ShouldBe(i - ix);
                }
            }
        }
    }
}
