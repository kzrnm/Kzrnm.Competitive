using AtCoder;
using AtCoder.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kzrnm.Competitive.Testing.DataStructure
{
    public class SwagDequeTests
    {
        [Fact]
        public void AddLastAndPopFirst()
        {
            var swag = new SwagDeque<int, SlideMinOp>();
            swag.AllProd.ShouldBe(int.MaxValue);
            swag.AddLast(1);
            swag.AllProd.ShouldBe(1);
            swag.PopFirst();
            swag.AllProd.ShouldBe(int.MaxValue);
        }
        [Fact]
        public void AddFirstAndPopLast()
        {
            var swag = new SwagDeque<int, SlideMinOp>();
            swag.AllProd.ShouldBe(int.MaxValue);
            swag.AddFirst(1);
            swag.AllProd.ShouldBe(1);
            swag.PopLast();
            swag.AllProd.ShouldBe(int.MaxValue);
        }

        Random rnd = new(227);
        Mod998244353AffineTransformation RandomAffine(Random rnd) => new(rnd.Next(), rnd.Next());
        int RandomInt32(Random rnd) => rnd.Next();

        public static IEnumerable<TheoryDataRow<PopAdd, PopAdd>> CreateTestData()
        {
            var array = new[] { PopAdd.First, PopAdd.Last, PopAdd.Random, };
            foreach (var pop in array)
                foreach (var add in array)
                    yield return (pop, add);
        }


        [Theory]
        [MemberData(nameof(CreateTestData))]
        public void AffineTransformation(PopAdd pop, PopAdd add)
        {
            var inner = new Inner<Mod998244353AffineTransformation, AffineTransformationOp>(rnd, pop, add);
            inner.RandomTest(RandomAffine);
            inner.NearZeroTest(RandomAffine);
        }

        [Theory]
        [MemberData(nameof(CreateTestData))]
        public void SlideMin(PopAdd pop, PopAdd add)
        {
            var inner = new Inner<int, SlideMinOp>(rnd, pop, add);
            inner.RandomTest(RandomInt32);
            inner.NearZeroTest(RandomInt32);
        }

        [Theory]
        [MemberData(nameof(CreateTestData))]
        public void SlideMax(PopAdd pop, PopAdd add)
        {
            var inner = new Inner<int, SlideMaxOp>(rnd, pop, add);
            inner.RandomTest(RandomInt32);
            inner.NearZeroTest(RandomInt32);
        }

        public enum PopAdd
        {
            First,
            Last,
            Random,
        }

        record Inner<T, TOp>(Random Rnd, PopAdd PopType, PopAdd AddType) where TOp : struct, ISegtreeOperator<T>
        {
            void Pop(Deque<T> deque, SwagDeque<T, TOp> swag)
            {
                switch (PopType)
                {
                    case PopAdd.First: PopFirst(deque, swag); break;
                    case PopAdd.Last: PopLast(deque, swag); break;
                    case PopAdd.Random: PopRandom(deque, swag); break;
                }
            }
            void Add(Deque<T> deque, SwagDeque<T, TOp> swag, T value)
            {
                switch (PopType)
                {
                    case PopAdd.First: AddFirst(deque, swag, value); break;
                    case PopAdd.Last: AddLast(deque, swag, value); break;
                    case PopAdd.Random: AddRandom(deque, swag, value); break;
                }
            }
            void PopFirst(Deque<T> deque, SwagDeque<T, TOp> swag)
            {
                swag.PopFirst();
                deque.PopFirst();
            }
            void PopLast(Deque<T> deque, SwagDeque<T, TOp> swag)
            {
                swag.PopLast();
                deque.PopLast();
            }
            void PopRandom(Deque<T> deque, SwagDeque<T, TOp> swag)
            {
                if (Rnd.Next(2) != 0)
                {
                    swag.PopFirst();
                    deque.PopFirst();
                }
                else
                {
                    swag.PopLast();
                    deque.PopLast();
                }
            }
            void AddFirst(Deque<T> deque, SwagDeque<T, TOp> swag, T value)
            {
                swag.AddFirst(value);
                deque.AddFirst(value);
            }
            void AddLast(Deque<T> deque, SwagDeque<T, TOp> swag, T value)
            {
                swag.AddLast(value);
                deque.AddLast(value);
            }
            void AddRandom(Deque<T> deque, SwagDeque<T, TOp> swag, T value)
            {
                if (Rnd.Next(2) != 0)
                {
                    swag.AddFirst(value);
                    deque.AddFirst(value);
                }
                else
                {
                    swag.AddLast(value);
                    deque.AddLast(value);
                }
            }

            public void NearZeroTest(Func<Random, T> nextProvider)
            {
                var op = new TOp();
                var swag = new SwagDeque<T, TOp>();
                var deque = new Deque<T>();

                swag.AllProd.ShouldBe(op.Identity);
                Should.Throw<ContractAssertException>(() => Pop(deque, swag))
                    .Message.ShouldBe("data is empty.");

                for (int q = 0; q < 2000; q++)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var next = nextProvider(Rnd);
                        Add(deque, swag, next);
                        swag.AllProd.ShouldBe(deque.Aggregate(op.Operate));
                    }

                    Pop(deque, swag);
                    swag.AllProd.ShouldBe(deque.Aggregate(op.Operate));
                    Pop(deque, swag);
                    swag.AllProd.ShouldBe(op.Identity);
                }
            }

            public void RandomTest(Func<Random, T> nextProvider)
            {
                var op = new TOp();
                var swag = new SwagDeque<T, TOp>();
                var deque = new Deque<T>();

                swag.AllProd.ShouldBe(op.Identity);

                for (int i = 0; i < 200; i++)
                {
                    var next = nextProvider(Rnd);
                    Add(deque, swag, next);
                    swag.AllProd.ShouldBe(deque.Aggregate(op.Operate));
                }

                for (int i = 1; i < 200; i++)
                {
                    Pop(deque, swag);
                    swag.AllProd.ShouldBe(deque.Aggregate(op.Operate));
                }

                Pop(deque, swag);
                swag.AllProd.ShouldBe(op.Identity);
                Should.Throw<ContractAssertException>(() => Pop(deque, swag))
                    .Message.ShouldBe("data is empty.");

                for (int i = 0; i < 200; i++)
                {
                    var next = nextProvider(Rnd);
                    Add(deque, swag, next);
                }
                RandomInitial(deque, nextProvider);
            }

            void RandomInitial(Deque<T> deque, Func<Random, T> nextProvider)
            {
                var op = new TOp();
                var swag = new SwagDeque<T, TOp>(deque.ToArray());
                swag.AllProd.ShouldBe(deque.Aggregate(op.Operate));

                for (int i = 0; i < 2000; i++)
                {
                    if (deque.Count > 0 && Rnd.Next(2) != 0)
                    {
                        Pop(deque, swag);
                        swag.AllProd.ShouldBe(deque.Aggregate(op.Operate));
                    }
                    else
                    {
                        var next = nextProvider(Rnd);
                        Add(deque, swag, next);
                        swag.AllProd.ShouldBe(deque.Aggregate(op.Operate));
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
}
