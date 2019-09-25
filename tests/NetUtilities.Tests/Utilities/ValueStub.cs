namespace NetUtilities.Tests.Utilities
{
    public class ValueStub<T>
    {
        public T Value { get; set; }
        public ValueStub() { }
        public ValueStub(T value)
        {
            Value = value;
        }
    }
}
