using System;
using Xunit;

namespace NetUtilities.Tests.System
{
    public class RandomizerTest
    {
        [Fact]
        public void NextLong()
        {
            var rand = new Randomizer();

            for (var x = 0; x < 1000; x++)
                Assert.True(rand.NextLong() >= 0 && rand.NextLong() < long.MaxValue);
        }

        [Fact]
        public void NextLongMax()
        {
            var rand = new Randomizer();
            var max = 100L;

            for (var x = 0; x < 1000; x++)
                Assert.True(rand.NextLong(max) >= 0 && rand.NextLong(max) < max);
        }

        [Fact]
        public void NextLongMinMax()
        {
            var rand = new Randomizer();
            var min = -100L;
            var max = 100L;

            for (var x = 0; x < 1000; x++)
                Assert.True(rand.NextLong(min, max) >= min && rand.NextLong(min, max) < max);
        }
    }
}
