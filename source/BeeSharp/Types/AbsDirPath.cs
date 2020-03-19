using System;
using System.IO;
using BeeSharp.Extensions;

using static BeeSharp.Internal.PathStringUtils;

namespace BeeSharp.Types
{
    public partial struct AbsDirPath : IConstrainedType<AbsDirPath>
    {
        private readonly string value;

        private AbsDirPath(string p) => this.value = p;

        public static readonly AbsDirPath Current = Of(Directory.GetCurrentDirectory()).Unwrap();

        public static AbsDirPath New(string s) => new AbsDirPath(Check(s));

        public static AbsDirPath UncheckedNew(string s) => new AbsDirPath(s);

        public static Res<AbsDirPath> Of(string s) => Res.Try(() => new AbsDirPath(Check(Fixup(s))));

        public int CompareTo(AbsDirPath other) => this.value.CompareTo(other.value);

        public bool Equals(AbsDirPath other) => this.value.Equals(other.value, PathComparisonType);

        public override bool Equals(object obj) => obj is AbsDirPath p && this.Equals(p);

        public override int GetHashCode() => this.value.GetHashCode();

        public static implicit operator string(AbsDirPath p) => p.value;

        public static AbsFilePath operator /(AbsDirPath d, FileName f)
            => AbsFilePath.UncheckedNew($"{(string)d}{(string)f}");

        public static AbsDirPath operator /(AbsDirPath ap, RelDirPath rp)
            => new AbsDirPath($"{Path.GetFullPath($"{(string)ap}{(string)rp}")}");

        public override string ToString() => this.value;

        private static string Fixup(string s)
            => Path.GetFullPath(
                s.Trim()
                .Replace(": ", ":") // Get rid of local disk relative paths
                .Replace(" :", ":") // Get rid of local disk relative paths
                .EnsureEndsWith(PathSeparator));

        private static string Check(string s)
        {
            int ti;
            if (!Path.IsPathRooted(s)) { throw new ArgumentException($"'{s}' is not an absolute directory path as it is not rooted."); }
            if (!s.EndsWith(PathSeparator)) { throw new ArgumentException($"'{s}' is not an absolute directory path as it does not end with '{PathSeparator}'."); }
            if ((ti = s.LastIndexOf(':')) > 1) { throw new ArgumentException($"Absolute directory path '{s}' contains invalid ':' at index {ti}."); }

            var _ = Path.GetFullPath(s);

            return s;
        }
    }
}
