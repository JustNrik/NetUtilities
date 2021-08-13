namespace System.Diagnostics
{
    public readonly struct ValueStopwatch
    {
        private static readonly decimal TimestampToTicks = TimeSpan.TicksPerSecond / (decimal)Stopwatch.Frequency;
        private readonly long _startTimestamp;

        public bool IsActive 
            => _startTimestamp != 0;

        /* Some day...
        public ValueStopwatch()
        {
            _startTimestamp = Stopwatch.GetTimestamp();
        }
        */

        public ValueStopwatch(long startTimestamp)
        {
            _startTimestamp = startTimestamp;
        }

        public static ValueStopwatch StartNew()
            => new(Stopwatch.GetTimestamp());

        public TimeSpan Elapsed
        {
            get
            {
                if (!IsActive)
                    return TimeSpan.Zero;

                var end = Stopwatch.GetTimestamp();
                var delta = end - _startTimestamp;
                var ticks = (long)(TimestampToTicks * delta);
                return new(ticks);
            }
        }
    }
}
