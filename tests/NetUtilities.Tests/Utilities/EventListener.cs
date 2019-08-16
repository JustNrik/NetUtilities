using System;
using System.Collections.Generic;
using System.Text;

namespace NetUtilities.Tests.Utilities
{
    public sealed class EventListener
    {
        [Handles(typeof(EventSource), nameof(EventSource.Test))]
        public void OnTest()
        {

        }
    }
}
