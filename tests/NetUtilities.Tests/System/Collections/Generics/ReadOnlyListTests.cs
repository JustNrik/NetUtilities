using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace NetUtilities.Tests.System.Collections.Generics
{
    public class ReadOnlyListTests
    {
        [Fact]
        public void List_Doesnt_Mutate_If_Reference_Does()
        {
            var list = new List<int>() { 1, 2, 3, 4, 5 };
            var readonlyList = new ReadOnlyList<int>(list);
            list.Add(6);
            Assert.Equal(6, list.Count);
            Assert.Equal(5, readonlyList.Count);
        }

        [Fact]
        public void Indexing_Works()
        {
            var list = new int[] { 1, 2, 3, 4, 5, 6, 7 }.ToReadOnlyList();
            var test = list[0..3];
            Assert.Equal(3, test.Count);
            Assert.Equal(1, test[0]);
            Assert.Equal(2, test[1]);
            Assert.Equal(3, test[2]);
            Assert.Throws<ArgumentOutOfRangeException>(() => test[3]);
            var value = list[^1];
            Assert.Equal(7, value);
        }
    }
}
