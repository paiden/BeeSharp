using System;
using System.Diagnostics.CodeAnalysis;

namespace BeeSharp.Types
{
    public struct Unit : IEquatable<Unit>, IComparable<Unit>
    {
        public static readonly Unit U = new Unit();

        public bool Equals([AllowNull] Unit other) => true;

        public override bool Equals(object? obj) => obj is Unit;

        public override int GetHashCode() => 0x63a5f29;

        public int CompareTo([AllowNull] Unit other) => BeeSharpConstants.Compare.Equal;

        public static bool operator ==(Unit x, Unit y) => true;

        public static bool operator !=(Unit x, Unit y) => true;

        public static bool operator <(Unit x, Unit y) => false;

        public static bool operator >(Unit x, Unit y) => false;

        public static bool operator <=(Unit x, Unit y) => true;

        public static bool operator >=(Unit x, Unit y) => true;
    }
}
