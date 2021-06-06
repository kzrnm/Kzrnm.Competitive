using FluentAssertions;
using System;
using System.Numerics;
using Xunit;

namespace Kzrnm.Competitive.GlobalNS
{
    public class BinarySearchGenericExTests
    {
        private struct FloatFull : IBinaryOk<float>
        {
            public float th;
            public bool Continue(float ok, float ng) => Math.Abs(ok - ng) > 50;
            public float Mid(float ok, float ng) => (ok + ng) / 2;

            public bool Ok(float value) => value < th;
        }
        [Fact]
        public void BinaryOkDefault()
        {
            __BinarySearchGenericEx.BinarySearch<float, FloatFull>(-1000000000F, 0F).Should().Be(-29.802322F);
            __BinarySearchGenericEx.BinarySearch<float, FloatFull>(-1000000000F, 10F).Should().Be(-19.802324F);
        }
        [Fact]
        public void BinaryOkArg()
        {
            __BinarySearchGenericEx.BinarySearch(-1000000000F, 1F, new FloatFull { th = 0.5F }).Should().Be(-28.802324F);
            __BinarySearchGenericEx.BinarySearch(-1000000000F, 10F, new FloatFull { th = 0.5F }).Should().Be(-19.802324F);
        }
    }
}
