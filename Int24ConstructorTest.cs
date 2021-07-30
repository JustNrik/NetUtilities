using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NetUtilities.Tests.System.Int24Tests
{
    public class Int24TestsConstructor
    {
        [Fact]
        public void UInt24LesserThanMinValue()
        {
            UInt24 U24 = new UInt24();
            U24 = (UInt24)24;
            Int24 int24 = new Int24(U24);

            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(-8388609));
        }

        [Fact]
        public void UInt24GreaterThanMaxValue()
        {
            UInt24 U24 = new UInt24();
            U24 = (UInt24)24;
            Int24 int24 = new Int24(U24);

            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(8388608));
        }

        [Fact]
        public void UInt32LesserThanMinValue()
        {
            UInt32 U32 = new UInt32();
            U32 = (UInt32)32;
            Int24 int24 = new Int24(U32);

            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(-8388609));
        }

        [Fact]
        public void UInt32GreaterThanMaxValue()
        {
            UInt32 U32 = new UInt32();
            U32 = (UInt32)32;
            Int24 int24 = new Int24(U32);

            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(8388608));
        }

        [Fact]
        public void Int32LesserThanMinValue()
        {
            Int32 I32 = new Int32();
            I32 = 32;
            Int24 int24 = new Int24(I32);

            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(-8388609));
        }

        [Fact]
        public void Int32GreaterThanMaxValue()
        {
            Int32 I32 = new Int32();
            I32 = 32;
            Int24 int24 = new Int24(I32);

            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(8388608));
        }

        [Fact]
        public void UInt64LesserThanMinValue()
        {
            UInt64 U64 = new UInt64();
            U64 = 64;
            Int24 int24 = new Int24(U64);

            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(-8388609));
        }

        [Fact]
        public void UInt64GreaterThanMaxValue()
        {
            UInt64 U64 = new UInt64();
            U64 = 64;
            Int24 int24 = new Int24(U64);

            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(8388608));
        }

        [Fact]
        public void ByteLesserThanMinValue()
        {
            byte byteValue = 64;
            Int24 int24 = new Int24(byteValue);

            int24 = (Int24)64;

            Assert.True(byteValue == int24);
            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(8388608));
        }

        [Fact]
        public void ByteGreaterThanMaxValue()
        {
            byte byteValue = 255;
            Int24 int24 = new Int24(byteValue);

            int24 = (Int24)255;

            Assert.True(byteValue == int24);
            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(8388608));
        }

        [Fact]
        public void IntPtrLesserThanMinValue()
        {
            IntPtr ptr = new IntPtr();
            ptr = (IntPtr)64;
            Int24 int24 = new Int24(ptr);

            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(-8388609));
        }

        [Fact]
        public void IntPtrGreaterThanMaxValue()
        {
            IntPtr ptr = new IntPtr();
            ptr = (IntPtr)64;
            Int24 int24 = new Int24(ptr);

            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(8388608));
        }

        [Fact]
        public void UIntPtrLesserThanMinValue()
        {
            UIntPtr ptr = new UIntPtr();
            ptr = (UIntPtr)255;
            Int24 int24 = new Int24(ptr);

            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(-8388609));
        }

        [Fact]
        public void UIntPtrGreaterThanMaxValue()
        {
            UIntPtr ptr = new UIntPtr();
            ptr = (UIntPtr)255;
            Int24 int24 = new Int24(ptr);

            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(8388608));
        }

        [Fact]
        public void SByteLesserThanMinValue()
        {
            sbyte byteValue = 64;
            Int24 int24 = new Int24(byteValue);

            int24 = (Int24)64;

            Assert.True(byteValue == int24);
            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(8388608));
        }

        [Fact]
        public void SByteGreaterThanMaxValue()
        {
            sbyte byteValue = 64;
            Int24 int24 = new Int24(byteValue);

            int24 = (Int24)64;

            Assert.True(byteValue == int24);
            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(8388608));
        }

        [Fact]
        public void ShortLesserThanMinValue()
        {
            short shortValue = 12;
            Int24 int24 = new Int24(shortValue);

            int24 = (Int24)12;

            Assert.True(shortValue == int24);
            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(8388608));
        }

        [Fact]
        public void ShortGreaterThanMaxValue()
        {
            short shortValue = 12;
            Int24 int24 = new Int24(shortValue);

            int24 = (Int24)12;

            Assert.True(shortValue == int24);
            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(8388608));
        }

        [Fact]
        public void UShortLesserThanMinValue()
        {
            ushort uShortValue = 10;
            Int24 int24 = new Int24(uShortValue);

            int24 = (Int24)10;

            Assert.True(uShortValue == int24);
            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(8388608));
        }

        [Fact]
        public void UShortGreaterThanMaxValue()
        {
            ushort uShortValue = 10;
            Int24 int24 = new Int24(uShortValue);

            int24 = (Int24)10;

            Assert.True(uShortValue == int24);
            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(8388608));
        }

        [Fact]
        public void LongLesserThanMinValue()
        {
            long longValue = 100;
            Int24 int24 = new Int24(longValue);

            int24 = (Int24)100;

            Assert.True(longValue == int24);
            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(8388608));
        }

        [Fact]
        public void LongGreaterThanMaxValue()
        {
            long longValue = 100;
            Int24 int24 = new Int24(longValue);

            int24 = (Int24)100;

            Assert.True(longValue == int24);
            Assert.Throws<ArgumentOutOfRangeException>(() => int24 = (Int24)(8388608));
        }

        [Fact]
        public void ByteArrayMustBeThree()
        {
            // Assign values inside Assert or Exception stated as rules

            Assert.Throws<ArgumentException>(() =>
            { 
                byte[] byteValues = { 1, 2, 3, 4 };
                Int24 int24 = new Int24(byteValues);
            });
        }

        [Fact]
        public void ByteArrayMustBeThreeWithIndex()
        {
            // Assign values inside Assert or Exception stated as rules

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                int startIndex = 4;
                byte[] byteValues = { 1, 2, 3, 4 };
                Int24 int24 = new Int24(byteValues, startIndex);
            });
        }
    }
}
