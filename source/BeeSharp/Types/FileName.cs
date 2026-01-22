using System;
using System.IO;

using static BeeSharp.Internal.PathStringUtils;

namespace BeeSharp.Types
{
    public partial struct FileName : IConstrainedType<FileName>
    {
        private readonly string value;

        public FileName(string n) => this.value = n;

        public static FileName New(string s) => new FileName(Check(s));

        public static R<FileName> Of(string s) => R<FileName>.Try(() => new FileName(Check(Fixup(s))));

        public static FileName UncheckedNew(string s) => new FileName(s);

        public static implicit operator string(FileName fn) => fn.value;

        public int CompareTo(FileName other) => this.value.CompareTo(other.value);

        public bool Equals(FileName other) => this.value.Equals(other.value, PathComparisonType);

        public override bool Equals(object obj) => obj is FileName fn && this.Equals(fn);

        public override int GetHashCode() => this.value.GetHashCode();

        private static string Check(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) { throw new ArgumentException($"'{s}' is not a valid filename."); }

            foreach (var ic in Path.GetInvalidFileNameChars())
            {
                if (s.Contains(ic)) { throw new ArgumentException($"'{s}' is not a valid filename as it contains disallowed character '{ic}'."); }
            }

            return s;
        }

        private static string Fixup(string s) => s.Trim();
    }
}
