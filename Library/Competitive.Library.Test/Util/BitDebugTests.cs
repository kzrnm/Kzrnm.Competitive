using Shouldly;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Util
{
    public class BitDebugTests
    {
        [Fact]
        public void IntArray()
        {
            var bd = new BitDebug(Enumerable.Range(0, 10).ToArray());
            bd.Items.Length.ShouldBe(10);
            bd.Items[0].ShouldBe(new("0000 [0]", 0));
            bd.Items[1].ShouldBe(new("0001 [1]", 1));
            bd.Items[2].ShouldBe(new("0010 [2]", 2));
            bd.Items[3].ShouldBe(new("0011 [3]", 3));
            bd.Items[4].ShouldBe(new("0100 [4]", 4));
            bd.Items[5].ShouldBe(new("0101 [5]", 5));
            bd.Items[6].ShouldBe(new("0110 [6]", 6));
            bd.Items[7].ShouldBe(new("0111 [7]", 7));
            bd.Items[8].ShouldBe(new("1000 [8]", 8));
            bd.Items[9].ShouldBe(new("1001 [9]", 9));
        }
        [Fact]
        public void StringArray()
        {
            var bd = new BitDebug(Enumerable.Range(0, 6).Select(n => n.ToString()).ToArray());
            bd.Items.Length.ShouldBe(6);
            bd.Items[0].ShouldBe(new("000 [0]", "0"));
            bd.Items[1].ShouldBe(new("001 [1]", "1"));
            bd.Items[2].ShouldBe(new("010 [2]", "2"));
            bd.Items[3].ShouldBe(new("011 [3]", "3"));
            bd.Items[4].ShouldBe(new("100 [4]", "4"));
            bd.Items[5].ShouldBe(new("101 [5]", "5"));
        }
    }
}
