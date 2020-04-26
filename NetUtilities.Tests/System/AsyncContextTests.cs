using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NetUtilities.Tests.System
{
    public class AsyncContextTests
    {
        [Fact]
        public void SyncContextTest()
        {
            var current = SynchronizationContext.Current;

            SynchronizationContextHelper.Run(() => Assert.True(SynchronizationContext.Current is DefaultSynchronizationContext));
            Assert.True(current == SynchronizationContext.Current);

            var defaultContext = new DefaultSynchronizationContext();

            SynchronizationContext.SetSynchronizationContext(defaultContext);
            SynchronizationContextHelper.Run(() => Assert.True(SynchronizationContext.Current != defaultContext));
            Assert.True(SynchronizationContext.Current == defaultContext);
            SynchronizationContext.SetSynchronizationContext(current);
        }

        [Fact]
        public async Task SyncContextAsyncTest()
        {
            var current = SynchronizationContext.Current;
            var result = await SynchronizationContextHelper.RunAsync(str =>
            {
                Assert.Equal(SynchronizationContext.Current, DefaultSynchronizationContext.Shared);
                return Task.FromResult(str.ToUpper());
            }, "Hello");

            Assert.Equal("HELLO", result);
            Assert.Equal(current, SynchronizationContext.Current);
        }
    }
}
