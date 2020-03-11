using System.Diagnostics;
using BeeSharp.Validation;

namespace BeeSharp.Types
{
    /// <summary>
    /// Type representing a trimmed string with at least one non whitespace character
    /// </summary>
    [DebuggerDisplay("{value, nq}")]
    public struct Name : IConstrainedType<Name>
    {
        private readonly string value;

        public Name(string s) => this.value = s;

        public static Name New(string s) => new Name(Check(s));

        public static Name Of(string s) => new Name(Check(s.Trim()));

        public static Name UncheckedNew(string s) => new Name(s);

        private static string Check(string s)
            => s.CheckTrimmed(nameof(s)).CheckMinLengthOf(1, nameof(s));

        public int CompareTo(Name other) => this.value.CompareTo(other.value);

        public bool Equals(Name other) => this.value == other.value;

        public override int GetHashCode() => this.value.GetHashCode();

        public override bool Equals(object obj) => obj is Name n && this.Equals(n);

        public static bool operator ==(Name x, Name y) => x.Equals(y);

        public static bool operator !=(Name x, Name y) => !x.Equals(y);

        public static implicit operator string(Name n) => n.value;
    }
}
