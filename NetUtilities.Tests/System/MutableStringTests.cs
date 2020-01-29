using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NetUtilities.Tests.System
{
    public class MutableStringTests
    {
        private static readonly Random random = new Random();

        [Fact]
        public void MutableString_TextApend_length()
        {
            MutableString mutableString = new MutableString("this is a test");
            Assert.Equal(14, mutableString.Length);
            Assert.Equal("this is a test", mutableString);

            mutableString.Append(" and i add other words");
            Assert.Equal("this is a test and i add other words", mutableString);
        }


        [Fact]
        public void MutableString_StartsWith_EndWith()
        {
            MutableString mutableString = new MutableString("this is a test!");
            Assert.True(mutableString.EndsWith('!'));
            Assert.False(mutableString.EndsWith('c'));

            Assert.True(mutableString.EndsWith("test!"));
            Assert.False(mutableString.EndsWith("!t!"));

            Assert.True(mutableString.StartsWith('t'));
            Assert.False(mutableString.StartsWith('i'));
            Assert.True(mutableString.StartsWith("this"));
            Assert.False(mutableString.StartsWith("no"));
        }



        [Fact]
        public void MutableString_Contains()
        {
            MutableString mutableString = new MutableString("this is a test!");

            Assert.True(mutableString.Contains("test"));
            Assert.False(mutableString.Contains("home"));
            Assert.False(mutableString.ContainsAny(new string[] { "home", "blabla" }));
            Assert.True(mutableString.ContainsAny(new string[] { "home", "this" }));

            Assert.False(mutableString.ContainsAll(new string[] { "home", "blabla" }));
            Assert.True(mutableString.ContainsAll(new string[] { "test", "this" }));

            Assert.True(mutableString.Contains('!', 14, 1));
            Assert.False(mutableString.Contains('!', 13, 1));

            Assert.True(mutableString.Contains('!', 11));

            Assert.True(mutableString.Contains("test", 10));
            Assert.False(mutableString.Contains("test", 11));

            Assert.True(mutableString.Contains("test", 10, 4));
            Assert.False(mutableString.Contains("test", 10, 3));
        }


        [Fact]
        public void MutableString_Split()
        {
            MutableString mutableString = new MutableString("this is| at| test| for split!");
            Assert.True( mutableString.Split('|').Count() == 4);
            Assert.True(mutableString.Split("t|").Count() == 3);
        }


    }
}
