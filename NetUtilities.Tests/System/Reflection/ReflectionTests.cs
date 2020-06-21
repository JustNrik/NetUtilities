using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using static NetUtilities.Tests.System.Reflection.MapperEventFake;

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

            Assert.True(mapper.Properties.Count == 0);
            Assert.True(mapper.Methods.Count == 7);
            int count = 0;
            foreach (var method in mapper.Methods)
            {
                if (method.Member.DeclaringType == typeof(MapperMethodsFake))
                {
                    count++;
                }
            }
            Assert.True(count == 1);
        }

        [Fact]
        public void MapperTest_Method_MethodDeclaringTypeOnly()
        {
            var fake = new MapperMethodsInheritanceFake();
            var mapper = new Mapper(fake);

            Assert.True(mapper.MethodsDeclaringTypeOnly.Count == 1);
            Assert.Equal("DoNothing", mapper.MethodsDeclaringTypeOnly[0].Member.Name);
        }

        [Fact]
        public void MapperTest_Method_MethodExcludingObjectMembers()
        {
            var fake = new MapperMethodsInheritanceFake();
            var mapper = new Mapper(fake);

            Assert.True(mapper.MethodsExcludingObjectBaseMembers.Count == 2);
            Assert.Equal("DoNothing", mapper.MethodsExcludingObjectBaseMembers[0].Member.Name);
            Assert.Equal("SumTest", mapper.MethodsExcludingObjectBaseMembers[1].Member.Name);
        }

        [Fact]
        public void MapperTest_Method_MethodSearch()
        {
            var fake = new MapperMethodsFake();
            var mapper = new Mapper(fake);

            Assert.True(mapper.Properties.Count == 0);
            var method = mapper.Methods.Find(x => x.Member.Name == "SumTest");
            Assert.Equal("SumTest", method.Member.Name);
        }

        [Fact]
        public void MapperTest_Method_MethodParamaters_Return()
        {
            var fake = new MapperMethodsFake();
            var mapper = new Mapper(fake);

            var method = mapper.Methods.Find(x => x.Member.Name == "SumTest");
            Assert.True(method.Parameters.Count == 2);
            Assert.True(method.Member.ReturnType == typeof(int));
            Assert.True(method.Parameters[0].ParameterType == typeof(int));
            Assert.True(method.Parameters[0].Name == "x");
            Assert.True(method.Parameters[1].ParameterType == typeof(int));
            Assert.True(method.Parameters[1].Name == "y");
        }

        [Fact]
        public void MapperTest_Field_FieldCount()
        {
            var fake = new MapperFieldsFake();
            var mapper = new Mapper(fake);

            Assert.True(mapper.Fields.Count == 2);
        }

        [Fact]
        public void MapperTest_Field_FieldInfo()
        {
            var fake = new MapperFieldsFake();
            var mapper = new Mapper(fake);

            Assert.True(mapper.Fields[0].Member.IsPublic);
            Assert.False(mapper.Fields[1].Member.IsPublic);
            Assert.True(mapper.Fields[1].Member.IsStatic);

            Assert.Equal("_intField", mapper.Fields[0].Member.Name);
            Assert.Equal("_stringField", mapper.Fields[1].Member.Name);
        }

        [Fact]
        public void MapperTest_Field_FieldValue()
        {
            var fake = new MapperFieldsFake();
            var mapper = new Mapper(fake);

            Assert.Equal(3, mapper.Fields[0].Member.GetValue(fake));
            Assert.Equal("test", mapper.Fields[1].Member.GetValue(fake));
        }

        [Fact]
        public void MapperTest_Event_EventCount()
        {
            var fake = new MapperEventFake();
            var mapper = new Mapper(fake);

            Assert.True(mapper.Events.Count == 1);
        }

        [Fact]
        public void MapperTest_Event_EventDetails()
        {
            var fake = new MapperEventFake();
            var mapper = new Mapper(fake);

            Assert.Equal("OnDoingThing", mapper.Events[0].Member.Name);
            Assert.Equal(typeof(EventHandlerTest), mapper.Events[0].Member.EventHandlerType);
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

    #region TestFakeClasses   
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

    public class MapperMethodsInheritanceFake : MapperMethodsFake
    {
        public void DoNothing()
        {
            //Does nothing
        }
    }

    public class MapperFieldsFake
    {
        public int _intField = 3;
        private static string _stringField = "test";
    }

    public class MapperEventFake
    {
        public delegate void EventHandlerTest();
        public event EventHandlerTest OnDoingThing;
    }

    public class Bar
    {
    }
    #endregion
}
