using System;
using System.Reflection;
using Xunit;

namespace NetUtilities.Tests.System.Reflection
{
    public class ReflectionTests
    {
        [Fact]
        public void Creating_Instances()
        {
            var x = Factory<int>.CreateInstance();
            var singleton = Factory<Bar>.Singleton;
            var singleton2 = Factory<Bar>.Singleton;
            var bar = Factory<Bar>.CreateInstance();

            Assert.Equal(0, x);
            Assert.True(ReferenceEquals(singleton, singleton2));
            Assert.NotNull(bar);
        }
    }

    public class Foo
    {
        public string Name { get; set; }

        public Foo(string name) => Name = name;
    }

    public class Bar
    {
    }
}
