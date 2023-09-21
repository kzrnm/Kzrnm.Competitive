using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Collection
{
    public class PersistentSetTests
    {
        [Fact]
        public void Set()
        {
            var ss = new PersistentSet<int>[100];
            ss[0] = PersistentSet<int>.Empty();
            ss[0].Should().BeEmpty();
            for (int i = 1; i < ss.Length; i++)
            {
                ss[i] = ss[i - 1].Add(i);
                ss[i].Should().Equal(Enumerable.Range(1, i));
                ss[i].Count.Should().Be(i);
                ss[i].Reverse().Should().Equal(Enumerable.Range(1, i).Reverse());
            }

            for (int i = ss.Length - 1; i >= 0; i--)
            {
                ss[i].Should().Equal(Enumerable.Range(1, i));
                ss[i].Count.Should().Be(i);

                var other0 = ss[i].Remove(0);
                other0.Should().Equal(Enumerable.Range(1, i));
                other0.Count.Should().Be(i);

                if (i > 0)
                {
                    var other1 = ss[i].Remove(1);
                    other1.Should().Equal(Enumerable.Range(2, i - 1));
                    other1.Count.Should().Be(i - 1);
                }

                var add2 = ss[i].Add(i).Add(i);
                add2.Count.Should().Be(i + 2);
                add2.Should().Equal(Enumerable.Range(1, i).Append(i).Append(i).OrderBy(i => i).ToArray());
            }
        }
        [Fact]
        public void SetDictionary()
        {
            var ss = new PersistentSetDictionary<int, string>[100];
            ss[0] = PersistentSetDictionary<int, string>.Empty();
            ss[0].Should().BeEmpty();
            for (int i = 1; i < ss.Length; i++)
            {
                ss[i] = ss[i - 1].Add(i, i.ToString());
                ss[i].Should().Equal(Enumerable.Range(1, i).Select(i => KeyValuePair.Create(i, i.ToString())));
                ss[i].Count.Should().Be(i);
                ss[i].Reverse().Should().Equal(Enumerable.Range(1, i).Select(i => KeyValuePair.Create(i, i.ToString())).Reverse());
            }

            for (int i = ss.Length - 1; i >= 0; i--)
            {
                ss[i].Should().Equal(Enumerable.Range(1, i).Select(i => KeyValuePair.Create(i, i.ToString())));
                ss[i].Count.Should().Be(i);

                var other0 = ss[i].Remove(0);
                other0.Should().Equal(Enumerable.Range(1, i).Select(i => KeyValuePair.Create(i, i.ToString())));
                other0.Count.Should().Be(i);

                if (i > 0)
                {
                    var other1 = ss[i].Remove(1);
                    other1.Should().Equal(Enumerable.Range(2, i - 1).Select(i => KeyValuePair.Create(i, i.ToString())));
                    other1.Count.Should().Be(i - 1);
                }

                var add2 = ss[i].Add(i, i.ToString()).Add(i, i.ToString());
                add2.Count.Should().Be(i + 2);
                add2.Should().Equal(Enumerable.Range(1, i).Append(i).Append(i).OrderBy(i => i).Select(i => KeyValuePair.Create(i, i.ToString())).ToArray());
            }
        }


        [Fact]
        public void RemoveMinMax()
        {
            var sd = PersistentSetDictionary<int, int>.Empty(isMulti: true);
            sd.Should().Equal();
            sd = sd.Add(1, 1);
            sd = sd.Add(1, 2);
            sd = sd.Add(2, 1);
            sd = sd.Add(2, 2);
            sd.Should().Equal(new[] {
                KeyValuePair.Create(1, 2),
                KeyValuePair.Create(1, 1),
                KeyValuePair.Create(2, 2),
                KeyValuePair.Create(2, 1),
            });

            sd.Min.Should().Be(KeyValuePair.Create(1, 2));
            sd.RemoveMin(out var min).Should().Equal(new[] {
                KeyValuePair.Create(1, 1),
                KeyValuePair.Create(2, 2),
                KeyValuePair.Create(2, 1),
            });
            min.Should().Be(KeyValuePair.Create(1, 2));

            sd.Max.Should().Be(KeyValuePair.Create(2, 1));
            sd.RemoveMax(out var max).Should().Equal(new[] {
                KeyValuePair.Create(1, 2),
                KeyValuePair.Create(1, 1),
                KeyValuePair.Create(2, 2),
            });
            max.Should().Be(KeyValuePair.Create(2, 1));
        }
    }
}
