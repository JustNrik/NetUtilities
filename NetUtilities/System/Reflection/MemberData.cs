namespace System.Reflection
{
    /// <summary>
    ///     Contains rich data and provides helper methods for a <typeparamref name="TMember"/>.
    /// </summary>
    /// <typeparam name="TMember">
    ///     The <see cref="MemberInfo"/> type.
    /// </typeparam>
    public abstract class MemberData<TMember>
        where TMember : MemberInfo
    {
        private readonly ConcurrentLazy<ReadOnlyList<Attribute>, TMember> _customAttributes;
        private readonly ConcurrentLazy<ReadOnlyList<CustomAttributeData>, TMember> _customAttributeDatas;

        /// <summary>
        ///     Gets the <see cref="MemberInfo"/> for this class.
        /// </summary>
        public TMember Member { get; init; }

        /// <summary>
        ///     Gets the custom attributes of this member.
        /// </summary>
        public ReadOnlyList<Attribute> CustomAttributes
            => _customAttributes.Value;

        /// <summary>
        ///     Gets the custom attribute datas of this member.
        /// </summary>
        public ReadOnlyList<CustomAttributeData> CustomAttributeDatas
            => _customAttributeDatas.Value;

        /// <summary>
        ///     Initializes a new instance of <see cref="MemberData{TMember}"/> <see langword="class"/> 
        ///     with the provided <typeparamref name="TMember"/> value.
        /// </summary>
        /// <param name="member">
        ///     The member.
        /// </param>
        protected MemberData(TMember member)
        {
            _customAttributes = new(static member => member.GetCustomAttributes().ToReadOnlyList(), member);
            _customAttributeDatas = new(static member => member.CustomAttributes.ToReadOnlyList(), member);

            Member = member;
        }
    }
}
