namespace System.Reflection
{
    public class FieldData : MemberData<FieldInfo>
    {
        public FieldData(FieldInfo field) : base(field)
        {
        }
    }
}
