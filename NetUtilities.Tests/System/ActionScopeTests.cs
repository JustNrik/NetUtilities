using System;
using NetUtilities.Tests.Utilities;
using Xunit;

namespace NetUtilities.Tests.System
{
    public class ActionScopeTests
    {
        // Make sure primitive values work alright.
        [Fact]
        public void CreateTest_Int_ShouldSucceed()
        {
            var stub = new ValueStub<int>(5);
            using (ActionScope<ValueStub<int>>.Create(stub, x => x.Value = 15, x => x.Value = 0))
                Assert.Equal(15, stub.Value);

            Assert.Equal(0, stub.Value);
        }

        [Fact]
        public void CreateTest_String_ShouldSucceed()
        {
            var stub = new ValueStub<string>("Test1");
            using (ActionScope<ValueStub<string>>.Create(stub, x => x.Value = "Test2", x => x.Value = "Test3"))
                Assert.Equal("Test2", stub.Value);

            Assert.Equal("Test3", stub.Value);
        }

        // Value types should be respected.
        [Fact]
        public void CreateTest_ValueShuffle_ShouldSucceed()
        {
            var stub = new ValueStub<string>("Test1");
            var stub2 = new ValueStub<string>("Test2");
            using (ActionScope<ValueStub<string>>.Create(stub, x => x.Value = stub2.Value, x => x.Value = "Test3"))
            {
                stub2.Value = "New Value";
                Assert.Equal("Test2", stub.Value);
            }

            Assert.Equal("Test3", stub.Value);
        }

        //Nested references should move around.
        [Fact]
        public void CreateTest_ReferenceShuffle_ShouldSucceed()
        {
            var stub1 = new ValueStub<ValueStub<string>>(new ValueStub<string>("Test1"));
            var stub2 = new ValueStub<ValueStub<string>>(new ValueStub<string>("Test2"));
            var testStub1 = new ValueStub<string>("TestStub1");
            var testStub2 = new ValueStub<string>("TestStub2");
            using (ActionScope<ValueStub<ValueStub<string>>>.Create(stub1, x => x.Value = stub2.Value, x => x.Value = testStub1))
            {
                stub2.Value = testStub2;
                Assert.Equal("Test2", stub1.Value.Value);
            }

            Assert.Equal("TestStub1", stub1.Value.Value);
        }
    }
}
