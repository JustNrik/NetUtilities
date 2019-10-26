using System;
using System.Collections;
using System.Collections.Generic;
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

        [Fact]
        public void MapperTest1()
        {
            var plop = new Plop();
            var mapper = new Mapper(plop);

            Assert.True(mapper.Properties.Count == 1);

            var property = mapper.Properties[0];
            property.SetValue(plop, 1337);

            Assert.Equal(1337, plop.Value);
            Assert.Equal(1337, property.GetValue(plop));
        }

        [Fact]
        public void InheritsAndImplementsTest()
        {
            Assert.True(typeof(int).Inherits(typeof(object)));
            Assert.True(typeof(int).Inherits(typeof(ValueType)));
            Assert.True(typeof(int).Implements(typeof(IEquatable<int>)));
            Assert.True(typeof(IEnumerable<int>).Inherits(typeof(IEnumerable)));
            Assert.False(typeof(IEnumerable<int>).Implements(typeof(IEnumerable)));
            Assert.False(typeof(Plop).Inherits(typeof(Bar)));
        }

        [Fact]
        public void MapperCreateInstanceTests()
        {
            var mapper = new Mapper(typeof(Foo));
            var foo = mapper.Constructors[0].CreateInstance("Bob") as Foo;

            Assert.NotNull(foo);
            Assert.Equal("Bob", foo.Name);
        }
    }

    public class Plop
    {
        public int Value { get; set; }
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
