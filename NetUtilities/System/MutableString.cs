using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public class MutableString : 
        IEnumerable, IEnumerable<char>, 
        IComparable, IComparable<string>, IComparable<MutableString>, 
        IEquatable<string>, IEquatable<MutableString>,
        IEqualityComparer ,IEqualityComparer<string>, IEqualityComparer<MutableString>
    {
        private readonly StringBuilder _builder;

        public MutableString()
            => _builder = new StringBuilder();

        public MutableString(string value)
            => _builder = new StringBuilder(value);

        public MutableString(int capacity, int maxCapacity)
            => _builder = new StringBuilder(capacity, maxCapacity);

        public override string ToString()
            => _builder.ToString();

        public static MutableString operator +(MutableString MutableString, string value)
        {
            MutableString._builder.Append(value);
            return MutableString;
        }

        public static string operator +(string value, MutableString MutableString)
            => value + MutableString.ToString();

        public static MutableString operator +(MutableString MutableString, char value)
        {
            MutableString._builder.Append(value);
            return MutableString;
        }

        public static string operator +(char value, MutableString mutableString)
            => value + mutableString.ToString();

        public static implicit operator string(MutableString mutableString)
            => mutableString.ToString();

        public static implicit operator MutableString(string value)
            => new MutableString(value);

        //public static implicit operator ReadOnlySpan<char>(MutableString mutable)
        //    => new ReadOnlySpan<char>(mutable.ToString());

        IEnumerator<char> IEnumerable<char>.GetEnumerator()
            => ToString().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ToString().GetEnumerator();

        int IComparable.CompareTo(object obj)
            => obj switch
        {
            string str => ((IComparable<string>)this).CompareTo(str),
            MutableString mutable => ((IComparable<string>)this).CompareTo(mutable.ToString()),
            _ => 0
        };

        int IComparable<string>.CompareTo(string other)
            => _builder.ToString().CompareTo(other);


        int IComparable<MutableString>.CompareTo(MutableString other)
            => ((IComparable<string>)this).CompareTo(other.ToString());

        public bool Equals(string other)
            => other.Equals(this);

        public bool Equals(MutableString other)
            => other.ToString().Equals(ToString());

        public char this[int index]
        {
            get => _builder[index];
            set => _builder[index] = value;
        }

        public override bool Equals(object obj)
            => obj switch
        {
            string str => Equals(str),
            MutableString mutable => Equals(mutable),
            _ => false
        };

        public override int GetHashCode()
            => _builder.GetHashCode();

        bool IEqualityComparer<string>.Equals(string x, string y)
            => x.Equals(y);

        int IEqualityComparer<string>.GetHashCode(string obj)
            => obj.GetHashCode();

        bool IEqualityComparer<MutableString>.Equals(MutableString x, MutableString y)
            => x.Equals(y);

        int IEqualityComparer<MutableString>.GetHashCode(MutableString obj)
            => obj.GetHashCode();

        bool IEqualityComparer.Equals(object x, object y)
            => x.Equals(y);

        int IEqualityComparer.GetHashCode(object obj)
            => obj.GetHashCode();

        public static bool operator ==(MutableString left, MutableString right)
            => left.Equals(right);

        public static bool operator !=(MutableString left, MutableString right)
            => !left.Equals(right);
    }
}
