using FluentAssertions;
using System;

namespace Kzrnm.Competitive.Testing.Extensions
{
    public class UpdateExtensionTests
    {
        [Fact]
        public void UpdateMax()
        {
            int a = 0;
            a.UpdateMax(10).Should().BeTrue();
            a.Should().Be(10);
            a.UpdateMax(0).Should().BeFalse();
            a.Should().Be(10);

            var d = new DateTime(2000, 1, 1);
            d.UpdateMax(new DateTime(2001, 1, 1)).Should().BeTrue();
            d.Should().Be(new DateTime(2001, 1, 1));
            d.UpdateMax(new DateTime(2000, 12, 1)).Should().BeFalse();
            d.Should().Be(new DateTime(2001, 1, 1));
        }

        [Fact]
        public void UpdateMin()
        {
            int a = 0;
            a.UpdateMin(-10).Should().BeTrue();
            a.Should().Be(-10);
            a.UpdateMin(0).Should().BeFalse();
            a.Should().Be(-10);

            DateTime d = new(2000, 1, 1);
            d.UpdateMin(new DateTime(1999, 1, 1)).Should().BeTrue();
            d.Should().Be(new DateTime(1999, 1, 1));
            d.UpdateMin(new DateTime(2000, 12, 1)).Should().BeFalse();
            d.Should().Be(new DateTime(1999, 1, 1));
        }
    }
}