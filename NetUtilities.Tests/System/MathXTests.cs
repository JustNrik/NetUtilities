using Xunit;

namespace NetUtilities.Tests.System
{
    public class MathXTests
    {
        private static readonly Random random = new Random();

        [Fact]
        public void Sum()
        {
            var start = random.Next(1, 100);
            var end = random.Next(start + 1, 200);
            var uStart = (uint)start;
            var uEnd = (uint)end;

            Assert.Equal(Enumerable.Range(1, end).Sum(), (int)MathX.Sum(uEnd));
            Assert.Equal(Enumerable.Range(start, end - start + 1).Sum(), (int)MathX.Sum(uStart, uEnd));
            Assert.Throws<InvalidOperationException>(() => MathX.Sum(uEnd, uStart));
            Assert.Equal(1u, MathX.Sum(1, 1));
            Assert.Equal(15u, MathX.Sum(0, 5));
        }

        [Fact]
        public void SquareSum()
        {
            var start = random.Next(1, 100);
            var end = random.Next(start + 1, 200);
            var uStart = (uint)start;
            var uEnd = (uint)end;

            Assert.Equal(Enumerable.Range(1, end).Sum(x => x * x), (int)MathX.SquareSum(uEnd));
            Assert.Equal(Enumerable.Range(start, end - start + 1).Sum(x => x * x), (int)MathX.SquareSum(uStart, uEnd));
            Assert.Throws<InvalidOperationException>(() => MathX.SquareSum(uEnd, uStart));
            Assert.Equal(4u, MathX.SquareSum(2, 2));
            Assert.Equal(55u, MathX.SquareSum(0, 5));
        }

        [Fact]
        public void CubicSum()
        {
            var start = random.Next(1, 100);
            var end = random.Next(start + 1, 200);
            var uStart = (uint)start;
            var uEnd = (uint)end;

            Assert.Equal(Enumerable.Range(1, end).Sum(x => x * x * x), (int)MathX.CubicSum(uEnd));
            Assert.Equal(Enumerable.Range(start, end - start + 1).Sum(x => x * x * x), (int)MathX.CubicSum(uStart, uEnd));
            Assert.Throws<InvalidOperationException>(() => MathX.CubicSum(uEnd, uStart));
            Assert.Equal(8u, MathX.CubicSum(2, 2));
            Assert.Equal(225u, MathX.CubicSum(0, 5));
        }
    }
}
