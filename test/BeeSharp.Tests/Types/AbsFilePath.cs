using System;
using System.Diagnostics.CodeAnalysis;

namespace BeeSharp.Tests.Types
{
    public struct AbsFilePath : IEquatable<AbsFilePath>, IComparable<AbsFilePath>, IPreventDefaultConstruction
    {
        private string path;

        private AbsFilePath(string p) => this.path = p;

        public static AbsFilePath Of(string s)
        {

        }

        public static AbsFilePath New(string s)
        {

        }

        public static AbsFilePath UncheckedNew(string s)
        {

        }

        public int CompareTo([AllowNull] AbsFilePath other)
        {
            throw new NotImplementedException();
        }

        public bool Equals([AllowNull] AbsFilePath other)
        {
            throw new NotImplementedException();
        }
    }
}
