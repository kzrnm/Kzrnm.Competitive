using AtCoder;
using FluentAssertions;

namespace Kzrnm.Competitive.Testing.Extensions
{
    public class TwoSatExtensionTests
    {
        [Fact]
        public void And_all()
        {
            var twoSat = new TwoSat(3);
            twoSat.And(0, true, 1, true);
            twoSat.And(2, true, 1, true);

            twoSat.Satisfiable().Should().BeTrue();
            twoSat.Answer().Should().Equal(true, true, true);
        }
        [Fact]
        public void And_set1()
        {
            var twoSat = new TwoSat(3);
            twoSat.And(0, true, 1, true);
            twoSat.Set(1, false);

            twoSat.Satisfiable().Should().BeFalse();
        }
        [Fact]
        public void And_set2()
        {
            var twoSat = new TwoSat(3);
            twoSat.And(0, true, 1, true);
            twoSat.Set(2, false);

            twoSat.Satisfiable().Should().BeTrue();
            twoSat.Answer().Should().Equal(true, true, false);
        }
        [Fact]
        public void Same()
        {
            var twoSat = new TwoSat(3);
            twoSat.Same(0, 1);
            twoSat.And(1, true, 2, false);

            twoSat.Satisfiable().Should().BeTrue();
            twoSat.Answer().Should().Equal(true, true, false);
        }
        [Fact]
        public void NotSame()
        {
            var twoSat = new TwoSat(3);
            twoSat.NotSame(0, 1);
            twoSat.And(1, true, 2, false);

            twoSat.Satisfiable().Should().BeTrue();
            twoSat.Answer().Should().Equal(false, true, false);
        }
        [Fact]
        public void IfThen1()
        {
            var twoSat = new TwoSat(3);
            twoSat.IfThen(0, true, 1, false);
            twoSat.And(0, true, 2, false);

            twoSat.Satisfiable().Should().BeTrue();
            twoSat.Answer().Should().Equal(true, false, false);
        }
        [Fact]
        public void IfThen2()
        {
            var twoSat = new TwoSat(3);
            twoSat.IfThen(0, true, 1, false);
            twoSat.And(1, true, 2, false);

            twoSat.Satisfiable().Should().BeTrue();
            twoSat.Answer().Should().Equal(false, true, false);
        }
    }
}