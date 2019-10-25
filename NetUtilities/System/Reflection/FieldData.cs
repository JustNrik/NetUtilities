namespace System.Reflection
{
    public class FieldData : MemberData
    {
        public new FieldInfo Member => (FieldInfo)base.Member;

        public FieldData(FieldInfo field) : base(field)
        {
        }
    }
}
