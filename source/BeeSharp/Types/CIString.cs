using System;
using System.Diagnostics;
using BeeSharp.Validation;

namespace BeeSharp.Types
{
    /// <summary>
    /// Data type representing a case insensitive string (OrdinalIgnoreCase).
    /// </summary>
    /// <remarks>
    /// Only Equals and Equality operator ignore case. All other things like comparison still
    /// care about the case of the string.
    /// </remarks>
    [DebuggerDisplay("{value, nq}")]
    public struct CIString : IEquatable<CIString>, IComparable<CIString>, IPreventDefaultConstruction
    {
        private readonly string value;

        private CIString(string s) => this.value = s;

        public static CIString Of(string s) => new CIString(s ?? string.Empty);

        public static CIString New(string s) => new CIString(s.CheckNotNull(nameof(s)));

        public static CIString UncheckedNew(string s)
        {
            AssertCIString(s);
            return new CIString(s);
        }

        public int CompareTo(CIString other)
            => this.value.CompareTo(other.value);

        public bool Equals(CIString other)
            => this.value.Equals(other, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object obj)
            => obj is CIString cis && cis.Equals(this);

        public override int GetHashCode()
            => this.value.GetHashCode();

        public static bool operator ==(CIString x, CIString y)
            => x.Equals(y);

        public static bool operator !=(CIString x, CIString y)
            => !x.Equals(y);

        public static implicit operator string(CIString s) => s.value;

        [Conditional(BeeSharpConstants.DbgCond)]
        private static void AssertCIString(string s)
            => Debug.Assert(s != null, "string should never be null when constructed with fast creator.");
    }
}
