using FluentAssertions;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Util
{
    public class BitDebugTests
    {
        [Fact]
        public void IntArray()
        {
            var bd = new BitDebug(Enumerable.Range(0, 10).ToArray());
            bd.Items.Should().HaveCount(10);
            bd.Items[0].Should().Be("0000 [0]", 0);
            bd.Items[1].Should().Be("0001 [1]", 1);
            bd.Items[2].Should().Be("0010 [2]", 2);
            bd.Items[3].Should().Be("0011 [3]", 3);
            bd.Items[4].Should().Be("0100 [4]", 4);
            bd.Items[5].Should().Be("0101 [5]", 5);
            bd.Items[6].Should().Be("0110 [6]", 6);
            bd.Items[7].Should().Be("0111 [7]", 7);
            bd.Items[8].Should().Be("1000 [8]", 8);
            bd.Items[9].Should().Be("1001 [9]", 9);
        }
        [Fact]
        public void StringArray()
        {
            var bd = new BitDebug(Enumerable.Range(0, 6).Select(n => n.ToString()).ToArray());
            bd.Items.Should().HaveCount(6);
            bd.Items[0].Should().Be("000 [0]", "0");
            bd.Items[1].Should().Be("001 [1]", "1");
            bd.Items[2].Should().Be("010 [2]", "2");
            bd.Items[3].Should().Be("011 [3]", "3");
            bd.Items[4].Should().Be("100 [4]", "4");
            bd.Items[5].Should().Be("101 [5]", "5");
        }
    }

    internal static class BitDebugDebugItemAssertionsExtension
    {
        public static BitDebugDebugItemAssertions Should(this BitDebug.DebugItem item) => new(item);
    }
    internal class BitDebugDebugItemAssertions : FluentAssertions.Primitives.ObjectAssertions<BitDebug.DebugItem, BitDebugDebugItemAssertions>
    {
        public BitDebugDebugItemAssertions(BitDebug.DebugItem value) : base(value) { }

        [CustomAssertion]
        public void Be(string key, object value, string because = "", params object[] becauseArgs)
        {
            Subject.key.Should().Be(key, because: because, becauseArgs: becauseArgs);
            Subject.value.Should().Be(value, because: because, becauseArgs: becauseArgs);
        }
    }
}
