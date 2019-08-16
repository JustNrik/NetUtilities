using NetUtilities.Tests.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NetUtilities.Tests.System
{
    public class EventManagerTests
    {
        [Fact]
        public void ShouldWork()
        {
            var source = new EventSource();
            var target = new EventListener();
            var manager = new EventManager<EventSource>(source);
            manager.AddHandlers(target);
            Assert.True(source.Raise());
        }
    }
}
