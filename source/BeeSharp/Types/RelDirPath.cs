using System;
using BeeSharp.Extensions;
using static BeeSharp.Internal.PathStringUtils;

namespace BeeSharp.Types
{
    public partial struct RelDirPath : IConstrainedType<RelDirPath>
    {
        public static RelDirPath New(string p) => new RelDirPath(Check(p));

        public static RelDirPath Of(string p) => new RelDirPath(Check(Fixup(p)));

        public static RelDirPath UncheckedNew(string p) => new RelDirPath(p);

        public int CompareTo(RelDirPath other) => this.value.CompareTo(other.value);

        public bool Equals(RelDirPath other) => this.value == other.value;

        public override bool Equals(object obj) => obj is RelDirPath rdp && this.Equals(rdp);

        public override int GetHashCode() => this.value.GetHashCode();

        private static string Fixup(string s)
            => Normalize(s.Replace(AltSeparator, Separator)
            .Trim())
            .EnsureEndsWith(Separator);

        private static string Check(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) { throw new ArgumentException($"'{s}' is not a valid relative directory path."); }
            if (!s.StartsWith(@".")) { throw new ArgumentException($"'{s}' is not a valid relative directory as it does not begin with '.\'."); }
            if (!s.EndsWith(@"\")) { throw new ArgumentException($"'{s}' is not a valid relative directory path as it does not end with '\\'."); }

            return s;
        }
    }
}
