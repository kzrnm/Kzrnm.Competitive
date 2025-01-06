using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kzrnm.Competitive.Testing.GlobalNS
{
    public class TensTests
    {
        [Fact]
        public void Ints()
        {
            Tens.Ints[0].Should().Be(1);
            for (int i = 1; i < Tens.Ints.Length; i++)
            {
                Tens.Ints[i].Should().Be(Tens.Ints[i - 1] * 10);
            }
        }
        [Fact]
        public void Longs()
        {
            Tens.Longs[0].Should().Be(1);
            for (int i = 1; i < Tens.Longs.Length; i++)
            {
                Tens.Longs[i].Should().Be(Tens.Longs[i - 1] * 10);
            }
        }
    }
}
