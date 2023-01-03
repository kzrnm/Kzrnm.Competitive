using System.Linq;

namespace Kzrnm.Competitive.Testing.Collection
{
    public class PersistentStackTests
    {
        [Fact]
        public void Stack()
        {
            var ss = new PersistentStack<int>[100];
            ss[0] = PersistentStack<int>.Empty;
            ss[0].Should().BeEmpty();
            for (int i = 1; i < ss.Length; i++)
            {
                ss[i] = ss[i - 1].Push(i);
                ss[i].Should().Equal(Enumerable.Range(1, i).Reverse());
                ss[i].Count.Should().Be(i);
            }

            for (int i = ss.Length - 1; i >= 0; i--)
            {
                ss[i].Should().Equal(Enumerable.Range(1, i).Reverse());
                ss[i].Count.Should().Be(i);

                if (i > 0)
                {
                    var other1 = ss[i].Pop();
                    other1.Should().Equal(Enumerable.Range(1, i - 1).Reverse());
                    other1.Count.Should().Be(i - 1);
                }

                var add2 = ss[i].Push(i).Push(i);
                add2.Count.Should().Be(i + 2);
                add2.Should().Equal(Enumerable.Range(1, i).Append(i).Append(i).OrderBy(i => i).Reverse());
            }

            ss[50].Push(-1).Push(-3).Should().Equal(new[] { -3, -1 }.Concat(Enumerable.Range(1, 50).Reverse()));
        }
        [Fact]
        public void Clear()
        {
            var s = PersistentStack<int>.Empty;
            s.Should().BeEmpty();
            for (int i = 0; i < 100; i++)
            {
                s.Clear().Should().BeSameAs(PersistentStack<int>.Empty);
                s = s.Push(i);
                s.Clear().Should().BeSameAs(PersistentStack<int>.Empty);
            }
        }
    }
}
