using System;
using System.IO;

namespace BeeSharp.Types
{
    public partial struct FileName : IConstrainedType<FileName>
    {
        public static FileName New(string s) => new FileName(Check(s));

        public static Res<FileName> Of(string s) => Res.Try(() => new FileName(Check(Fixup(s))));

        public static FileName UncheckedNew(string s) => new FileName(s);

        public int CompareTo(FileName other) => this.value.CompareTo(other.value);

        public bool Equals(FileName other) => this.value == other.value;

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
