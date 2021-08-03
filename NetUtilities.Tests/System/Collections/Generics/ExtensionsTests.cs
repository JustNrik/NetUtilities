using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NetUtilities.Tests.System.Collections.Generics
{
    public class ExtensionsTests
    {
        [Fact(Skip = "This test is broken in MacOS. Investigate.")]
        public void ContainsSequenceTest()
        {
            var source = Enumerable.Range(0, 100).ToArray();

            Assert.True(source.ContainsSequence(0, 1, 2, 3));
            Assert.True(source.ContainsSequence(5, 6, 7));
            Assert.True(source.ContainsSequence(50));
            Assert.True(source.ContainsSequence(97, 98, 99));
            Assert.True(source.ContainsSequence(source));

            Assert.False(source.ContainsSequence(0, 1, 2, 4));
            Assert.False(source.ContainsSequence(0, 1, 2, 4, 5, 6));
            Assert.False(source.ContainsSequence(200));
            Assert.False(source.ContainsSequence(4, 3, 2, 1));
        }
    }
}
