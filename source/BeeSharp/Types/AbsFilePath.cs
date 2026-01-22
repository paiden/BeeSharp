using System;
using System.IO;

using static BeeSharp.Internal.PathStringUtils;

namespace BeeSharp.Types
{
    public partial struct AbsFilePath : IConstrainedType<AbsFilePath>
    {
        private readonly string value;

        private AbsFilePath(string p) => this.value = p;

        public static AbsFilePath New(string p) => new AbsFilePath(Check(p));

        public static AbsFilePath UncheckedNew(string p) => new AbsFilePath(p);

        public static R<AbsFilePath> Of(string p) => R<AbsFilePath>.Try(() => new AbsFilePath(Check(Fixup(p))));

        public static bool operator ==(AbsFilePath x, AbsFilePath y) => x.Equals(y);

        public static bool operator !=(AbsFilePath x, AbsFilePath y) => !x.Equals(y);

        public static implicit operator string(AbsFilePath p) => p.value;

        public int CompareTo(AbsFilePath other) => this.value.CompareTo(other.value);

        public bool Equals(AbsFilePath other) => this.value.Equals(other.value, PathComparisonType);

        public override bool Equals(object obj) => obj is AbsFilePath fp && this.Equals(fp);

        public override int GetHashCode() => this.value.GetHashCode();

        private static string Check(string s)
        {
            if (!Path.IsPathRooted(s)) { throw new ArgumentException($"'{s}' is not an absolute file path as it is not rooted."); }
            if (Path.GetFileName(s).Length <= 0) { throw new ArgumentException($"'{s}' is not an absolute file path as it does not contain a file name."); }

            return s;
        }

        private static string Fixup(string s)
            => Path.GetFullPath(s.Trim()
                .Replace(": ", ":") // Get rid of local disk relative paths
                .Replace(" :", ":")); // Get rid of local disk relative paths

        public override string ToString() => this.value;
    }
}
