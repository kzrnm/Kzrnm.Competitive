
namespace Kzrnm.Competitive.Testing.DataStructure
{
    public class PersistentBinaryTrieTests
    {
        [Fact]
        public void Simple()
        {
            var bt = new PersistentBinaryTrie();
            bt = bt.Add(1, 2);
            bt = bt.Add(2);
            bt = bt.Add(3);
            bt = bt.Remove(3);
            bt = bt.Add(5);
            bt.Add(3).Add(6).Remove(2);

            bt.KthElement(0).Num.ShouldBe(1u);
            bt.KthElement(1).Num.ShouldBe(1u);
            bt.KthElement(2).Num.ShouldBe(2u);
            bt.KthElement(3).Num.ShouldBe(5u);

            bt.MinElement().Num.ShouldBe(1u);
            bt.MaxElement().Num.ShouldBe(5u);

            bt.Count(1).ShouldBe(2);
            bt.Count(2).ShouldBe(1);
            bt.Count(5).ShouldBe(1);

            bt.CountLess(1).ShouldBe(0);
            bt.CountLess(2).ShouldBe(2);
            bt.CountLess(3).ShouldBe(3);
            bt.CountLess(5).ShouldBe(3);
            bt.CountLess(6).ShouldBe(4);



            bt.KthElement(0, 4).Num.ShouldBe(1u);
            bt.KthElement(1, 4).Num.ShouldBe(5u);
            bt.KthElement(2, 4).Num.ShouldBe(5u);
            bt.KthElement(3, 4).Num.ShouldBe(6u);

            bt.MinElement(4).Num.ShouldBe(1u);
            bt.MaxElement(4).Num.ShouldBe(6u);

            bt.Count(1, 4).ShouldBe(1);
            bt.Count(2, 4).ShouldBe(0);
            bt.Count(5, 4).ShouldBe(2);
            bt.Count(6, 4).ShouldBe(1);

            bt.CountLess(1, 4).ShouldBe(0);
            bt.CountLess(2, 4).ShouldBe(1);
            bt.CountLess(3, 4).ShouldBe(1);
            bt.CountLess(5, 4).ShouldBe(1);
            bt.CountLess(6, 4).ShouldBe(3);
            bt.CountLess(7, 4).ShouldBe(4);

            bt = bt.Remove(2);
            bt.KthElement(0).Num.ShouldBe(1u);
            bt.KthElement(1).Num.ShouldBe(1u);
            bt.KthElement(2).Num.ShouldBe(5u);

            bt.MinElement().Num.ShouldBe(1u);
            bt.MaxElement().Num.ShouldBe(5u);

            bt.Count(1).ShouldBe(2);
            bt.Count(5).ShouldBe(1);

            bt.CountLess(1).ShouldBe(0);
            bt.CountLess(2).ShouldBe(2);
            bt.CountLess(5).ShouldBe(2);
            bt.CountLess(6).ShouldBe(3);
        }
    }
}
