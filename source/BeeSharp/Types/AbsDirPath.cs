﻿using System;
using System.IO;
using BeeSharp.Extensions;

namespace BeeSharp.Types
{
    public partial struct AbsDirPath : IConstrainedType<AbsDirPath>
    {
        public static AbsDirPath New(string s) => new AbsDirPath(Check(s));

        public static AbsDirPath UncheckedNew(string s) => new AbsDirPath(s);

        public static Res<AbsDirPath> Of(string s) => Res.Try(() => new AbsDirPath(Check(Fixup(s))));

        public int CompareTo(AbsDirPath other) => this.value.CompareTo(other.value);

        public bool Equals(AbsDirPath other) => this.value == other.value;

        public override bool Equals(object obj) => obj is AbsDirPath p && this.Equals(p);

        public override int GetHashCode() => this.value.GetHashCode();

        public static implicit operator string(AbsDirPath p) => p.value;

        public override string ToString() => this.value;

        private static string Fixup(string s)
            => Path.GetFullPath(
                s.Trim()
                .Replace(": ", ":") // Get rid of local disk relative paths
                .Replace(" :", ":") // Get rid of local disk relative paths
                .EnsureEndsWith(SepChar));

        private static string Check(string s)
        {
            if (!Path.IsPathRooted(s)) { throw new ArgumentException($"'{s}' is not an absolute directory path as it is not rooted."); }
            if (!s.EndsWith(SepChar)) { throw new ArgumentException($"'{s}' is not an absolute directory path as it does not end with '{SepChar}'."); }

            var _ = Path.GetFullPath(s);

            return s;
        }

    }
}