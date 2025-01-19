using System;
using System.Diagnostics;
using BeeSharp.Validation;

namespace BeeSharp.Types
{
    public struct Trimmed : IEquatable<Trimmed>, IComparable<Trimmed>, IPreventDefaultConstruction
    {
        private readonly string value;

        private Trimmed(string s) => this.value = s;

        public static Trimmed Of(string s) => new Trimmed(s?.Trim() ?? string.Empty);

        public static Trimmed New(string s) => new Trimmed(s.CheckTrimmed(nameof(s)));

        public static Trimmed UncheckedNew(string s)
        {
            AssertTrimmed(s);
            return new Trimmed(s);
        }

        public static implicit operator string(Trimmed t) => t.value;

        [Conditional(BeeSharpConstants.DbgCond)]
        private static void AssertTrimmed(string s)
        {
            Debug.Assert(s.IsTrimmed());
        }

        public int CompareTo(Trimmed other) => this.value.CompareTo(other.value);

        public bool Equals(Trimmed other) => this.value == other.value;

        public override bool Equals(object obj) => obj is Trimmed t && this.Equals(t);

        public override int GetHashCode() => this.value.GetHashCode();

        public static bool operator ==(Trimmed x, Trimmed y) => x.Equals(y);

        public static bool operator !=(Trimmed x, Trimmed y) => !x.Equals(y);
    }
}
