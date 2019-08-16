using System;
using System.Collections.Generic;
using System.Text;

namespace NetUtilities.Tests.Utilities
{
    public sealed class EventSource
    {
        public event Action Test;

        public bool Raise()
        {
            if (Test is null)
                return false;

            Test.Invoke();
            return true;
        }
    }
}
