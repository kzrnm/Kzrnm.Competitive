using System.Linq;

namespace Kzrnm.Competitive.Testing.Extensions
{
    public class CollectionExtensionTests
    {
        [Fact]
        public void CompressCount()
        {
            Enumerable.Range(0, 6).CompressCount()
                .Should()
                .Equal(
                    (0, 1),
                    (1, 1),
                    (2, 1),
                    (3, 1),
                    (4, 1),
                    (5, 1)
                );
            new int[] { 1, 1, 2, 2, 3, 3, }.CompressCount()
                .Should()
                .Equal(
                    (1, 2),
                    (2, 2),
                    (3, 2)
                );
            new int[] { 1, 1, 2, 2, 3, }.CompressCount()
                .Should()
                .Equal(
                    (1, 2),
                    (2, 2),
                    (3, 1)
                );
            new int[] { 1, 2, 2, 3, 3, }.CompressCount()
                .Should()
                .Equal(
                    (1, 1),
                    (2, 2),
                    (3, 2)
                );
        }
    }
}