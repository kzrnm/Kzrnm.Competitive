using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Kzrnm.Competitive.Testing.Extensions
{
    public class CollectionTupleExtensionTests
    {
        [Fact]
        public void StackTryPop2()
        {
            var collection = new Stack<(string, int)>();
            collection.TryPop(out _, out _).Should().BeFalse();

            collection.Push(("foo", 227));
            collection.TryPop(out var v1, out var v2).Should().BeTrue();
            v1.Should().Be("foo");
            v2.Should().Be(227);
        }
        [Fact]
        public void StackTryPeek2()
        {
            var collection = new Stack<(string, int)>();
            collection.TryPeek(out _, out _).Should().BeFalse();

            collection.Push(("foo", 227));
            collection.TryPeek(out var v1, out var v2).Should().BeTrue();
            v1.Should().Be("foo");
            v2.Should().Be(227);
        }
        [Fact]
        public void StackTryPop3()
        {
            var collection = new Stack<(string, int, DateTime)>();
            collection.TryPop(out _, out _, out _).Should().BeFalse();

            collection.Push(("foo", 227, new DateTime(2022, 2, 22)));
            collection.TryPop(out var v1, out var v2, out var v3).Should().BeTrue();
            v1.Should().Be("foo");
            v2.Should().Be(227);
            v3.Should().Be(new DateTime(2022, 2, 22));
        }
        [Fact]
        public void StackTryPeek3()
        {
            var collection = new Stack<(string, int, DateTime)>();
            collection.TryPeek(out _, out _, out _).Should().BeFalse();

            collection.Push(("foo", 227, new DateTime(2022, 2, 22)));
            collection.TryPeek(out var v1, out var v2, out var v3).Should().BeTrue();
            v1.Should().Be("foo");
            v2.Should().Be(227);
            v3.Should().Be(new DateTime(2022, 2, 22));
        }

        [Fact]
        public void QueueTryDequeue2()
        {
            var collection = new Queue<(string, int)>();
            collection.TryDequeue(out _, out _).Should().BeFalse();

            collection.Enqueue(("foo", 227));
            collection.TryDequeue(out var v1, out var v2).Should().BeTrue();
            v1.Should().Be("foo");
            v2.Should().Be(227);
        }
        [Fact]
        public void QueueTryPeek2()
        {
            var collection = new Queue<(string, int)>();
            collection.TryPeek(out _, out _).Should().BeFalse();

            collection.Enqueue(("foo", 227));
            collection.TryPeek(out var v1, out var v2).Should().BeTrue();
            v1.Should().Be("foo");
            v2.Should().Be(227);
        }
        [Fact]
        public void QueueTryDequeue3()
        {
            var collection = new Queue<(string, int, DateTime)>();
            collection.TryDequeue(out _, out _, out _).Should().BeFalse();

            collection.Enqueue(("foo", 227, new DateTime(2022, 2, 22)));
            collection.TryDequeue(out var v1, out var v2, out var v3).Should().BeTrue();
            v1.Should().Be("foo");
            v2.Should().Be(227);
            v3.Should().Be(new DateTime(2022, 2, 22));
        }
        [Fact]
        public void QueueTryPeek3()
        {
            var collection = new Queue<(string, int, DateTime)>();
            collection.TryPeek(out _, out _, out _).Should().BeFalse();

            collection.Enqueue(("foo", 227, new DateTime(2022, 2, 22)));
            collection.TryPeek(out var v1, out var v2, out var v3).Should().BeTrue();
            v1.Should().Be("foo");
            v2.Should().Be(227);
            v3.Should().Be(new DateTime(2022, 2, 22));
        }
    }
}
