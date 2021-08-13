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
