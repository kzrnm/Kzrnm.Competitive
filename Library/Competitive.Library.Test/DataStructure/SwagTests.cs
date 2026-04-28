using AtCoder;
using AtCoder.Internal;

namespace Kzrnm.Competitive.Testing.DataStructure;

public class SwagTests
{
    Random rnd = new(227);
    Mod998244353AffineTransformation RandomAffine(Random rnd) => new(rnd.Next(), rnd.Next());
    int RandomInt32(Random rnd) => rnd.Next();

    [Test, MultipleAssertions] public async Task AffineTransformation() => await new Inner<Mod998244353AffineTransformation, AffineTransformationOp>(rnd).RandomTest(RandomAffine);
    [Test, MultipleAssertions] public async Task SlideMin() => await new Inner<int, SlideMinOp>(rnd).RandomTest(RandomInt32);
    [Test, MultipleAssertions] public async Task SlideMax() => await new Inner<int, SlideMaxOp>(rnd).RandomTest(RandomInt32);

    record Inner<T, TOp>(Random Rnd) where TOp : struct, ISegtreeOperator<T>
    {
        public async Task RandomTest(Func<Random, T> nextProvider)
        {
            var op = new TOp();
            var swag = new Swag<T, TOp>();
            var deque = new Deque<T>();

            await swag.AllProd.Should().BeEqualTo(op.Identity);
            await Assert.That(swag.Pop).Throws<ContractAssertException>()
                .WithMessage("data is empty.");

            for (int i = 0; i < 200; i++)
            {
                var next = nextProvider(Rnd);
                deque.AddLast(next); swag.Push(next);
                await swag.AllProd.Should().BeEqualTo(deque.Aggregate(op.Operate));
            }

            for (int i = 1; i < 200; i++)
            {
                deque.PopFirst(); swag.Pop();
                await swag.AllProd.Should().BeEqualTo(deque.Aggregate(op.Operate));
            }

            deque.PopFirst(); swag.Pop();
            await swag.AllProd.Should().BeEqualTo(op.Identity);
            await Assert.That(swag.Pop).Throws<ContractAssertException>()
                .WithMessage("data is empty.");

            for (int i = 0; i < 200; i++)
            {
                var next = nextProvider(Rnd);
                deque.AddLast(next); swag.Push(next);
            }
            await RandomInitial(deque, nextProvider);
        }
        async Task RandomInitial(Deque<T> deque, Func<Random, T> nextProvider)
        {
            var op = new TOp();
            var swag = new Swag<T, TOp>(deque.ToArray());
            await swag.AllProd.Should().BeEqualTo(deque.Aggregate(op.Operate));

            for (int i = 0; i < 2000; i++)
            {
                if (deque.Count > 0 && Rnd.Next(2) != 0)
                {
                    deque.PopFirst(); swag.Pop();
                    await swag.AllProd.Should().BeEqualTo(deque.Aggregate(op.Operate));
                }
                else
                {
                    var next = nextProvider(Rnd);
                    deque.AddLast(next); swag.Push(next);
                    await swag.AllProd.Should().BeEqualTo(deque.Aggregate(op.Operate));
                }
            }
        }
    }
    readonly struct AffineTransformationOp : ISegtreeOperator<Mod998244353AffineTransformation>
    {
        public Mod998244353AffineTransformation Operate(Mod998244353AffineTransformation x, Mod998244353AffineTransformation y)
            => y.Apply(x);
        public Mod998244353AffineTransformation Identity => new(StaticModInt<Mod998244353>.One, default);
    }
    readonly struct SlideMinOp : ISegtreeOperator<int>
    {
        public int Identity => int.MaxValue;
        public int Operate(int x, int y) => Math.Min(x, y);
    }
    readonly struct SlideMaxOp : ISegtreeOperator<int>
    {
        public int Identity => int.MinValue;
        public int Operate(int x, int y) => Math.Max(x, y);
    }
}