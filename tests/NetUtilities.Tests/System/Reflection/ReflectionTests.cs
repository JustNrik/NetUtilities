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
            var bar = Factory<Bar>.CreateInstance();
            var newX = Factory.CreateInstance(typeof(Bar));

            Assert.Equal(0, x);
            Assert.NotNull(bar);
            Assert.NotNull(newX);
        }

        [Fact]
        public void Singleton_Should_Point_To_The_Same_Object()
        {
            var singleton = Factory<Bar>.Singleton;
            var singleton2 = Factory<Bar>.Singleton;
            Assert.Same(singleton, singleton2);
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
