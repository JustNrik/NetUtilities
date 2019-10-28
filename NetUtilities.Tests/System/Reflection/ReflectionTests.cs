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
        public void MapperTest_Properties_SingleProperty()
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
        public void MapperTest_Properties_MultipleProperties()
        {
            var fake = new MapperPropertiesFake();
            var mapper = new Mapper(fake);

            Assert.True(mapper.Properties.Count == 3);

            var propertyOne = mapper.Properties[0];
            var propertyTwo = mapper.Properties[1];
            var propertyThree = mapper.Properties[2];

            propertyOne.SetValue(fake, 42);
            propertyTwo.SetValue(fake, "John");
            propertyThree.SetValue(fake, DateTime.MinValue);

            Assert.Equal(42, fake.Id);
            Assert.Equal(42, propertyOne.GetValue(fake));

            Assert.Equal("John", fake.Name);
            Assert.Equal("John", propertyTwo.GetValue(fake));

            Assert.Equal(fake.TheDay, DateTime.MinValue);
            Assert.Equal(DateTime.MinValue, propertyThree.GetValue(fake));
        }

        [Fact]
        public void MapperTest_Method_MethodCount()
        {
            var fake = new MapperMethodsFake();
            var mapper = new Mapper(fake);

            Assert.True(mapper.Properties.Count == 1);
            Assert.True(mapper.Methods.Count == 1);
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

    public class MapperPropertiesFake
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime TheDay { get; set; }
    }

    public class MapperMethodsFake
    {
        public int SumTest(int x, int y)
        {
            return x + y;
        }
    }

   

    public class Bar
    {
    }
}
