namespace System
{
    public class Randomizer : Random
    {
        public virtual long NextLong()
            => (long)(Sample() * long.MaxValue);

        public virtual long NextLong(long max)
            => (long)(Sample() * max);

        public virtual long NextLong(long min, long max)
            => (long)(Sample() * (max - min)) + min;
    }
}
