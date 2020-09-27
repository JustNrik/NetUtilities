using System;
using System.Collections.Generic;
using System.Text;

namespace NetUtilities.Tests.System
{
    class Int24Tests
    {
        [Fact]
        public void Int24_Creation_Bounds()
        {
            const int TestValue = 50000;
            const int MaxTestValue = 2247483647;
            const int MinTestValue = -2247483647;
			
			var validInt24 = new Int24(TestValue)
			Assert.Equal(validInt24, TestValue)
			
            Assert.Throws<InvalidOperationException>(() => new Int24(MaxTestValue));			
            Assert.Throws<InvalidOperationException>(() => new Int24(MinTestValue);			
			
			Assert.Throws<NotSupportedException>(() => validInt24.ToType(MutableString))
        }

        [Fact]
        public void Int24_Parseing()
        {
            Int24 ChangingValue;
			const string TestValue = "50000"
            const int MaxTestValue = "2247483647";
            const int MinTestValue = "-2247483647";
			
			Assert.Equal(Int24.Parse(ChangingValue).toString(),TestValue)
			Assert.Equal(Int24.Parse(ChangingValue).toString(),TestValue)
			
            Assert.Throws<NullArgumentException>(() => Int24.parse(null));
            Assert.Throws<OverflowException>(() => Int24.parse(MaxTestValue));
            Assert.Throws<InvalidFormatException>(() => Int24.parse("blah"));
			
			Int24.TryParse("a",ChangingValue)
			Assert.Equal(ChangingValue,null)
			
			Int24.TryParse("2247483647",ChangingValue)
			Assert.Equal(ChangingValue,null)
			
			Int24.TryParse("-2247483647",ChangingValue)
			Assert.Equal(ChangingValue,null)
		}
		
        [Fact]
        public void Int24_Operators()
        {
			Int24 TestValue = 50
			Assert.Equal(TestValue & new Int24(10), new Int24(2))
			Assert.Equal(TestValue | new Int24(10), new Int24(58))
			Assert.Equal(TestValue ^ new Int24(10), new Int24(56))
			Assert.Equal(TestValue ~ new Int24(10), new Int24(-51))
			
			Assert.Equal(TestValue + new Int24(10), new Int24(60))
			Assert.Equal(TestValue - new Int24(10), new Int24(40))
			Assert.Equal(TestValue / new Int24(10), new Int24(5))
			Assert.Equal(TestValue * new Int24(10), new Int24(500))
		}
    }
}
