using System;
using System.Linq;

namespace Kzrnm.Competitive.Testing.Extensions
{
    public class UpdateExtensionTests
    {
        [Fact]
        public void UpdateMax()
        {
            int a = 0;
            a.UpdateMax(10).ShouldBeTrue();
            a.ShouldBe(10);
            a.UpdateMax(0).ShouldBeFalse();
            a.ShouldBe(10);

            var d = new DateTime(2000, 1, 1);
            d.UpdateMax(new DateTime(2001, 1, 1)).ShouldBeTrue();
            d.ShouldBe(new DateTime(2001, 1, 1));
            d.UpdateMax(new DateTime(2000, 12, 1)).ShouldBeFalse();
            d.ShouldBe(new DateTime(2001, 1, 1));
        }

        [Fact]
        public void UpdateMin()
        {
            int a = 0;
            a.UpdateMin(-10).ShouldBeTrue();
            a.ShouldBe(-10);
            a.UpdateMin(0).ShouldBeFalse();
            a.ShouldBe(-10);

            DateTime d = new(2000, 1, 1);
            d.UpdateMin(new DateTime(1999, 1, 1)).ShouldBeTrue();
            d.ShouldBe(new DateTime(1999, 1, 1));
            d.UpdateMin(new DateTime(2000, 12, 1)).ShouldBeFalse();
            d.ShouldBe(new DateTime(1999, 1, 1));
        }


        [Fact]
        public void UpdateValues()
        {
            var rnd = new Random(227);
            for (int i = 0; i < 1000; i++)
            {
                var bytes = new byte[20];
                rnd.NextBytes(bytes);
                bytes[0] = byte.MaxValue >> 1;
                byte num;
                {
                    num = 0;
                    num.UpdateMin(bytes).ShouldBeFalse();
                    num.ShouldBe(default);
                }
                {
                    num = 0;
                    num.UpdateMax(bytes).ShouldBeTrue();
                    num.ShouldBe(bytes.Max());
                }
                {
                    num = byte.MaxValue;
                    num.UpdateMin(bytes).ShouldBeTrue();
                    num.ShouldBe(bytes.Min());
                }
                {
                    num = byte.MaxValue;
                    num.UpdateMax(bytes).ShouldBeFalse();
                    num.ShouldBe(byte.MaxValue);
                }
            }
        }
    }
}